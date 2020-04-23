using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using MainMusicStore.DataAccess.IMainRepository;
using MainMusicStore.Models.DbModels;
using MainMusicStore.Models.ViewModels;
using MainMusicStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace MainMusicStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ProjectConstant.Role_Admin)]
    public class ProductController : Controller
    {

        #region Variables
        private readonly IUnitOfWork _uow;
        private readonly IWebHostEnvironment _hostEnvironment;
        AmazonS3Client client;
        public BasicAWSCredentials credentials = new BasicAWSCredentials(AWSSettings.AccessKeyID, AWSSettings.SecretKeyID);
        string bucketName = "mytestudemybucketapp1";

        #endregion

        #region CTOR
        public ProductController(IUnitOfWork uow, IWebHostEnvironment hostEnvironment)
        {
            _uow = uow;
            _hostEnvironment = hostEnvironment;
            client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.EUWest1);

        }
        #endregion

        #region Actions
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region API CALLS
        public IActionResult GetAll()
        {
            var allObj = _uow.Product.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var deleteData = _uow.Product.Get(id);
            if (deleteData == null)
                return Json(new { success = false, message = "Data Not Found!" });

            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, deleteData.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _uow.Product.Remove(deleteData);
            _uow.Save();
            return Json(new { success = true, message = "Delete Operation Successfully" });
        }

        #endregion

        /// <summary>
        /// Create Or Update Get Method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _uow.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.CategoryName,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _uow.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
                return View(productVM);

            productVM.Product = _uow.Product.Get(id.GetValueOrDefault());
            if (productVM.Product == null)
                return NotFound();
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async System.Threading.Tasks.Task<IActionResult> Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {

                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\products");
                    var extenstion = Path.GetExtension(files[0].FileName);

                    if (productVM.Product.ImageUrl != null)
                    {
                        var imageUrl = productVM.Product.ImageUrl;
                        var imagePath = Path.Combine(webRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    var pathRoot = AppDomain.CurrentDomain.BaseDirectory;
                    using (var fileStreams = new FileStream(Path.Combine(pathRoot, fileName + extenstion), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    //TODO:CreateBucket
                    try
                    {
                        if (await AmazonS3Util.DoesS3BucketExistAsync(client, bucketName))
                        {
                            throw new Exception("Oluşturulmak istenilen Bucket Zaten Mevcut");
                        }
                        else
                        {
                            var bucketRequest = new PutBucketRequest
                            {
                                BucketName = bucketName,
                                UseClientRegion = true
                            };

                            var bucketResponse = client.PutBucketAsync(bucketRequest);
                            if (bucketResponse.Result.HttpStatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var transferUtility = new TransferUtility(client);
                                var transferRequest = new TransferUtilityUploadRequest
                                {
                                    FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + fileName + extenstion,
                                    BucketName = bucketName,
                                    CannedACL = S3CannedACL.PublicRead
                                };
                                transferUtility.Upload(transferRequest);
                            }
                        }
                    }
                    catch (AmazonS3Exception e)
                    {
                        Console.WriteLine(e.Message.ToString());
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                    }

                    //productVM.Product.ImageUrl = @"\images\products\" + fileName + extenstion;
                }
                else
                {
                    if (productVM.Product.Id != 0)
                    {
                        var productData = _uow.Product.Get(productVM.Product.Id);
                        productVM.Product.ImageUrl = productData.ImageUrl;
                    }
                }

                if (productVM.Product.Id == 0)
                {
                    //Create
                    _uow.Product.Add(productVM.Product);
                }
                else
                {
                    //Update
                    _uow.Product.Update(productVM.Product);
                }
                _uow.Save();
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _uow.Category.GetAll().Select(a => new SelectListItem
                {
                    Text = a.CategoryName,
                    Value = a.Id.ToString()
                });

                productVM.CoverTypeList = _uow.CoverType.GetAll().Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                });

                if (productVM.Product.Id != 0)
                {
                    productVM.Product = _uow.Product.Get(productVM.Product.Id);
                }
            }
            return View(productVM.Product);
        }
    }
}

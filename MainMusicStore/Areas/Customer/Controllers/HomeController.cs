using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MainMusicStore.Models;
using MainMusicStore.DataAccess.IMainRepository;
using MainMusicStore.Models.DbModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MainMusicStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _uow;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork uow)
        {
            _logger = logger;
            _uow = uow;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _uow.Product.GetAll(includeProperties: "Category,CoverType");
            return View(productList);
        }

        public IActionResult Details(int id)
        {
            var product = _uow.Product.GetFirstOrDefault(p => p.Id == id, includeProperties: "Category,CoverType");

            ShoppingCart cart = new ShoppingCart()
            {
                Product = product,
                ProductId = product.Id
            };
            return View(cart);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart cartObj)
        {
            cartObj.Id = 0;
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                cartObj.ApplicationUserId = claim.Value;

                ShoppingCart fromDb = _uow.ShoppingCart.GetFirstOrDefault(
                    s => s.ApplicationUserId == cartObj.ApplicationUserId
                    && s.ProductId == cartObj.ProductId,
                    includeProperties: "Product");

                if (fromDb == null)
                {
                    //Insert
                    _uow.ShoppingCart.Add(cartObj);
                }
                else
                {
                    //Update
                    fromDb.Count += cartObj.Count;
                }

                _uow.Save();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                var product = _uow.Product.GetFirstOrDefault(p => p.Id == cartObj.ProductId, includeProperties: "Category,CoverType");

                ShoppingCart cart = new ShoppingCart()
                {
                    Product = product,
                    ProductId = product.Id
                };
                return View(cart);
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

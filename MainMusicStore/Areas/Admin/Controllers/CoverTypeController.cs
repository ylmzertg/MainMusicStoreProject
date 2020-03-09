using Dapper;
using MainMusicStore.DataAccess.IMainRepository;
using MainMusicStore.Models.DbModels;
using MainMusicStore.Utility;
using Microsoft.AspNetCore.Mvc;

namespace MainMusicStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        #region Variables
        private readonly IUnitOfWork _uow;
        #endregion

        #region CTOR
        public CoverTypeController(IUnitOfWork uow)
        {
            _uow = uow;
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
            //var allObj = _uow.CoverType.GetAll();
            var allCoverTypes = _uow.sp_call.List<CoverType>(ProjectConstant.Proc_CoverType_GetAll, null);
            return Json(new { data = allCoverTypes });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            //var deleteData = _uow.CoverType.Get(id);
            //if (deleteData == null)
            //    return Json(new { success = false,message ="Data Not Found!"});

            //_uow.CoverType.Remove(deleteData);
            //_uow.Save();
            //return Json(new { success = true ,message ="Delete Operation Successfully"});
            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);

            var deleteData = _uow.sp_call.OneRecord<CoverType>(ProjectConstant.Proc_CoverType_Get, parameter);
            if (deleteData == null)
                return Json(new { success = false, message = "Data Not Found!" });

            _uow.sp_call.Execute(ProjectConstant.Proc_CoverType_Delete, parameter);
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
            CoverType coverType = new CoverType();
            if (id == null)
            {
                //This for Create
                return View(coverType);
            }

            var parameter = new DynamicParameters();
            parameter.Add("@Id",id);
            coverType = _uow.sp_call.OneRecord<CoverType>(ProjectConstant.Proc_CoverType_Get, parameter);

            //cat = _uow.CoverType.Get((int)id);
            if (coverType != null)
                return View(coverType);
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType CoverType)
        {
            if (ModelState.IsValid)
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Name", CoverType.Name);
                if (CoverType.Id == 0)
                {
                    //Create
                    //_uow.CoverType.Add(CoverType);
                    _uow.sp_call.Execute(ProjectConstant.Proc_CoverType_Create, parameter);
                }
                else
                {
                    //Update
                    parameter.Add("@Id", CoverType.Id);
                    //_uow.CoverType.Update(CoverType);
                    _uow.sp_call.Execute(ProjectConstant.Proc_CoverType_Update, parameter);
                }
                _uow.Save();
                return RedirectToAction("Index");
            }
            return View(CoverType);
        }
    }
}
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
           
        }


        public IActionResult Index()
        {
            var objCompanyList = _unitOfWork.Company.GetAll().ToList();
           
            return View(objCompanyList);
        }


        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                // This is for create
                return View(new Company());
            }
            else
            {
                // This is for update
                Company companyObj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyObj);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {
            if (ModelState.IsValid)
            {
               
                if (CompanyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(CompanyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(CompanyObj);
                }

                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View(CompanyObj);
            }
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var Company = _unitOfWork.Company.Get(u => u.Id == id);
            if (Company == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Company.Delete(Company);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        } 
    }
}

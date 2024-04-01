using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace N_Tier.BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController(IUnitOfWork unitOfWork) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public IActionResult Index()
        {
            var listCompanies = _unitOfWork.Company.GetAll();
            return View(listCompanies);
        }

        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Company objCompany)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Company.Add(objCompany);
                _unitOfWork.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Company objCompany = _unitOfWork.Company.Get(x => x.Id == id);
            if (objCompany == null)
            {
                return NotFound();
            }
            return View(objCompany);
        }

        [HttpPost]
        public IActionResult Edit(Company objCompany) 
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Company.Update(objCompany);
                _unitOfWork.Save();
                TempData["success"] = "Company updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        /*public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Company? objCompany = _unitOfWork.Company.Get(x => x.Id == id);

            if (objCompany == null)
            {
                return NotFound();
            }
            return View(objCompany);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Company? objCompany = _unitOfWork.Company.Get(x => x.Id == id);
            if (objCompany == null)
            {
                return NotFound();
            }
            _unitOfWork.Company.Remove(objCompany);
            _unitOfWork.Save();
            TempData["success"] = "Company deleted successfully";
            return RedirectToAction("Index");
        }*/
        #region API Calls
        public IActionResult GetAll()
        {
            var objCompany = _unitOfWork.Company.GetAll();

            return Json(new { data = objCompany });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDelete = _unitOfWork.Company.Get(u => u.Id == id);

            if (companyToBeDelete == null)
            {
                return Json(new { success = false, message = "Error: Could not find the data" });
            }

            _unitOfWork.Company.Remove(companyToBeDelete);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}

using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace N_Tier.BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
            return View(objProductList);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            ViewBag.CategoryList = categoryList;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product) {

            if (ModelState.IsValid) {
                _unitOfWork.Product.Add(product);
                _unitOfWork.Save();
                TempData["success"] = "Product has been created";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Product has not been created";
            return View();
        }


        public IActionResult Edit(int id) {
            Product result = _unitOfWork.Product.Get(x => x.Id == id);

            if (result == null) { 
                return NotFound();
            }
            return View(result);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid) { 
                _unitOfWork.Product.Update(product);
                _unitOfWork.Save();
                TempData["success"] = "Product has been updated";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Product has not been updated";
            return View();
        }


        public IActionResult Delete(int id)
        {
            Product result = _unitOfWork.Product.Get(x => x.Id == id);

            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteProduct(int id) {

            var deletionProduct = _unitOfWork.Product.Get(x=>x.Id == id);

            _unitOfWork.Product.Remove(deletionProduct);
            _unitOfWork.Save();

            TempData["success"] = "Product has been deleted";
            return RedirectToAction("Index");
        }
    }
}

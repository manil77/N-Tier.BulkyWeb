using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
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

        public IActionResult Upsert(int? id)
        {

            ProductVm productVm = new()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            if (id == null || id == 0)
            {
                //create
                return View(productVm); 
            }
            else {
                //update
                productVm.Product = _unitOfWork.Product.Get(x => x.Id == id);

                return View(productVm);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVm productVm, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(productVm.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product has been created";
                return RedirectToAction("Index");
            }
            else
            {
                productVm.CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
                TempData["error"] = "Product has not been created";
                return View(productVm);
            }
        }


        public IActionResult Edit(int id)
        {
            Product result = _unitOfWork.Product.Get(x => x.Id == id);

            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
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
        public IActionResult DeleteProduct(int id)
        {

            var deletionProduct = _unitOfWork.Product.Get(x => x.Id == id);

            _unitOfWork.Product.Remove(deletionProduct);
            _unitOfWork.Save();

            TempData["success"] = "Product has been deleted";
            return RedirectToAction("Index");
        }
    }
}

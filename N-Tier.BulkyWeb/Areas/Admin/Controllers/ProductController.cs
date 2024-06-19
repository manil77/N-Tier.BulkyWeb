using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;

namespace N_Tier.BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(null, "Category").ToList();
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
            else
            {
                //update
                productVm.Product = _unitOfWork.Product.Get(x => x.Id == id);

                return View(productVm);
            }
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVm obj, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;

                }
                if (obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        /*[HttpPost]
        public IActionResult Upsert(ProductVm productVm, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var productPath = Path.Combine(wwwRootPath, @"images\products");

                    if (!string.IsNullOrEmpty(productVm.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVm.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVm.Product.ImageUrl = @"\images\products\" + fileName;
                }

                if (productVm.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVm.Product);
                }
                else {
                    _unitOfWork.Product.Update(productVm.Product);
                }
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
        }*/


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


        /*public IActionResult Delete(int id)
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
        }*/

        #region API Calls
        [HttpGet]

        public IActionResult GetAll() {
            var result = _unitOfWork.Product.GetAll(null, "Category");

            return Json( new { data = result});
        }

        [HttpDelete]
        public IActionResult Delete(int? id) 
        {

            var productToBeDelete = _unitOfWork.Product.Get(u => u.Id == id);

            if (productToBeDelete == null) {
                return Json(new { success = false, message = "Error: Could not find the data" });
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                productToBeDelete.ImageUrl.TrimStart('\\'));


            if (System.IO.File.Exists(oldImagePath)) {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToBeDelete);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}

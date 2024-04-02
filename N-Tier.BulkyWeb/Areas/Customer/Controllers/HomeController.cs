using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace N_Tier.BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll(includeProperties:"Category");    
            return View(products);
        }

        public IActionResult Detail(int id)
        {
           
            ShoppingCart objShoppingCart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == id, includeProperties: "Category"),
                Count = 1
            };
            objShoppingCart.ProductId = objShoppingCart.Product.Id; 
            return View(objShoppingCart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Detail(ShoppingCart objShoppingCart)
        {
            objShoppingCart.Id = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value; //ClaimsIdentity are provided by dot net team

            objShoppingCart.ApplicationUserId = userId;

            _unitOfWork.ShoppingCart.Add(objShoppingCart);
            _unitOfWork.Save();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

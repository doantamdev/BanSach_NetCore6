using BanSach.DataAccess.Repository.IRepository;
using BanSach.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;


namespace WebBanSach.Areas.Customer.Controllers
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
			IEnumerable<Product> ProductList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
			return View(ProductList);
		}
        public IActionResult Details(int productId)
        {
            ShoppingCart cartObj = new()
            {
                Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == productId, includeProperties: "Category,CoverType"),
                Count=1,
                ProductId= productId
            };

            return View(cartObj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity; //danh tính người dùng
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier); //tên định danh
            shoppingCart.ApplicationUserId = claim.Value;

            //tìm giỏ hàng có trùng không
            ShoppingCart cartObj = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                    u => u.ApplicationUserId == claim.Value && u.ProductId==shoppingCart.ProductId);

            if (cartObj==null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }
            else
            {
                _unitOfWork.ShoppingCart.IncreamentCount(cartObj, shoppingCart.Count);
            }

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index)); //quay về Index
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

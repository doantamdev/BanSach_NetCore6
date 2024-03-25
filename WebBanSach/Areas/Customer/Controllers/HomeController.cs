using BanSach.DataAccess.Repository.IRepository;
using BanSach.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration;
using QRCoder;
using Stripe.Climate;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Claims;
using WebBanSach.Areas.StrateryPattern;
using WebBanSach.StrategyPattern;
using static System.Runtime.CompilerServices.RuntimeHelpers;


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

        public IActionResult Index(string order = null)
        {
			IEnumerable<BanSach.Models.Product> ProductList;
            OrdersContext ordersContext;
			IStrategy Orders;

			switch (order)
			{
				case "asending":
					Orders = new AsendingOrders(_unitOfWork);

                    ordersContext = new OrdersContext(Orders);
					ProductList = ordersContext.ExcuteOrders();

					break;

				case "desending":
					Orders = new DesendingOrders(_unitOfWork);

					ordersContext = new OrdersContext(Orders);
					ProductList = ordersContext.ExcuteOrders();

					break;

				default:
					ProductList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
					break;
			}

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

			var product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == productId, includeProperties: "Category,CoverType");

			QRCodeGenerator QrGenerator = new QRCodeGenerator();
			QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(product.Title, QRCodeGenerator.ECCLevel.Q);
			QRCode QrCode = new QRCode(QrCodeInfo);
			Bitmap QrBitmap = QrCode.GetGraphic(40);
			byte[] BitmapArray = QrBitmap.BitmapToByteArray();
			string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
			ViewBag.QRCodeImageBytes = QrUri;

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

	public static class BitmapExtension
	{
		public static byte[] BitmapToByteArray(this Bitmap bitmap)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				bitmap.Save(ms, ImageFormat.Png);
				return ms.ToArray();
			}
		}
	}
}

using BanSach.DataAccess.Repository.IRepository;
using BanSach.Models;
using BanSach.Models.ViewModel;
using BanSach.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Stripe.TestHelpers;
using System.Diagnostics;
using System.Security.Claims;
using RefundService = Stripe.RefundService;

namespace WebBanSach.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [BindProperties]
	public class OrderController : Controller
	{
        private readonly IUnitOfWork _unitOfWork;
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
		public IActionResult Index()
		{
			return View();
		}

        public IActionResult Details(int orderId)
        {
            OrderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderId,
                        includeProperties: "ApplicationUser"),
                OrderDetails = _unitOfWork.OrderDetail.GetAll(u => u.OrderId == orderId, includeProperties: "Product")

            };
            return View(OrderVM);
        }

        [ActionName("Details")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details_PAY_NOW()
        {
            OrderVM.OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id,
                        includeProperties: "ApplicationUser");
            OrderVM.OrderDetails = _unitOfWork.OrderDetail.GetAll(u => u.OrderId == OrderVM.OrderHeader.Id,
                        includeProperties: "Product");

            // Payment settings
            var domain = "https://localhost:44307/";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions>()
                ,
                Mode = "payment",
                SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderHeaderid=={OrderVM.OrderHeader.Id}",
                CancelUrl = domain + $"admin/order/details?orderId={OrderVM.OrderHeader.Id}",
            };

            foreach (var item in OrderVM.OrderDetails)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        },
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItem);

            }

            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.OrderHeader.UpdateStripePaymentID(OrderVM.OrderHeader.Id, session.Id, 
                        session.PaymentIntentId);
            //ShoppingCartVM.OrderHeader.SessionId=session.Id; 
            //ShoppingCartVM.OrderHeader.PaymentIntentId=session.PaymentIntentId;
            _unitOfWork.Save();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
            
        }

        public IActionResult PaymentConfirmation(int orderHeaderid)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderHeaderid);
            if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);
                // check the stripe status
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentID(orderHeaderid, orderHeader.SessionId,
                        session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(orderHeaderid, orderHeader.OrderStatus, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }

            return View(orderHeader);
        }

        [HttpPost]
        [Authorize(Roles =SD.Role_User_Admin+","+SD.Role_User_Employee)]
        [ValidateAntiForgeryToken]

        public IActionResult UpdateOrderDetail()
        {
            var orderheaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(u=>u.Id==OrderVM.OrderHeader.Id,
                                    tracked:false);
            orderheaderFromDb.Name = OrderVM.OrderHeader.Name;
            orderheaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderheaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
            orderheaderFromDb.City = OrderVM.OrderHeader.City;
            orderheaderFromDb.State = OrderVM.OrderHeader.State;
            orderheaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;
            if (OrderVM.OrderHeader.Carrier!=null)
            {
                orderheaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
            }
            if (OrderVM.OrderHeader.TrackingNumber != null)
            {
                orderheaderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            }
            //_unitOfWork.OrderHeader.Update(orderheaderFromDb);
            _unitOfWork.Save();
            TempData["Success"] = "Order Details Updated Successfully.";
            return RedirectToAction("Details", "Order", new { orderId = orderheaderFromDb.Id });           
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_User_Admin + "," + SD.Role_User_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult StartProcessing()
        {
            _unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id,SD.StatusInProcess);
            _unitOfWork.Save();
            TempData["Success"] = "Order Status Updated Successfully.";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_User_Admin + "," + SD.Role_User_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult ShipOrder()
        {
            var orderheaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id,
                                   tracked: false);
            orderheaderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderheaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
            orderheaderFromDb.OrderStatus = SD.StatusShipped;
            orderheaderFromDb.ShipppingDate = DateTime.Now ;
            if (orderheaderFromDb.PaymentStatus==SD.PaymentStatusDelayedPayment)
            {
                orderheaderFromDb.PaymentDueDate = DateTime.Now.AddDays(30);
            }
            _unitOfWork.OrderHeader.Update(orderheaderFromDb);           
            _unitOfWork.Save();
            TempData["Success"] = "Order Shipped Successfully.";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_User_Admin + "," + SD.Role_User_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder()
        {
            var orderheaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id,
                                   tracked: false);
            if (orderheaderFromDb.PaymentStatus==SD.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderheaderFromDb.PaymentIntentId
                };
                var service = new RefundService();
                Refund refund = service.Create(options);
                _unitOfWork.OrderHeader.UpdateStatus(orderheaderFromDb.Id, SD.StatusCancelled, SD.StatusRefunded);
            }
            else
            {
                _unitOfWork.OrderHeader.UpdateStatus(orderheaderFromDb.Id, SD.StatusCancelled, SD.StatusRefunded);
            }
            _unitOfWork.Save();

            TempData["Success"] = "Order Cancelled Successfully.";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders;
            if (User.IsInRole(SD.Role_User_Admin))
            {
                orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeaders = _unitOfWork.OrderHeader.GetAll(u=>u.ApplicationUserId==claim.Value,
                    includeProperties: "ApplicationUser");
            }
           

            switch (status)
            {
                case "pending":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusPending);
                    break;
                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                default:                  
                    break;

            }

            return Json(new { data = orderHeaders });
        }
        #endregion
    }
}

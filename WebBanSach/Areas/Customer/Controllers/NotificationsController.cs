using BanSach.DataAccess.Repository.IRepository;
using BanSach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WebBanSach.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class NotificationsController : Controller
    {
        INotiService _notiService;
        List<Noti> _oNotifications = new List<Noti>();
        ILogger<NotificationsController> _logger;

        public NotificationsController(INotiService notiService, ILogger<NotificationsController> logger)
        {
            _notiService = notiService;
            _logger = logger;
        }

        public ActionResult AllNotifications()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetNotifications(bool isGetOnlyUnread = false)
        {
            // Lấy UserId từ session
            string nIDUser = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(nIDUser))
            {
                return Json(new { error = "User not logged in" });
            }

            _oNotifications = new List<Noti>();
            _oNotifications = _notiService.GetNotification(nIDUser, isGetOnlyUnread);
            return Json(_oNotifications);
        }

    }
}

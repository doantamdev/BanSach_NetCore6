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
            _logger.LogError("da toi");
            string nIDUser = "3a67f634-82ec-4deb-ae25-973c4a56b20a";
            _oNotifications = new List<Noti>();
            _oNotifications = _notiService.GetNotification(nIDUser, isGetOnlyUnread);
            return Json(_oNotifications);
        }
    }
}

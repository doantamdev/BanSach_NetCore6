using BanSach.DataAccess.Data;
using BanSach.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace WebBanSach.Areas.CommandPattern.Controllers
{
    [Area("CommandPattern")]
    public class ResetPassController : PageModel
    {
        private readonly ApplicationDbContext _db;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ResetPassController> _logger;
        public ResetPassController(ApplicationDbContext db, UserManager<IdentityUser> userManager, ILogger<ResetPassController> logger)
        {
            _db = db;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> ResetPass(string id)
        {
            List<ICommand> commands = new List<ICommand>();
            Invoker invoker = new Invoker(commands);

            _logger.LogInformation("This is an informational log message.");

            if (id == null)
            {
                return RedirectToAction("Login", "Manager");
            }

            ResetPassword reset; 

            try
            {
                reset = new ResetPassword(_db, id, _userManager, _logger);

                invoker.AddCommand(reset);
                await invoker.ExecuteCommand();

                _logger.LogInformation("Success");
                //TempData["Success"] = "Reset thành công!";
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", "Có lỗi xảy ra:" + e.Message);
                invoker.UndoLastCommand();
            }

            return RedirectToAction("Manage", "Account", new {area="Identity"});
        }
    }
}

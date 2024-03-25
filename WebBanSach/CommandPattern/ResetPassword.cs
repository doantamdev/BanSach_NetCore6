using BanSach.DataAccess.Data;
using BanSach.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using WebBanSach.Areas.CommandPattern.Controllers;

namespace WebBanSach.Areas.CommandPattern
{
    public class ResetPassword : ICommand
    {
        private ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ResetPassController> _logger;
        private string _id;
        private string _PrevPass;

        public ResetPassword(ApplicationDbContext db, string id, UserManager<IdentityUser> userManager
            , ILogger<ResetPassController> logger)
        {
            _db = db;
            _id = id;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task Execute()
        {
            if (_id != null)
            {
                var User = await _userManager.FindByIdAsync(_id);

                if (User.Id == _id && User != null)
                {
                    _PrevPass = User.PasswordHash;

                    var token = await _userManager.GeneratePasswordResetTokenAsync(User);

                    string DefaultPass = "User123user";

                    var result = await _userManager.ResetPasswordAsync(User, token, DefaultPass);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Load success!");
                    }
                    else
                    {
                        Undo();
                    }
                }
                else
                {
                    _logger.LogInformation("User not found");
                    throw new InvalidOperationException("User not found.");
                }

            }
            else
            {
                _logger.LogInformation("Invalid user ID.");
                // Xử lý nếu ID không hợp lệ
                throw new InvalidOperationException("Invalid user ID.");
            }
        }

        public async Task Undo()
        {
            var User = await _userManager.FindByIdAsync(_id);

            var token = await _userManager.GeneratePasswordResetTokenAsync(User);
            _logger.LogInformation("Load token!");

            _logger.LogInformation("Load prev password!");

            var result = await _userManager.ResetPasswordAsync(User, token, _PrevPass);

            if (result.Succeeded)
            {
                _logger.LogInformation("Load pre password success!");
            }
        }
    }
}

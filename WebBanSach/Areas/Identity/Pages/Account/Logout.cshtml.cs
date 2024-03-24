// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebBanSach.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            // Đăng xuất người dùng
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            // Xóa dữ liệu trong session
            HttpContext.Session.Clear(); // Hoặc có thể sử dụng HttpContext.Session.Remove("UserId") nếu bạn chỉ muốn xóa một key cụ thể

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // Chuyển hướng đến trang chủ hoặc trang đăng nhập, tùy thuộc vào logic của bạn
                return RedirectToPage("/Index");
            }
        }

    }
}

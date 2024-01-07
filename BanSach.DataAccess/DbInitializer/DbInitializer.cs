using Bansach.Utility;
using BanSach.DataAccess.Data;
using BanSach.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanSach.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db= db;
        }
        public void Initialize()
        {
            // migration
            try
            {
                if (_db.Database.GetPendingMigrations().Count()>0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }
            // create roles default
            if (!_roleManager.RoleExistsAsync(SD.Role_User_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Indi)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Comp)).GetAwaiter().GetResult();

                //if roles are not created,then we will create admin user as well
                _userManager.CreateAsync(
                    new ApplicationUser
                    {
                        UserName="admin@gmail.com",
                        Email="admin@gmail.com",
                        Name="admin",
                        PhoneNumber="1212312345",
                        StreetAddress="134 Kim Ma P12",
                        State   ="TP",
                        PostalCode="700000",
                        City="Ho Chi Minh"
                    }, "Admin123*").GetAwaiter().GetResult(); //pass: Admin123*
                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@gmail.com");
                _userManager.AddToRoleAsync(user, SD.Role_User_Admin).GetAwaiter().GetResult();
            }

            return;

        }
    }
}

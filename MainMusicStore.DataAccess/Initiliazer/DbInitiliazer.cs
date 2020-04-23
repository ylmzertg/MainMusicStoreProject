using MainMusicStore.Data;
using MainMusicStore.Models.DbModels;
using MainMusicStore.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainMusicStore.DataAccess.Initiliazer
{
    public class DbInitializer : IDbInitiliazer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initiliaze()
        {
            //try
            //{
            //    if (_db.Database.GetPendingMigrations().Count() > 0)
            //    {
            //        _db.Database.Migrate();
            //    }
            //}
            //catch (Exception ex)
            //{

            //}

            //if (_db.Roles.Any(r => r.Name == ProjectConstant.Role_Admin)) return;

            //_roleManager.CreateAsync(new IdentityRole(ProjectConstant.Role_Admin)).GetAwaiter().GetResult();
            //_roleManager.CreateAsync(new IdentityRole(ProjectConstant.Role_Employee)).GetAwaiter().GetResult();
            //_roleManager.CreateAsync(new IdentityRole(ProjectConstant.Role_User_Comp)).GetAwaiter().GetResult();
            //_roleManager.CreateAsync(new IdentityRole(ProjectConstant.Role_User_Indi)).GetAwaiter().GetResult();

            //_userManager.CreateAsync(new ApplicationUser
            //{
            //    UserName = "ertugrulyilmaz@noktaatisi.com",
            //    Email = "admin@noktaatisi.com",
            //    EmailConfirmed = true,
            //    Name = "Ertugrul Yilmaz"
            //}, "Admin123*").GetAwaiter().GetResult();

            //ApplicationUser user = _db.ApplicationUsers.Where(u => u.Email == "admin@noktaatisi.com").FirstOrDefault();

            //_userManager.AddToRoleAsync(user, ProjectConstant.Role_Admin).GetAwaiter().GetResult();
        }
    }
}

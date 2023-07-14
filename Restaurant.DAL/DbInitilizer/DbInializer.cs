using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant.Models;
using Restaurant.Utilitiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DAL.DbInitilizer
{
    public class DbInializer : IDbInializer
    {
        private readonly RestaurantDBContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInializer(RestaurantDBContext db
            , UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public RestaurantDBContext Db { get; }
        public UserManager<IdentityUser> UserManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }

        public async Task Inialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {

                throw;
            }

            if (!await _roleManager.RoleExistsAsync(ConstRoleDef.ManagerRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(ConstRoleDef.ManagerRole));
                await _roleManager.CreateAsync(new IdentityRole(ConstRoleDef.KitchenRole));
                await _roleManager.CreateAsync(new IdentityRole(ConstRoleDef.FrontDeskRole));
                await _roleManager.CreateAsync(new IdentityRole(ConstRoleDef.CustomerRole));

                var user = new ApplicationUser()
                {
                    UserName = "user3",
                    Email = "user3@gmail.com",
                    EmailConfirmed = true,
                    FirstName = "Alaoui",
                    LastName = "dev",


                };
                await _userManager.CreateAsync(user, "admin123");
                ApplicationUser userApp = await _db.ApplicationUser
                    .FirstOrDefaultAsync(o => o.Email == "user3@gmail.com");
                await _userManager.AddToRoleAsync(userApp, ConstRoleDef.ManagerRole);



            }

        }
    }
}

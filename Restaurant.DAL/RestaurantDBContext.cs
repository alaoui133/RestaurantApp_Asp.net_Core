using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DAL
{
    public class RestaurantDBContext:IdentityDbContext
    {
        public RestaurantDBContext(DbContextOptions<RestaurantDBContext> options): base(options)
        {
            
        }
        public DbSet<Category> Category { get; set; }
        public DbSet<FoodType>FoodTypes { get; set; }
        public DbSet<MenuItem> MenuItem { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
    }
}

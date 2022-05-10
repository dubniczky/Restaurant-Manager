using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Restaurant.Core.Data;

namespace Restaurant.Server.Models
{
    public class RestaurantContext : IdentityDbContext<AdminUser, IdentityRole<int>, int>
    {
        public DbSet<Food> Foods { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OrderedFood> OrderedFoods { get; set; }

        public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}

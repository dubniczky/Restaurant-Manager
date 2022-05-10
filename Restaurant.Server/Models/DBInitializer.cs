using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Restaurant.Core.Data;

namespace Restaurant.Server.Models
{
    public class DBInitializer
    {
        private static RestaurantContext context;
        private static UserManager<AdminUser> userManager;
        private static RoleManager<IdentityRole<int>> roleManager;

        public static void Initialize(RestaurantContext context,
                                      UserManager<AdminUser> userManager,
                                      RoleManager<IdentityRole<int>> roleManager)
        {
            DBInitializer.context = context;
            DBInitializer.userManager = userManager;
            DBInitializer.roleManager = roleManager;

            context.Database.Migrate();

            //Init menu
            if (!context.Categories.Any())
            {
                InitializeFoods();
                InitializeCategories();
            }

            //Init users
            if (!context.Users.Any())
            {
                InitializeUsers();
            }
        }

        private static void InitializeUsers()
        {
            var admin = new AdminUser
            {
                UserName = "admin",
                Email = "admin@restaurant.com",
                PhoneNumber = "+36 99 999 9999",
            };
            var password = "root";
            var role = new IdentityRole<int>("administrator");

            var a = userManager.CreateAsync(admin, password).Result;
            var b = roleManager.CreateAsync(role).Result;
            var c = userManager.AddToRoleAsync(admin, role.Name).Result;
        }
        private static void InitializeFoods()
        {
            var foods = new Food[]
            {
                new Food
                {
                    Name = "Húsos Pizza",
                    Price = 3000,
                    OrderCount = 12,
                    Type = "pizza",
                    Description = "alap, szalámi, sajt",
                    Spicy = false,
                    Vegetarian = false
                },
                new Food
                {
                    Name = "Négysajtos Pizza",
                    Price = 3300,
                    OrderCount = 3,
                    Type = "pizza",
                    Description = "alap, szalámi, 4 féle sajt",
                    Spicy = false,
                    Vegetarian = true
                }
            };

            foreach (var f in foods)
            {
                context.Foods.Add(f);
            }
            context.SaveChanges();
        }
        private static void InitializeCategories()
        {
            var categories = new Category[]
            {
                new Category
                {
                    Name = "Pizzák",
                    Link = "Pizzas",
                    ComplexList = true,
                    TypeName = "pizza"
                },
                new Category
                {
                    Name = "Levesek",
                    Link = "Soups",
                    ComplexList = true,
                    TypeName = "soup"
                },
                new Category
                {
                    Name = "Üdítők",
                    Link = "Drinks",
                    ComplexList = false,
                    TypeName = "drink"
                },
            };

            foreach (var c in categories)
            {
                context.Categories.Add(c);
            }
            context.SaveChanges();
        }
    }
}

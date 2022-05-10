using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

using Restaurant.Server.Models;
using Restaurant.Core.Data;

namespace Restaurant.Test
{
    public static class TestDbInitializer
    {
        public static void Initialize(RestaurantContext context)
        {
            IList<Food> defaultFoods = new List<Food>
            {
                new Food
                {
                    Id = 1,
                    Name = "Nagy pizza",
                    Description = "really big",
                    Price = 5999,
                    Spicy = true,
                    Vegetarian = false,
                    Type = "pizza",
                },
                new Food
                {
                    Id = 2,
                    Name = "Kis pizza",
                    Description = "really small",
                    Price = 1999,
                    Spicy = false,
                    Vegetarian = true,
                    Type = "pizza",
                },
                new Food
                {
                    Id = 3,
                    Name = "Nagy leves",
                    Description = "really big soup",
                    Price = 2399,
                    Spicy = true,
                    Vegetarian = true,
                    Type = "soup",
                },
            };
            IList<Category> defaultCategories = new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Pizzák",
                    Link = "Pizzas",
                    TypeName = "pizza",
                    ComplexList = true,
                },
                new Category
                {
                    Id = 2,
                    Name = "Levese",
                    Link = "Soups",
                    TypeName = "soup",
                    ComplexList = true,
                },
            };
            IList<Order> defaultOrders = new List<Order>
            {
                new Order
                {
                    Id = 1,
                    Name = "Jani",
                    Address = "Jani's address",
                    Price = 9999,
                    Date = DateTime.Now,
                    Completed = false,
                    Phone = "+36 99 999 9999",
                    CompleteTime = DateTime.Now,
                },
            };
            IList<OrderedFood> defaultOrderedFoods = new List<OrderedFood>
            {
                new OrderedFood
                {
                    Id = 1,
                    OrderId = 1,
                    FoodId = 1,
                },
                new OrderedFood
                {
                    Id = 2,
                    OrderId = 1,
                    FoodId = 2,
                },
            };

            foreach (var i in defaultFoods)
                context.Foods.Add(i);
            foreach (var i in defaultCategories)
                context.Categories.Add(i);
            foreach (var i in defaultOrders)
                context.Orders.Add(i);
            foreach (var i in defaultOrderedFoods)
                context.OrderedFoods.Add(i);

            context.SaveChanges();
        }
    }
}
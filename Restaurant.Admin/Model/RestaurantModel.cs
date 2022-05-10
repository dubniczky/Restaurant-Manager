using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Admin.Persistence;
using Restaurant.Core.Data;

using System.Windows;

namespace Restaurant.Admin.Model
{
    public class RestaurantModel
    {
        public List<Category> Categories;
        public List<Food> Foods;
        public List<Order> Orders;
        public List<OrderedFood> OrderedFoods;
        public bool IsLoggedIn { get; set; }

        private RestaurantPersistence persistence;

        public RestaurantModel(RestaurantPersistence persistence)
        {
            if (persistence == null)
            {
                throw new ArgumentNullException(nameof(persistence));
            }

            IsLoggedIn = false;
            this.persistence = persistence;
        }

        //Connection
        public async Task<bool> LoginAsync(string username, string password)
        {
            IsLoggedIn = await persistence.LoginAsync(username, password);
            return IsLoggedIn;
        }
        public async Task<bool> LogoutAsync()
        {
            if (!IsLoggedIn)
            {
                return true;
            }
            IsLoggedIn = !(await persistence.LogoutAsync());

            return IsLoggedIn;
        }

        //Load
        public async Task LoadAsync()
        {
            Foods = (await persistence.GetFoodsAsync()).ToList();
            Orders = (await persistence.GetOrdersAsync()).ToList();
            Categories = (await persistence.GetCategoriesAsync()).ToList();
            OrderedFoods = (await persistence.GetOrderedFoodsAsync()).ToList();
        }

        //Food
        public async Task<bool> AddFoodAsync(Food f)
        {
            if (f == null)
            {
                MessageBox.Show("Item cannot be null");
                return false;
            }
            if (Foods.Contains(f))
            {
                MessageBox.Show("Item already in collection.");
                return false;
            }

            //Temp id
            f.Id = (Foods.Count > 0 ? Foods.Max(b => b.Id) : 0) + 1;
            Foods.Add(f);
            if (!await persistence.CreateFoodAsync(f))
            {
                MessageBox.Show("Could not save food.");
            }

            return true;
        }
        public async Task<bool> UpdateFoodAsync(Food f)
        {
            if (f == null)
            {
                throw new ArgumentNullException(nameof(f));
            }                

            Food foodToMod = Foods.FirstOrDefault(i => i.Id == f.Id);
            if (foodToMod == null)
            {
                throw new ArgumentException("The food does not exist", nameof(f));
            }

            foodToMod.Name = f.Name;
            foodToMod.OrderCount = f.OrderCount;
            foodToMod.Price = f.Price;
            foodToMod.Spicy = f.Spicy;
            foodToMod.Type = f.Type;
            foodToMod.Vegetarian = f.Vegetarian;
            foodToMod.Description = f.Description;

            if (!await persistence.UpdateFoodAsync(foodToMod))
            {
                MessageBox.Show("Could not save food.");
            }

            return true;
        }

        //Order
        public async Task<bool> UpdateOrderAsync(Order o)
        {
            if (o == null)
            {
                throw new ArgumentNullException(nameof(o));
            }

            Order orderToMod = Orders.FirstOrDefault(i => i.Id == o.Id);
            if (orderToMod == null)
            {
                throw new ArgumentException("The food does not exist", nameof(o));
            }

            orderToMod.Completed = true;

            if (!await persistence.UpdateOrderAsync(orderToMod))
            {
                MessageBox.Show("Could not save food.");
            }

            return true;
        }

        //Category
        public async Task<bool> AddCategoryAsync(Category c)
        {
            if (c == null)
            {
                MessageBox.Show("Item cannot be null");
                return false;
            }
            if (Categories.Contains(c))
            {
                MessageBox.Show("Item already in collection.");
                return false;
            }

            //Temp id
            c.Id = (Foods.Count > 0 ? Foods.Max(b => b.Id) : 0) + 1;
            Categories.Add(c);
            if (!await persistence.CreateCategoryAsync(c))
            {
                MessageBox.Show("Could not save category.");
            }

            return true;
        }
        public async Task<bool> UpdateCategoryAsync(Category c)
        {
            if (c == null)
            {
                throw new ArgumentNullException(nameof(c));
            }

            Category categoryToMod = Categories.FirstOrDefault(i => i.Id == c.Id);
            if (categoryToMod == null)
            {
                throw new ArgumentException("The category does not exist", nameof(c));
            }

            categoryToMod.Name = c.Name;
            categoryToMod.Link = c.Link;
            categoryToMod.TypeName = c.TypeName;
            categoryToMod.ComplexList = c.ComplexList;

            if (!await persistence.UpdateCategoryAsync(categoryToMod))
            {
                MessageBox.Show("Could not save category.");
            }

            return true;
        }
        public async Task<bool> DeleteCategoryAsync(Category c)
        {
            if (c == null)
            {
                MessageBox.Show("Item cannot be null");
                return false;
            }
            if (!Categories.Contains(c))
            {
                MessageBox.Show("Item not found in collection.");
                return false;
            }

            if (!await persistence.DeleteCategoryAsync(c))
            {
                MessageBox.Show("Could not delete category.");
            }
            return true;
        }
    }
}

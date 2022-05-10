using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

using Restaurant.Core.Data;
using Restaurant.Core.Routes;

namespace Restaurant.Admin.Persistence
{
    public class RestaurantPersistence
    {
        private HttpClient client;
        private string address;

        public RestaurantPersistence(string address)
        {
            this.address = address;

            client = new HttpClient
            {
                BaseAddress = new Uri(address),
                Timeout = new TimeSpan(0, 0, 5),
            };
        }

        //User
        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"api/login/{username}/{password}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                throw new Exception("Persistence unavaiable", e);
            }
        }
        public async Task<bool> LogoutAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"api/logout");
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                throw new Exception("Persistence unavaiable", e);
            }
        }

        //Food
        public async Task<IEnumerable<Food>> GetFoodsAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(ApiRoutes.GetFood);
                if (!response.IsSuccessStatusCode) throw new Exception("Persistence unabailable: " + response.StatusCode);

                return await response.Content.ReadAsAsync<IEnumerable<Food>>();
            }
            catch (Exception e)
            {
                throw new Exception("Persistence unabailable", e);
            }
        }
        public async Task<bool> CreateFoodAsync(Food food)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(ApiRoutes.CreateFood, food);
                food.Id = (await response.Content.ReadAsAsync<Food>()).Id;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Persistence unavailable!", ex);
            }
        }
        public async Task<bool> UpdateFoodAsync(Food food)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(ApiRoutes.UpdateFood, food);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Persistence unavailable!", ex);
            }
        }

        //Order
        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(ApiRoutes.GetOrder);
                if (!response.IsSuccessStatusCode) throw new Exception("Persistence unabailable: " + response.StatusCode);

                return await response.Content.ReadAsAsync<IEnumerable<Order>>();
            }
            catch (Exception e)
            {
                throw new Exception("Persistence unabailable", e);
            }
        }
        public async Task<bool> UpdateOrderAsync(Order order)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(ApiRoutes.UpdateOrder, order);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Persistence unavailable!", ex);
            }
        }
        public async Task<IEnumerable<OrderedFood>> GetOrderedFoodsAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(ApiRoutes.GetOrderedFoods);
                if (!response.IsSuccessStatusCode) throw new Exception("Persistence unabailable: " + response.StatusCode);

                return await response.Content.ReadAsAsync<IEnumerable<OrderedFood>>();
            }
            catch (Exception e)
            {
                throw new Exception("Persistence unabailable", e);
            }
        }

        //Category
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(ApiRoutes.GetCategory);
                if (!response.IsSuccessStatusCode) throw new Exception("Persistence unabailable: " + response.StatusCode);

                return await response.Content.ReadAsAsync<IEnumerable<Category>>();
            }
            catch (Exception e)
            {
                throw new Exception("Persistence unabailable", e);
            }
        }
        public async Task<bool> CreateCategoryAsync(Category category)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(ApiRoutes.CreateCategory, category);
                category.Id = (await response.Content.ReadAsAsync<Category>()).Id;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Persistence unavailable!", ex);
            }
        }
        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(ApiRoutes.UpdateCategory, category);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Persistence unavailable!", ex);
            }
        }
        public async Task<bool> DeleteCategoryAsync(Category category)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync(ApiRoutes.DeleteCategory + $"/{category.Id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                throw new Exception("Persistence unabailable", e);
            }
        }
    }
}

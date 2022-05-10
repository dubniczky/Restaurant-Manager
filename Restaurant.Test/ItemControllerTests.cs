using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

using Restaurant.Core.Routes;
using Restaurant.Core.Data;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Internal;
using System.Net.Http;
using System.Linq;

namespace Restaurant.Test
{
    public class ItemsControllersTest : IClassFixture<ServerClientFixture>
    {
        private readonly ServerClientFixture fixture;

        public ItemsControllersTest(ServerClientFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async void Test_GetFoods_ReturnsNotEmpty()
        {
            // Act
            var response = await fixture.Client.GetAsync(ApiRoutes.GetFood);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<IEnumerable<Food>>(responseString);

            Assert.NotNull(responseObject);
            Assert.True(responseObject.Any());
        }

        [Fact]
        public async void Test_PostFood_ShouldAddItem()
        {
            // Arrange
            Food item = new Food()
            {
                Id = 11,
                Name = "Kolbász",
                Description = "kb",
                Price = 9,
                Spicy = true,
                Vegetarian = false,
                Type = "soup",
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync(ApiRoutes.CreateFood, content);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.NotNull(fixture.Context.Foods.FirstOrDefault(i => i.Name == "xy"));
        }

        [Fact]
        public async void Test_PostFood_ShouldUpdateItem()
        {
            // Arrange
            Food f = fixture.Context.Foods.FirstOrDefault();

            f.Price = 59999;

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(f), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync(ApiRoutes.UpdateFood, content);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.NotNull(fixture.Context.Foods.FirstOrDefault(i => i.Name == "kakukk"));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void Test_DeleteCategory_SouldDeleteCategory(int id)
        {
            // Arrange
            var category = fixture.Context.Categories.FirstOrDefault(i => i.Id == id);
            Assert.NotNull(category);

            // Act
            var response = await fixture.Client.GetAsync(ApiRoutes.DeleteCategory + "/" + id);

            // Assert
            Assert.Null(fixture.Context.Categories.FirstOrDefault(i => i.Id == id));
        }
    }
}

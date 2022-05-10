using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using Restaurant.Server.Models;
using Restaurant.Core.Language;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Linq;

using Restaurant.Core.Data;
using Restaurant.Core.Routes;

namespace Restaurant.Server.Controllers
{
    [Controller] 
    public class ApiController : Controller
    {
        protected RestaurantContext context;
        protected UserManager<AdminUser> userManager;
        protected SignInManager<AdminUser> signInManager;

        public ApiController(RestaurantContext db, UserManager<AdminUser> userManager, SignInManager<AdminUser> signInManager)
        {
            this.context = db;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet(ApiRoutes.Login + "/{username}/{password}")]
        public async Task<IActionResult> LoginAsync(string username, string password)
        {
            //Valid username
            if (username == null || string.IsNullOrWhiteSpace(username))
            {
                return Forbid();
            }

            try
            {
                var res = await signInManager.PasswordSignInAsync(username, password, false, false);
                if (res.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return Forbid();
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet(ApiRoutes.Logout)]
        [Authorize]
        public async Task<IActionResult> LogoutAsync()
        {
            try
            {
                await signInManager.SignOutAsync();
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //Foods

        [HttpGet(ApiRoutes.GetFood)]
        [Authorize]
        public IActionResult GetFoods()
        {
            try
            {
                return Ok(context.Foods.ToList());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost(ApiRoutes.CreateFood)]
        [Authorize]
        public IActionResult AddFood([FromBody]Food food)
        {
            try
            {
                food.Id = 0;
                var added = context.Foods.Add(food);
                context.SaveChanges();

                return Created(added.Entity.Id.ToString(), food);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut(ApiRoutes.UpdateFood)]
        [Authorize]
        public IActionResult UpdateFood([FromBody]Food food)
        {
            try
            {
                Food f = context.Foods.FirstOrDefault(i => i.Id == food.Id);

                if (f == null) return NotFound();

                f.Name = food.Name;
                f.OrderCount = food.OrderCount;
                f.Price = food.Price;
                f.Description = food.Description;
                f.Spicy = food.Spicy;
                f.Vegetarian = food.Vegetarian;

                context.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //Orders

        [HttpGet(ApiRoutes.GetOrder)]
        [Authorize]
        public IActionResult GetOrder()
        {
            try
            {
                return Ok(context.Orders.ToList());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut(ApiRoutes.UpdateOrder)]
        [Authorize]
        public IActionResult UpdateOrder([FromBody]Order order)
        {
            try
            {
                Order o = context.Orders.FirstOrDefault(i => i.Id == order.Id);

                if (o == null) return NotFound();

                o.Completed = true;
                o.CompleteTime = DateTime.Now;

                context.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet(ApiRoutes.GetOrderedFoods)]
        [Authorize]
        public IActionResult GetOrderedFoods()
        {
            try
            {
                return Ok(context.OrderedFoods.ToList());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //Categories

        [HttpGet(ApiRoutes.GetCategory)]
        [Authorize]
        public IActionResult GetCategories()
        {
            try
            {
                return Ok(context.Categories.ToList());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost(ApiRoutes.CreateCategory)]
        [Authorize]
        public IActionResult AddCategory([FromBody]Category category)
        {
            try
            {
                category.Id = 0;
                var added = context.Categories.Add(category);
                context.SaveChanges();

                return Created(added.Entity.Id.ToString(), category);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut(ApiRoutes.UpdateCategory)]
        [Authorize]
        public IActionResult UpdateCategory([FromBody]Category category)
        {
            try
            {
                Category c = context.Categories.FirstOrDefault(i => i.Id == category.Id);

                if (c == null) return NotFound();

                c.Name = category.Name;
                c.Link = category.Link;
                c.TypeName = category.TypeName;
                c.ComplexList = category.ComplexList;

                context.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete(ApiRoutes.DeleteCategory + "/{id}")]
        [Authorize]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                Category c = context.Categories.FirstOrDefault(i => i.Id == id);

                if (c == null) return NotFound();

                context.Categories.Remove(c);

                context.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

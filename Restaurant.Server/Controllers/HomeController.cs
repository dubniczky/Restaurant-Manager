using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

using Restaurant.Core.Data;
using Restaurant.Server.Models;

namespace Restaurant.Server.Controllers
{
    public class HomeController : Controller
    {
        private readonly RestaurantContext db;

        public HomeController(RestaurantContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            IndexViewModel im = new IndexViewModel();

            im.Categories = db.Categories.ToList();
            var a = db.OrderedFoods.OrderByDescending((_) => _.FoodId).
                                    GroupBy((_) => _.FoodId).                                    
                                    Take(10).ToArray();

            im.TopFoods = new List<Food>();
            foreach(var e in a)
            {
                im.TopFoods.Add(db.Foods.Find(e.Key));                
            }

            return View("Index", im);
        }

        [HttpGet]
        [Route("/category/{category}")]
        public IActionResult Category(string category)
        {
            ListViewModel fm = new ListViewModel();

            //Get category
            IList<Category> cat = db.Categories.Where(a => a.Link == category).Take(1).ToList();
            if (cat == null || cat.Count == 0)
            {
                //Category does not exist
                return Index();
            }
            fm.Category = cat[0];

            //Get foods
            fm.Foods = db.Foods.Where(a => a.Type == fm.Category.TypeName).ToList();

            return View("List", fm);
        }

        [HttpGet]
        [Route("/cart")]
        public IActionResult Cart()
        {
            CartViewModel cm = new CartViewModel();

            //Deserialize session
            string session = HttpContext.Session.GetString("session");
            SessionData sd;
            if (session == null)
            {
                //New session
                sd = new SessionData();
                sd.Cart = new List<Food>();
            }
            else
            {
                //Load session
                sd = (SessionData)JsonConvert.DeserializeObject(session, typeof(SessionData));
            }

            cm.Foods = sd.Cart;

            //Calculate price
            int price = 0;
            foreach (var f in cm.Foods)
            {
                price += f.Price;
            }
            cm.Price = price;

            return View("Cart", cm);
        }

        [HttpGet]
        [Route("/cart/add/{category}/{id}")]
        public IActionResult CartAdd(int id, string category)
        {
            //if (id == null) return Category(category);
            //int ids = id.GetValueOrDefault();

            //Check if item exists
            var food = db.Foods.Find(id);
            if (food == null)
            {
                ViewData["Message"] = "A termék már nem elérhető!";
                return Category(category);
            }

            //Deserialize session
            string session = HttpContext.Session.GetString("session");
            SessionData sd;
            if (session == null)
            {
                //New session
                sd = new SessionData();
                sd.Cart = new List<Food>();
            }
            else
            {
                //Load session
                sd = (SessionData)JsonConvert.DeserializeObject(session, typeof(SessionData));
            }

            //Update session
            int price = 0;
            foreach (var i in sd.Cart)
            {
                price += i.Price;
            }
            if (price + food.Price > 20000)
            {
                ViewData["Message"] = "A kosár tartalma nem haladhatja meg a 20 000 forintot!";
                return Category(category);
            }
            sd.Cart.Add(food);

            //Commit changes
            HttpContext.Session.SetString("session", JsonConvert.SerializeObject(sd));
            ViewData["Message"] = "Kosárhoz adva!";
            return Category(category);
        }

        [HttpGet]
        [Route("/cart/remove/{id}")]
        public IActionResult CartRemove(int id)
        {
            //Deserialize session
            string session = HttpContext.Session.GetString("session");
            SessionData sd;
            if (session == null)
            {
                //New session
                sd = new SessionData();
                sd.Cart = new List<Food>();
            }
            else
            {
                //Load session
                sd = (SessionData)JsonConvert.DeserializeObject(session, typeof(SessionData));
            }

            //Remove item
            for (int i = 0; i < sd.Cart.Count; i++)
            {
                if (sd.Cart[i].Id == id)
                {
                    sd.Cart.RemoveAt(i);
                    break;
                }
            }

            //Serialize
            HttpContext.Session.SetString("session", JsonConvert.SerializeObject(sd));
            ViewData["Message"] = "Törölve a kosárból!";
            return Redirect("/cart");
        }

        [HttpPost]
        [Route("/cart/submit")]
        [ValidateAntiForgeryToken]
        public IActionResult CartSubmit([Bind] CustomerModel customer)
        {
            //Validate model
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Minden adatot ki kell tölteni!";
                return Cart();
            }

            //Deserialize session
            string session = HttpContext.Session.GetString("session");
            SessionData sd;
            if (session == null)
            {
                //New session
                sd = new SessionData();
                sd.Cart = new List<Food>();
            }
            else
            {
                //Load session
                sd = (SessionData)JsonConvert.DeserializeObject(session, typeof(SessionData));
            }

            //Validate cart length
            if (sd.Cart.Count == 0)
            {
                ViewData["Message"] = "Legalább egy terméknek lennie kell a kosárban!";
                return Cart();
            }

            //Calculate price and make arg list
            int price = 0;
            foreach (var f in sd.Cart)
            {
                price += f.Price;
            }

            Order order = new Order
            {
                Price = price,
                Date = DateTime.Now,
                Name = customer.Name,
                Phone = customer.Phone,
                Address = customer.Address
            };

            //Add items
            db.Orders.Add(order);
            db.SaveChanges();

            OrderedFood of;
            foreach (var f in sd.Cart)
            {
                of = new OrderedFood(order.Id, f.Id);
                db.OrderedFoods.Add(of);
            }

            //Save and confirm
            db.SaveChanges();
            return View("Confirm");
        }

        [HttpGet]
        [Route("/error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

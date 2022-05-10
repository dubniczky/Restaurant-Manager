using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Core.Routes
{
    public static class ApiRoutes
    {
        //User
        public const string Login = "api/login";
        public const string Logout = "api/logout";

        //Get
        public const string GetFood = "api/foods";
        public const string GetOrder = "api/orders";
        public const string GetCategory = "api/categories";
        public const string GetOrderedFoods = "api/orderedfoods";

        //Create
        public const string CreateFood = "api/foods/create";
        public const string CreateCategory = "api/categories/create";

        //Update
        public const string UpdateFood = "api/foods/update";
        public const string UpdateOrder = "api/orders/update";
        public const string UpdateCategory = "api/categories/update";

        //Delete
        public const string DeleteCategory = "api/categories/delete";

    }
}

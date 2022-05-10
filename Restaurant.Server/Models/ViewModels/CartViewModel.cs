using System.Collections.Generic;
using Restaurant.Core.Data;

namespace Restaurant.Server.Models
{
    public struct CartViewModel
    {
        public IList<Food> Foods { get; set; }
        public int Price { get; set; }
        public CustomerModel Customer { get; set; }
    }
}

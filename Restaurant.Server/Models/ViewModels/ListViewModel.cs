using System.Collections.Generic;
using Restaurant.Core.Data;

namespace Restaurant.Server.Models
{
    public struct ListViewModel
    {
        public Category Category { get; set; }
        public IList<Food> Foods { get; set; }
    }
}

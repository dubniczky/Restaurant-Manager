using System.Collections.Generic;
using Restaurant.Core.Data;

namespace Restaurant.Server.Models
{
    public struct IndexViewModel
    {
        public IList<Category> Categories { get; set; }
        public IList<Food> TopFoods { get; set; }

        public IndexViewModel(IList<Category> Categories, IList<Food> TopFoods)
        {
            this.Categories = Categories;
            this.TopFoods = TopFoods;
        }
    }
}

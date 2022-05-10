namespace Restaurant.Core.Data
{
    public class Food
    {
        public int Id { get; set; }
        public int OrderCount { get; set; }
        public int Price { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }

        public string Description { get; set; }
        public bool Spicy { get; set; }
        public bool Vegetarian { get; set; }
    }
}

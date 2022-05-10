using System;

namespace Restaurant.Core.Data
{
    public class Order
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public bool Completed { get; set; }
        public DateTime Date { get; set; }
        public DateTime? CompleteTime { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}

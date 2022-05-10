namespace Restaurant.Core.Data
{
    public class OrderedFood
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int FoodId { get; set; }

        public OrderedFood()
        {
           
        }
        public OrderedFood(int orderId, int foodId)
        {
            OrderId = orderId;
            FoodId = foodId;
        }
    }
}

namespace OnlineStore.DTOs
{
    public class FoodDTO
    {
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }
    }
}

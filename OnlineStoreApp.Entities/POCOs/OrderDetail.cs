namespace OnlineStoreApp.Entities.POCOs
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public Guid OrderId { get; set; }
        public int FoodId { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }

        public Order Order { get; set; }
        public Food Food { get; set; }
    }
}
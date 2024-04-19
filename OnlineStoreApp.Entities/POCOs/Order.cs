namespace OnlineStoreApp.Entities.POCOs
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public User User { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    }
}

namespace OnlineStore.DTOs
{
    public class OrderRequestDTO
    {
        public OrderDTO Order { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; }
    }
}

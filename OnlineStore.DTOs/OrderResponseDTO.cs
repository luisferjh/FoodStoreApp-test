namespace OnlineStore.DTOs
{
    public class OrderResponseDTO
    {
        public OrderDto Orden { get; set; }
        public List<OrderDetailDto> orderDetailDtos { get; set; } = new List<OrderDetailDto>();
    }

    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
    }

    public class OrderDetailDto
    {
        public int Id { get; set; }
        public int FoodId { get; set; }
        public string Food { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }

    }
}

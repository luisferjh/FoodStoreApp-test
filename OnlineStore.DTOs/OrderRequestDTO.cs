using System.ComponentModel.DataAnnotations;

namespace OnlineStore.DTOs
{
    public class OrderRequestDTO
    {
        [Required]
        public OrderDTO Order { get; set; }
        [Required]
        public List<OrderDetailDTO> OrderDetails { get; set; }
    }
}

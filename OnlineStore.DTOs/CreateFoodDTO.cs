using System.ComponentModel.DataAnnotations;

namespace OnlineStore.DTOs
{
    public class CreateFoodDTO
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Must be 100 characters")]
        public string Name { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Must be 200 characters")]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int QuantityAvailable { get; set; }
    }
}

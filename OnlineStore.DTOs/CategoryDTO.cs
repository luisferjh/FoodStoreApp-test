using System.ComponentModel.DataAnnotations;

namespace OnlineStore.DTOs
{
    public class CategoryDTO
    {
        [Required]
        public string Name { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace OnlineStore.DTOs
{
    public class UserCreateDTO
    {
        [Required]
        [StringLength(50, ErrorMessage = "Must be between 10 and 50 characters", MinimumLength = 10)]
        [DataType(DataType.Password)]
        public string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Must be between 10 and 50 characters")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}

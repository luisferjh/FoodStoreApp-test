using System.ComponentModel.DataAnnotations;

namespace OnlineStore.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [StringLength(30, ErrorMessage = "Password must not have more than 30 characters")]
        public string Password { get; set; }
    }
}

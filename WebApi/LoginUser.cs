using System.ComponentModel.DataAnnotations;

namespace WebApi
{
    public class LoginUser
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

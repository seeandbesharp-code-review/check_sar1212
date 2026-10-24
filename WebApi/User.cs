using System.ComponentModel.DataAnnotations;

namespace WebApi
{
    public class User
    {

        public int UserID{ get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]

        public string LastName { get; set; }
    }
}

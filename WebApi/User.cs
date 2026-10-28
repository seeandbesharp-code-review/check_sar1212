using System.ComponentModel.DataAnnotations;

namespace WebApi
{
    //In C#, "Id" is written with a small d: UserId instead of UserID.
    //If you rename it, users.txt still has the old name, so read it with
    //new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
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

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Zxcvbn;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        const string filePath = "./users.txt";

        // GET: api/<UsersController>
        [HttpGet]
    

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            using (StreamReader reader = System.IO.File.OpenText(filePath))
            {
                string? currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    User user =
                    JsonSerializer.Deserialize<User>(currentUserInFile);
                    if (user.UserID == id)
                        return Ok(user);
                }
            }
            return NotFound();
        }

        // POST api/<UsersController>
        [HttpPost("login")]
        public ActionResult<User> PostLogin([FromBody] LoginUser loginUser)
        {
            using (StreamReader reader = System.IO.File.OpenText(filePath))
            {
                string? currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    User user = JsonSerializer.Deserialize<User>(currentUserInFile);
                    if (user.Email == loginUser.Email && user.Password == loginUser.Password)
                        return Ok(user);
                }
            }
            return NotFound();

        }

        [HttpPost("signin")]
        public ActionResult<User> PostSignIn([FromBody] User user)
        {
            if (Zxcvbn.Core.EvaluatePassword(user.Password).Score < 3)
                return BadRequest("the password is weak!");
            int numberOfUsers = System.IO.File.ReadLines(filePath).Count();
            user.UserID = numberOfUsers + 1;
            string userJson = JsonSerializer.Serialize(user);
            System.IO.File.AppendAllText(filePath, userJson + Environment.NewLine);
            return CreatedAtAction(nameof(Get), new { id = user.UserID }, user);
        }




        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(User userToUpdate)
        {
            string textToReplace = string.Empty;
            if (Zxcvbn.Core.EvaluatePassword(userToUpdate.Password).Score < 3)
                return BadRequest("weak password");
            using (StreamReader reader = System.IO.File.OpenText(filePath))
            {
                string currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    User user = JsonSerializer.Deserialize<User>(currentUserInFile);
                    if (user.UserID == userToUpdate.UserID)
                        textToReplace = currentUserInFile;
                }
            }
            if (textToReplace != string.Empty)
            {
                string text = System.IO.File.ReadAllText(filePath);
                text = text.Replace(textToReplace, JsonSerializer.Serialize(userToUpdate));
                System.IO.File.WriteAllText(filePath, text);
            }
            return Ok();
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost("CheckPassword")]
        public int PasswordCheck([FromBody] string password)
        {
            return Zxcvbn.Core.EvaluatePassword(password).Score;
        }

    }
}

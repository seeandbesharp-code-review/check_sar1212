//fix: Removed "using Microsoft.AspNetCore.Http.HttpResults;" - it was added by mistake and is not used
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
        //fix: Removed an extra [HttpGet] that was left here, so it was attached to Get(int id) below as well
    

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
        //The assignment asks a wrong login to return NoContent() (204)
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
        //"Sign in" usually means logging in. This action creates a new user, so a name like
        //register or signup (and PostRegister / PostSignUp) would be clearer. What do you think?
        public ActionResult<User> PostSignIn([FromBody] User user)
        {
            if (Zxcvbn.Core.EvaluatePassword(user.Password).Score < 3)
                return BadRequest("the password is weak!");
            //users.txt has two users with UserID 9 (and none with 8). Counting the lines gives a repeated id
            //if a line was removed. How can you make sure every new user gets an id that isn't used yet?
            int numberOfUsers = System.IO.File.ReadLines(filePath).Count();
            user.UserID = numberOfUsers + 1;
            string userJson = JsonSerializer.Serialize(user);
            System.IO.File.AppendAllText(filePath, userJson + Environment.NewLine);
            return CreatedAtAction(nameof(Get), new { id = user.UserID }, user);
        }




        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        //fix: Take the id from the route (api/users/5), so the right user is updated even if the body has no UserID
        public IActionResult Put(int id, User userToUpdate)
        {
            //fix: Keep the id from the route
            userToUpdate.UserID = id;
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
                    {
                        textToReplace = currentUserInFile;
                        //fix: Found- break
                        break;
                    }
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

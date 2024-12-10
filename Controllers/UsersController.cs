using NEWPROJECT.Services;
using NEWPROJECT.Interfaces;
using NEWPROJECT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace NEWPROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _usersService;
        private TokenService _tokenService;
        private ILogger<UsersController>? logger;

        public UsersController(IUserService usersService, TokenService tokenService)
        {
            _usersService = usersService;
            _tokenService = tokenService;

        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public ActionResult<List<User>> Get()
        {
            return _usersService.GetAll();
        }



        [HttpGet("{id}")]
        [Authorize(Policy = "Admin")]
        public ActionResult<User> Get(int id)
        {
            if (int.Parse(User.FindFirst("id")?.Value!) != id && User.FindFirst("type")?.Value != "Admin")
                return Unauthorized();

            var user = _usersService.GetById(id);
            if (user == null)
                return NotFound();
            return user;
        }




        [HttpPost]
        [Authorize(Policy = "Admin")]
        public ActionResult Post(User newUser)
        {
            _usersService.Add(newUser);
            return NoContent();
        }




        [HttpPost("login")]
        public ActionResult<objectToReturn> Login([FromBody] User user)
        {
            try
            {

                Console.WriteLine($"Trying to login user {user.Name} with ID {user.Id}");

                int userExistID = _usersService.ExistUser(user.Name, user.Id);

                if (userExistID == -1)
                {
                    Console.WriteLine("המשתמש לא נמצא");
                    return Unauthorized();  // אם המשתמש לא נמצא
                }

                var claims = new List<Claim> { };
                if (user.Id == 123) // דוגמה למנהל
                    claims.Add(new Claim("type", "Admin"));
                else
                    claims.Add(new Claim("type", "User"));

                claims.Add(new Claim("id", userExistID.ToString()));

                var token = TokenService.GenerateToken(claims);

                return new OkObjectResult(new { Id = userExistID, token });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public ActionResult Put(int id, User newUser)
        {
            var result = _usersService.Update(id, newUser);
            if (!result)
            {
                return BadRequest();
            }
            return NoContent();
        }




        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id)
        {
            bool result = _usersService.Delete(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }


    public class objectToReturn
    {
        public int Id { get; set; }
        public string token { get; set; }
    }
}

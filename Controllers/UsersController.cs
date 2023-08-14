using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserDal _userDal;

        public UsersController(UserDal userDal)
        {
            _userDal = userDal;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] Users user)
        {
            if (user == null)
            {
                return BadRequest("Informações do usuário inválidas.");
            }

 

            int userId = _userDal.RegisterUser(user);

            if (userId > 0)
            {
                
                return CreatedAtAction(nameof(Register), new { id = userId }, user);
            }
            else
            {
                return StatusCode(500, "Um erro ocorreu enquanto registrava o usuário.");
            }
        }



        [HttpPost("login")]
        public IActionResult Login([FromBody] Users userCredentials)
        {
            if (userCredentials == null || string.IsNullOrEmpty(userCredentials.Email) || string.IsNullOrEmpty(userCredentials.HashedPassword))
            {
                return BadRequest("Credenciais inválidas.");
            }

            var userId = _userDal.CheckUserCredentials(userCredentials.Email, userCredentials.HashedPassword);

            if (userId.HasValue && userId.Value > 0)
            {
                return Ok(new { id = userId.Value });
            }

            return Unauthorized();  
        }


        [HttpGet("profile/{userId}")]
        public IActionResult GetProfile(int userId)
        {
            var userProfile = _userDal.GetUserProfile(userId);

            if (userProfile != null)
            {
                return Ok(userProfile);
            }

            return NotFound();  
        }

        [HttpPut("profile")]
        public IActionResult UpdateProfile([FromBody] Users user)
        {
            if (user == null)
            {
                return BadRequest("Informações do usuário inválidas.");
            }

            bool isUpdated = _userDal.UpdateUserProfile(user);

            if (isUpdated)
            {
                return Ok("Perfil atualizado com sucesso.");
            }
            else
            {
                return StatusCode(500, "Um erro ocorreu enquanto atualizava o perfil.");
            }
        }


    }
}

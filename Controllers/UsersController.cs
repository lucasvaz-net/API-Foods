using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Models;
using API.Data.API.Data;
using API.Filters;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserDal _userDal;
        private readonly UserTokenDal _userTokenDal;

        public UsersController(UserDal userDal, UserTokenDal userTokenDal)
        {
            _userDal = userDal;
            _userTokenDal = userTokenDal;
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
                // Recuperar a API key do cabeçalho
                string apiKey = HttpContext.Request.Headers["ApiKey"].FirstOrDefault();
                if (string.IsNullOrEmpty(apiKey))
                {
                    return BadRequest("API key ausente.");
                }

                string generatedToken = _userTokenDal.CreateToken(userId.Value, apiKey);
                return Ok(new { id = userId.Value, token = generatedToken });
            }

            return Unauthorized();
        }


        [TokenAuthorize]
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

        [TokenAuthorize]
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




        [TokenAuthorize]
        [HttpPost("logout")]
        public IActionResult Logout(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("UserId inválido.");
            }

            _userTokenDal.LogoutUser(userId);

            return Ok("Logout realizado com sucesso.");
        }





    }
}

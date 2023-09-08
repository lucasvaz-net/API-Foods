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


            user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(user.HashedPassword);

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
            if (userCredentials == null || string.IsNullOrEmpty(userCredentials.Email))
            {
                return BadRequest("Credenciais inválidas.");
            }

            var storedHashedPassword = _userDal.GetHashedPasswordByEmail(userCredentials.Email);

            if (string.IsNullOrEmpty(storedHashedPassword) || !BCrypt.Net.BCrypt.Verify(userCredentials.HashedPassword, storedHashedPassword))
            {
                return Unauthorized(new { message = "Credenciais inválidas." });

            }

            // Recuperar a API key do cabeçalho
            string apiKey = HttpContext.Request.Headers["ApiKey"].FirstOrDefault();

            var userId = _userDal.CheckUserCredentials(userCredentials.Email, storedHashedPassword);

            if (userId.HasValue && userId.Value > 0)
            {
                string generatedToken = _userTokenDal.CreateToken(userId.Value, apiKey);
                return Ok(new { id = userId.Value, token = generatedToken });
            }

            return Unauthorized(new { message = "Credenciais inválidas." });

        }



        [TokenAuthorize]
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {

            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Token inválido. Por favor, faça login novamente ou crie um usuário, para maiores informações consulte nossa documentação http://apifood-001-site1.atempurl.com/StaticFiles/docs.html." });
            }

            int? userId = _userDal.GetUserIdByToken(token);

            if (!userId.HasValue || userId.Value <= 0)
            {
                return Unauthorized(new { message = "Token inválido. Por favor, faça login novamente ou crie um usuário, para maiores informações consulte nossa documentação http://apifood-001-site1.atempurl.com/StaticFiles/docs.html." });
            }

            var userProfile = _userDal.GetUserProfile(userId.Value);

            if (userProfile != null)
            {
                return Ok(userProfile);
            }

            return NotFound(new { message = "Ocorreu um erro, Consulte a Administração do sistema ou verifique nossa documentação disponivel em  http://apifood-001-site1.atempurl.com/StaticFiles/docs.html." });
           
        }


        [TokenAuthorize]
        [HttpPut("profile")]
        public IActionResult UpdateProfile([FromBody] Users user)
        {
            if (user == null)
            {
                return BadRequest("Informações do usuário inválidas.");
            }

            // Recuperar o token do cabeçalho
            string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').Last();

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Token inválido. Por favor, faça login novamente ou crie um usuário, para maiores informações consulte nossa documentação http://apifood-001-site1.atempurl.com/StaticFiles/docs.html." });
            }

          
            var userId = _userDal.GetUserIdByToken(token);
            if (!userId.HasValue)
            {
                return Unauthorized(new { message = "Token inválido. Por favor, faça login novamente ou crie um usuário, para maiores informações consulte nossa documentação http://apifood-001-site1.atempurl.com/StaticFiles/docs.html." });
            }

            user.Id = userId.Value; 

           
            if (!string.IsNullOrEmpty(user.HashedPassword))
            {
               
                user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(user.HashedPassword);
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
        public IActionResult Logout()
        {
           
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Token inválido. Por favor, faça login novamente ou crie um usuário, para maiores informações consulte nossa documentação http://apifood-001-site1.atempurl.com/StaticFiles/docs.html." });
            }

            int? userId = _userDal.GetUserIdByToken(token);

            if (!userId.HasValue || userId.Value <= 0)
            {
                return Unauthorized(new { message = "Token inválido. Por favor, faça login novamente ou crie um usuário, para maiores informações consulte nossa documentação http://apifood-001-site1.atempurl.com/StaticFiles/docs.html." });
            }

            _userTokenDal.LogoutUser(userId.Value);

            return Ok("Logout realizado com sucesso.");
        }






    }
}

using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Models;
using API.Filters;

namespace API.Controllers
{
    [TokenAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FoodDiaryController : ControllerBase
    {
        private readonly FoodDiaryDal _foodDiaryDal;
        private readonly UserDal _userDal; // Injeção de dependência do UserDal para acesso ao método GetUserIdFromToken

        public FoodDiaryController(FoodDiaryDal foodDiaryDal, UserDal userDal)
        {
            _foodDiaryDal = foodDiaryDal;
            _userDal = userDal; // Inicialização
        }

        private int? GetUserIdFromToken()
        {
            string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').Last();

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            return _userDal.GetUserIdByToken(token);
        }

        [HttpGet]
        public IActionResult GetFoodDiaryEntries()
        {
            var userId = GetUserIdFromToken();

            if (!userId.HasValue)
            {
                return Unauthorized();
            }

            List<FoodDiaryEntry> entries = _foodDiaryDal.GetUserFoodDiary(userId.Value);

            if (entries == null || entries.Count == 0)
            {
                return NotFound($"Nenhuma entrada encontrada para o usuário com ID {userId.Value}.");
            }

            return Ok(entries);
        }

        [HttpPost]
        public IActionResult AddFoodToDiary([FromBody] FoodDiary foodDiaryEntry)
        {
            var userId = GetUserIdFromToken();

            if (!userId.HasValue)
            {
                return Unauthorized();
            }

            if (foodDiaryEntry == null || foodDiaryEntry.Foods.Id <= 0)
            {
                return BadRequest("Informações inválidas.");
            }

            bool isSuccess = _foodDiaryDal.AddFoodToDiary(userId.Value, foodDiaryEntry.Foods.Id, (float)foodDiaryEntry.ServingSize, foodDiaryEntry.Date);
            if (isSuccess)
            {
                return Ok("Entrada adicionada com sucesso!");
            }
            return StatusCode(500, "Um erro ocorreu enquanto adicionava a entrada.");
        }

        [HttpDelete("{entryId}")]
        public IActionResult RemoveFoodFromDiary(int entryId)
        {
            if (entryId <= 0)
            {
                return BadRequest("ID da entrada inválido.");
            }

            bool isSuccess = _foodDiaryDal.RemoveFoodFromDiary(entryId);
            if (isSuccess)
            {
                return Ok("Entrada removida com sucesso!");
            }
            return NotFound("A entrada não foi encontrada ou já foi removida.");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Models;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodDiaryController : ControllerBase
    {
        private readonly FoodDiaryDal _foodDiaryDal;

        public FoodDiaryController(FoodDiaryDal foodDiaryDal)
        {
            _foodDiaryDal = foodDiaryDal;
        }

        [HttpGet]
        public IActionResult GetFoodDiaryEntries(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("UserId inválido.");
            }

            List<FoodDiaryEntry> entries = _foodDiaryDal.GetUserFoodDiary(userId);

            if (entries == null || entries.Count == 0)
            {
                return NotFound($"Nenhuma entrada encontrada para o usuário com ID {userId}.");
            }

            return Ok(entries);
        }


        [HttpPost]
        public IActionResult AddFoodToDiary([FromBody] FoodDiary foodDiaryEntry)
        {
            if (foodDiaryEntry == null || foodDiaryEntry.UserId <= 0 || foodDiaryEntry.Foods.Id <= 0)
            {
                return BadRequest("Informações inválidas.");
            }

            bool isSuccess = _foodDiaryDal.AddFoodToDiary(foodDiaryEntry.UserId, foodDiaryEntry.Foods.Id, (float)foodDiaryEntry.ServingSize, foodDiaryEntry.Date);
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

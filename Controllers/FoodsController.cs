using API.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        private readonly FoodDal _foodDal;

        public FoodsController(FoodDal foodDal)
        {
            _foodDal = foodDal;
        }

        [HttpGet]
        public IActionResult GetAllFoods()
        {
            var foods = _foodDal.GetAllFoods();
            return Ok(foods);
        }

        [HttpGet("{foodId}")]
        public IActionResult GetFoodById(int foodId)
        {
            var food = _foodDal.GetFoodById(foodId);
            if (food != null)
                return Ok(food);
            else
                return NotFound();
        }
    }
}

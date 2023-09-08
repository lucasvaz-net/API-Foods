using API.Data;
using API.Filters;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [TokenAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PremiumController : ControllerBase
    {
        private readonly PremiumSubscriptionDal _premiumSubscriptionDal;
        private readonly UserDal _userDal;


        public PremiumController(PremiumSubscriptionDal premiumSubscriptionDal, UserDal userDal)
        {
            _premiumSubscriptionDal = premiumSubscriptionDal;
            _userDal = userDal;
        }
        [HttpGet]
        public IActionResult GetSubscriptionStatus()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];
            var userId = _userDal.GetUserIdByToken(token);

            if (userId <= 0)
            {
                return BadRequest("ID de usuário inválido.");
            }

            var subscription = _premiumSubscriptionDal.GetUserSubscriptionStatus((int)userId);

            if (subscription == null)
            {
                return NotFound("Status da assinatura não encontrado.");
            }

            return Ok(subscription);
        }

        [HttpPost("purchase")]
        public IActionResult PurchasePremiumSubscription()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];
            var userId = _userDal.GetUserIdByToken(token);

            if (userId <= 0)
            {
                return BadRequest("ID de usuário inválido.");
            }

            var subscription = new PremiumSubscription { UserId = (int)userId };

            try
            {
                _premiumSubscriptionDal.PurchasePremiumSubscription(subscription);
                return Ok("Assinatura premium comprada ou atualizada com sucesso!");
            }
            catch (Exception ex)
            {
                // Considere logar a exceção 'ex' para ajudar no debug.
                return StatusCode(500, "Um erro ocorreu enquanto processava a assinatura.");
            }
        }




    }
}

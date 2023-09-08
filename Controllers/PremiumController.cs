using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Models;
using API.Filters;

namespace API.Controllers
{
    [TokenAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PremiumController : ControllerBase
    {
        private readonly PremiumSubscriptionDal _premiumSubscriptionDal;

        public PremiumController(PremiumSubscriptionDal premiumSubscriptionDal)
        {
            _premiumSubscriptionDal = premiumSubscriptionDal;
        }

        [HttpGet("{userId}")]
        public IActionResult GetSubscriptionStatus(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("ID de usuário inválido.");
            }

            var subscription = _premiumSubscriptionDal.GetUserSubscriptionStatus(userId);

            if (subscription == null)
            {
                return NotFound("Status da assinatura não encontrado.");
            }

            return Ok(subscription);
        }


        [HttpPost("purchase")]
        public IActionResult PurchasePremiumSubscription([FromBody] PremiumSubscription subscription)
        {
            if (subscription == null || subscription.UserId <= 0)
            {
                return BadRequest("Informações inválidas.");
            }

            try
            {
                _premiumSubscriptionDal.PurchasePremiumSubscription(subscription);
                return Ok("Assinatura premium comprada ou atualizada com sucesso!");
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, "Um erro ocorreu enquanto processava a assinatura.");
            }
        }


    }
}

namespace API.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using API.Data;
    using API.Data.API.Data;
    using System.Net;

    public class TokenAuthorizeAttribute : ActionFilterAttribute
    {
        private UserTokenDal _tokenDal;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _tokenDal = context.HttpContext.RequestServices.GetService<UserTokenDal>();

            // Recupera o token do cabeçalho "Authorization"
            var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Se o token não estiver presente ou for inválido, configura o resultado como Unauthorized
            if (string.IsNullOrEmpty(token) || !_tokenDal.ValidateToken(token))
            {
                var responseObj = new
                {
                    status = 401,
                    message = "Token inválido. Por favor, faça login novamente ou crie um usuário, para maiores informações consulte nossa documentação http://apifood-001-site1.atempurl.com/StaticFiles/docs.html"
                };
                context.Result = new ObjectResult(responseObj)
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
                return; // Interrompe a execução aqui
            }

            base.OnActionExecuting(context);
        }
    }

}

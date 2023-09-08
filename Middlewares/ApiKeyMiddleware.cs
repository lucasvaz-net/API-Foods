using API.Data.API.Data;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ApiKeyMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
    {
        _next = next;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var apiKeyDal = scope.ServiceProvider.GetRequiredService<ApiKeyDal>();

            var apiKey = context.Request.Headers["ApiKey"].ToString();

            if (string.IsNullOrEmpty(apiKey) || !apiKeyDal.IsApiKeyValid(apiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key Invalida - A chave deve ser enviada no Header da requisição com nome ApiKey, para maiores informações consulte nossa documentação http://apifood-001-site1.atempurl.com/StaticFiles/docs.html");
                return;
            }
        }

        await _next(context);
    }
}

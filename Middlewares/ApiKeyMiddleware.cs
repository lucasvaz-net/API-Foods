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
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Invalid API Key");
                return;
            }
        }

        await _next(context);
    }
}

using API.Data;
using API.Data.API.Data;
using API.Filters;
using Microsoft.Extensions.FileProviders;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Configura��es
builder.Configuration.AddJsonFile("appsettings.json");

// Servi�os
builder.Services.AddControllers();

// Conex�o com o banco de dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton<DatabaseConnection>(sp => new DatabaseConnection(connectionString));
builder.Services.AddScoped<FoodDal>();
builder.Services.AddScoped<UserDal>(sp => new UserDal(connectionString));
builder.Services.AddScoped<FoodDiaryDal>(sp => new FoodDiaryDal(connectionString));
builder.Services.AddScoped<PremiumSubscriptionDal>(sp => new PremiumSubscriptionDal(connectionString));
builder.Services.AddScoped<ApiKeyDal>(sp => new ApiKeyDal(connectionString));
builder.Services.AddScoped<UserTokenDal>(sp => new UserTokenDal(connectionString)); // Adicionando o UserTokenDal
builder.Services.AddScoped<TokenAuthorizeAttribute>(); // Adicionando o filtro TokenAuthorize

// Configura��es do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middlewares
app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsProduction())
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles")),
        RequestPath = "/StaticFiles"
    });
}



app.UseHttpsRedirection();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseAuthorization();

// Rotas
app.Map("/docs", builder =>
{
    builder.Run(async context =>
    {
        context.Response.Redirect("/StaticFiles/docs.html");
    });
});
app.MapControllers();

app.Run();

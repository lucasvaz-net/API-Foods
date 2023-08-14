using Microsoft.Extensions.DependencyInjection;
using API.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Carrega o arquivo appsettings.json
builder.Configuration.AddJsonFile("appsettings.json");

// Add services to the container.
builder.Services.AddControllers();

// Injetando a classe de conexão ao banco de dados
builder.Services.AddSingleton<DatabaseConnection>(sp =>
    new DatabaseConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<FoodDal>();
builder.Services.AddScoped<UserDal>(sp => new UserDal(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<FoodDiaryDal>(sp => new FoodDiaryDal(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<PremiumSubscriptionDal>(sp => new PremiumSubscriptionDal(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger/OpenAPI setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.Map("/api", builder =>
{
    builder.Run(async context =>
    {
        context.Response.Redirect("/StaticFiles/docs.html");
    });
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles")),
    RequestPath = "/StaticFiles"
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
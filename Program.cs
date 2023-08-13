using Microsoft.Extensions.DependencyInjection;
using API.Data;  // Garanta que está importando o namespace correto para sua classe DatabaseConnection
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Carrega o arquivo appsettings.json
builder.Configuration.AddJsonFile("appsettings.json");

// Add services to the container.
builder.Services.AddControllers();

// Injetando a classe de conexão ao banco de dados
builder.Services.AddSingleton<DatabaseConnection>(sp =>
    new DatabaseConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<FoodDal>();

// Swagger/OpenAPI setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

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
builder.Services.AddScoped<UserDal>(sp => new UserDal(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<FoodDiaryDal>(sp => new FoodDiaryDal(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<PremiumSubscriptionDal>(sp => new PremiumSubscriptionDal(builder.Configuration.GetConnectionString("DefaultConnection")));



// Swagger/OpenAPI setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using API.Data;
using Microsoft.Extensions.FileProviders;
using API.Data.API.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddJsonFile("appsettings.json");


builder.Services.AddControllers();

builder.Services.AddSingleton<DatabaseConnection>(sp =>
    new DatabaseConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<FoodDal>();
builder.Services.AddScoped<UserDal>(sp => new UserDal(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<FoodDiaryDal>(sp => new FoodDiaryDal(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<PremiumSubscriptionDal>(sp => new PremiumSubscriptionDal(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ApiKeyDal>(sp => new ApiKeyDal(builder.Configuration.GetConnectionString("DefaultConnection"))); // Adicionado


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles")),
    RequestPath = "/StaticFiles"
});

app.UseHttpsRedirection();

// Registrar o middleware
app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();

app.Map("/docs", builder =>
{
    builder.Run(async context =>
    {
        context.Response.Redirect("/StaticFiles/docs.html");
    });
});

app.MapControllers();
app.Run();

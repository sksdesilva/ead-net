using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UserRegistrationAPI.Models;
using UserRegistrationAPI.Services;
using UserRegistrationAPI.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container, including MongoDB settings and services.

// 1. Configure MongoDB settings using the appsettings.json section "MongoDbSettings"
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// 2. Register MongoDbSettings for DI
builder.Services.AddSingleton<MongoDbSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

// 3. Register MongoClient for interacting with the MongoDB server
builder.Services.AddSingleton<IMongoClient>(s =>
    new MongoClient(builder.Configuration.GetValue<string>("MongoDbSettings:ConnectionString")));

// 4. Register application services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();

// 5. Register RoleAuthorizeFilter with parameters
builder.Services.AddScoped<RoleAuthorizeFilter>(sp =>
{
    var userService = sp.GetRequiredService<IUserService>();
    return new RoleAuthorizeFilter("Role", userService); // "Role" is a placeholder, actual role will be set by RoleAuthorizeAttribute
});

// 6. Add controller services
builder.Services.AddControllers();

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseAuthorization();

// Map controller endpoints to handle API requests
app.MapControllers();

// Run the application
app.Run();

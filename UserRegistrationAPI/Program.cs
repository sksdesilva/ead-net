using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UserRegistrationAPI.Models;
using UserRegistrationAPI.Services;
using UserRegistrationAPI.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

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
builder.Services.AddScoped<IVendorService, VendorService>();


// 5. Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Add the events here
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                // Log the failure message
                Console.WriteLine("Authentication failed: " + context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                // Log the claims if token is valid
                var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                Console.WriteLine("Token validated. Claims: " + string.Join(", ", claimsIdentity.Claims.Select(c => c.Type + ": " + c.Value)));
                return Task.CompletedTask;
            }
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// 6. Register RoleAuthorizeFilter with logger
builder.Services.AddScoped<RoleAuthorizeFilter>(sp =>
{
    var userService = sp.GetRequiredService<IUserService>();
    var logger = sp.GetRequiredService<ILogger<RoleAuthorizeFilter>>();
    return new RoleAuthorizeFilter("Role", userService, logger); // The role will be set in the attribute
});

// 7. Add authorization services
builder.Services.AddAuthorization();

// 8. Add controller services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseAuthentication();  // Ensure the authentication middleware is added
app.UseAuthorization();

// Map controller endpoints to handle API requests
app.MapControllers();

// Run the application
app.Run();

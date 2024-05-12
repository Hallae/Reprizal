global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using System.Text;
global using Microsoft.OpenApi.Models;
using myApi.Data;
using myApi.Services.UserService;
using Swashbuckle.AspNetCore.Filters;
using myApi.Interfaces;
using myApi.Repository;
using myApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Ensure the appsettings.json file is correctly loaded
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Define an authorization policy for application controller
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
        policy.RequireRole("Admin"));
});


builder.Services.AddHttpContextAccessor();


builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// Configure JWT Bearer authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });


    builder.Services.AddScoped<IGuidGenerator, GuidGenerator>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddControllers();
    builder.Services.AddRouting();
    builder.Services.AddScoped<IContextApplication, ApplicationRepository>();
 

    var app = builder.Build();

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
 

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthentication(); // Ensure authentication middleware is added

    app.UseAuthorization(); // Ensure authorization middleware is added

    // Map controllers to enable attribute routing
    app.MapControllers();

    app.Run();

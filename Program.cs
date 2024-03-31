global using Microsoft.EntityFrameworkCore;
global using Swashbuckle.AspNetCore;
global using Microsoft.EntityFrameworkCore.Design;
using myApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add dbcontext for sqlserver
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddDbContext<ActivityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ActivityConnection"));
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRouting();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

// Map controllers to enable attribute routing
app.MapControllers();

app.Run();

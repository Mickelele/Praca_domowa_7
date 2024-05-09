using Microsoft.EntityFrameworkCore;
using WebApplication4.Context;
using WebApplication4.Repositories;
using WebApplication4.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<WarehouseService>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<WarehouseRepository>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TestContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<TestContext>();

var app = builder.Build();

app.Configuration.GetConnectionString("Default");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();


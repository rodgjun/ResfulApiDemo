using Restful.Demo.Domain.Interfaces;
using Restful.Demo.Data.Command;
using MediatR;
using Restful.Demo.Domain.Models;
using Restful.Demo.Services.ServiceQuery;
using Restful.Demo.Services.CommandHandler;
using Restful.Demo.Services.QueryHandler;
using Restful.Demo.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Register HttpClient and ProductService
builder.Services.AddHttpClient<IProductServices, ProductService>(client =>
{
    client.BaseAddress = new Uri("https://api.restful-api.dev/");
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ProductQueryHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ProductCommandHandler).Assembly));
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddMemoryCache();  // For caching

// Dependency Injection
builder.Services.AddSingleton<IProductServices, ProductService>();  // Singleton for external API client
builder.Services.AddScoped<IProductRepository, ProductRepository>();  // Scoped for DB operations
//builder.Services.AddHttpClient<IProductServices, ProductService>();  // Configure HttpClient

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

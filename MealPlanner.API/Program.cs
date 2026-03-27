using MealPlanner.API.Menus;
using MealPlanner.Services;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

builder.Services.AddSingleton<InMemoryDatabase>();
await builder.Services.RegisterMenuServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Meal Planner API v1");
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
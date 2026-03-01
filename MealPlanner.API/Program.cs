using MealPlanner.API.DailyMenu;
using MealPlanner.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<InMemoryDatabase>();
await builder.Services.RegisterDailyMenuServices();

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

await app.UseDailyMenuEndpoints();

app.Run();
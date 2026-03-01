using MealPlanner.Services.DailyMenus.Create;
using Microsoft.AspNetCore.Mvc;

namespace MealPlanner.API.DailyMenus.Create;

public static class Endpoint
{
    public const string Address = "/daily-menu/create";
    
    public static async Task<Response> Create(
        [FromServices] ICreateDailyMenu dailyMenuCreator,
        [FromBody] Request request) =>
        await dailyMenuCreator.Create(request);
}
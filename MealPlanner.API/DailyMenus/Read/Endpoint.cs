using MealPlanner.Services.DailyMenus.Read;
using Microsoft.AspNetCore.Mvc;

namespace MealPlanner.API.DailyMenus.Read;

public static class Endpoint
{
    public const string Address = "/daily-menu/{id:guid}";
    
    public static async Task<IResult> Read(
        [FromServices] IReadDailyMenu dailyMenuReader,
        [FromRoute(Name = "id")] Guid id)
    {
        var dailyMenu = await dailyMenuReader.Get(id);
        return dailyMenu is null ? Results.NotFound() : Results.Ok(dailyMenu);
    }
}
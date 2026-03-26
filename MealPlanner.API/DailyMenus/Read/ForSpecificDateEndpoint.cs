using MealPlanner.Services.DailyMenus.Read;
using MealPlanner.Shared.DailyMenus.Responses;
using Microsoft.AspNetCore.Mvc;

namespace MealPlanner.API.DailyMenus.Read;

public static class ForSpecificDateEndpoint
{
    public const string Address = Constants.EndpointPrefix + "/{day:datetime}";
    
    [ProducesResponseType(typeof(GetDailyMenuResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public static async Task<IResult> Read([FromServices] IReadDailyMenu dailyMenuReader, [FromRoute(Name = "day")] DateOnly day)
    {
        var dailyMenu = await dailyMenuReader.Get(day);
        return dailyMenu is null ? Results.NotFound() : Results.Ok(dailyMenu);
    }
}
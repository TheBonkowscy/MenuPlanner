using MealPlanner.Services.DailyMenus.Create;
using MealPlanner.Shared.DailyMenus.Requests;
using MealPlanner.Shared.DailyMenus.Responses;
using Microsoft.AspNetCore.Mvc;

namespace MealPlanner.API.DailyMenus.Create;

public static class Endpoint
{
    public const string Address = Constants.EndpointPrefix;
    
    [ProducesResponseType(typeof(CreateDailyMenuResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public static async Task<CreateDailyMenuResponse> Create(
        [FromServices] ICreateDailyMenu dailyMenuCreator,
        [FromBody] CreateDailyMenuRequest createDailyMenuRequest) =>
        await dailyMenuCreator.Create(createDailyMenuRequest);
}
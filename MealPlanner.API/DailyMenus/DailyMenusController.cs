using MealPlanner.Services.DailyMenus.Create;
using MealPlanner.Services.DailyMenus.Read;
using MealPlanner.Shared.DailyMenus.Requests;
using MealPlanner.Shared.DailyMenus.Responses;
using Microsoft.AspNetCore.Mvc;

namespace MealPlanner.API.DailyMenus;

[ApiController]
[Route(Constants.EndpointPrefix)]
public class DailyMenusController(
    ICreateDailyMenu dailyMenuCreator,
    IReadDailyMenu dailyMenuReader) : ControllerBase
{
    [ProducesResponseType(typeof(CreateDailyMenuResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<CreateDailyMenuResponse> Create([FromBody] CreateDailyMenuRequest createDailyMenuRequest, CancellationToken cancellationToken) =>
        await dailyMenuCreator.Create(createDailyMenuRequest);
    
    [ProducesResponseType(typeof(GetDailyMenuResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id:guid}")]
    public async Task<IResult> GetById([FromRoute(Name = "id")] Guid id, CancellationToken cancellationToken)
    {
        var dailyMenu = await dailyMenuReader.Get(id);
        return dailyMenu is null ? Results.NotFound() : Results.Ok(dailyMenu);
    }
    
    [ProducesResponseType(typeof(GetDailyMenuResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{day:datetime}")]
    public async Task<IResult> GetForSpecificDate([FromRoute(Name = "day")] DateTime day, CancellationToken cancellationToken)
    {
        var date = DateOnly.FromDateTime(day);
        var dailyMenu = await dailyMenuReader.Get(date);
        return dailyMenu is null ? Results.NotFound() : Results.Ok(dailyMenu);
    }
    
    [ProducesResponseType(typeof(GetDailyMenuResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("today")]
    public async Task<IResult> GetForToday(CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var dailyMenu = await dailyMenuReader.Get(today);
        return dailyMenu is null ? Results.NotFound() : Results.Ok(dailyMenu);
    }
}
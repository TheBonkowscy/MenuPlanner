using MealPlanner.Domain;
using MealPlanner.Shared.DailyMenus.Responses;

namespace MealPlanner.Services.DailyMenus.Read;

public interface IReadDailyMenu
{
    Task<GetDailyMenuResponse?> Get(Guid id);
    Task<GetDailyMenuResponse?> Get(DateOnly date);
}

public class DailyMenuReader(InMemoryDatabase ctx) : IReadDailyMenu
{
    public Task<GetDailyMenuResponse?> Get(Guid id)
    {
        if (!ctx.Database.TryGetValue(id, out var dailyMenu))
        {
            return Task.FromResult<GetDailyMenuResponse?>(null);
        }

        return MapDailyMenu(dailyMenu);
    }

    private static Task<GetDailyMenuResponse?> MapDailyMenu(DailyMenu dailyMenu)
    {
        var mappedMeals = dailyMenu.Meals.Select(x => x.Value.Name);
        var mappedResponse = new GetDailyMenuResponse(dailyMenu.Id, dailyMenu.Date, mappedMeals);
        return Task.FromResult<GetDailyMenuResponse?>(mappedResponse);
    }

    public Task<GetDailyMenuResponse?> Get(DateOnly date)
    {
        var menuForDate = ctx.Database.Values.FirstOrDefault(x => x.Date == date);
        
        if (menuForDate is null)
        {
            return Task.FromResult<GetDailyMenuResponse?>(null);
        }

        return MapDailyMenu(menuForDate);
    }
}
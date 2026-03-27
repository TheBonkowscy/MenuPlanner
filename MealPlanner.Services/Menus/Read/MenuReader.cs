using MealPlanner.Domain;
using MealPlanner.Shared.Menus.Responses;

namespace MealPlanner.Services.Menus.Read;

public interface IReadMenu
{
    Task<GetMenuResponse?> Get(int id);
    Task<GetMenuResponse?> Get(DateOnly date);
}

public class MenuReader(InMemoryDatabase ctx) : IReadMenu
{
    public Task<GetMenuResponse?> Get(int id)
    {
        if (!ctx.Database.TryGetValue(id, out var menu))
        {
            return Task.FromResult<GetMenuResponse?>(null);
        }

        return MapMenu(menu);
    }

    private static Task<GetMenuResponse?> MapMenu(Menu menu)
    {
        var mappedMeals = menu.Items.Select(x => x.Meal.Name);
        var mappedResponse = new GetMenuResponse(menu.Id, menu.Date, mappedMeals);
        return Task.FromResult<GetMenuResponse?>(mappedResponse);
    }

    public Task<GetMenuResponse?> Get(DateOnly date)
    {
        var menuForDate = ctx.Database.Values.FirstOrDefault(x => x.Date == date);
        
        if (menuForDate is null)
        {
            return Task.FromResult<GetMenuResponse?>(null);
        }

        return MapMenu(menuForDate);
    }
}
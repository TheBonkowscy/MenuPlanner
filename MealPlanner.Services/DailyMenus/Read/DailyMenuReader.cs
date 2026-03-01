namespace MealPlanner.Services.DailyMenus.Read;

public interface IReadDailyMenu
{
    Task<Response?> Get(Guid id);
}

public class DailyMenuReader(InMemoryDatabase ctx) : IReadDailyMenu
{
    public Task<Response?> Get(Guid id)
    {
        if (!ctx.Database.TryGetValue(id, out var dailyMenu))
        {
            return Task.FromResult<Response?>(null);
        }

        var mappedMeals = dailyMenu.Meals.Select(x => x.Value.Name);
        var mappedResponse = new Response(id, dailyMenu.Date, mappedMeals);
        return Task.FromResult<Response?>(mappedResponse);
    }
}
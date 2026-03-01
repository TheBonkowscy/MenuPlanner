using System.Collections.Concurrent;
using MealPlanner.Domain;

namespace MealPlanner.Services.DailyMenus.Create;

public interface ICreateDailyMenu
{
    Task<Response> Create(Request request);
}

public class DailyMenuCreator : ICreateDailyMenu
{
    private static IDictionary<Guid, DailyMenu> IN_MEMORY_DATABASE = new ConcurrentDictionary<Guid, DailyMenu>();
    
    public Task<Response> Create(Request request)
    {
        try
        {
            var result = DailyMenu.Create(request.Date);
            if (request.Meals is not null)
            {
                var mappedMeals = request.Meals.Select(Meal.Create).ToList();
                mappedMeals.ForEach(meal => result.AddMeal(meal));
            }

            var id = Guid.NewGuid();
            IN_MEMORY_DATABASE[id] = result;

            return Task.FromResult(new Response(id));
        }
        catch (Exception exception)
        {
            return Task.FromException<Response>(exception);
        }
    }
}
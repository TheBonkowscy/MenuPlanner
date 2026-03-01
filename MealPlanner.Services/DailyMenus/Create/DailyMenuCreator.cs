using System.Collections.Concurrent;
using MealPlanner.Domain;

namespace MealPlanner.Services.DailyMenus.Create;

public interface ICreateDailyMenu
{
    Task<Response> Create(Request request);
}

public class DailyMenuCreator : ICreateDailyMenu
{
    public Task<Response> Create(Request request)
    {
        try
        {
            var existingMenuForDay = InMemoryDatabase.Database.Values.FirstOrDefault(x => x.Date == request.Date);
            if (existingMenuForDay is not null)
            {
                throw new InvalidOperationException($"There is already a Daily Menu defined for {request.Date}.");
            }
            
            var result = DailyMenu.Create(request.Date);
            if (request.Meals is not null)
            {
                var mappedMeals = request.Meals.Select(Meal.Create).ToList();
                mappedMeals.ForEach(meal => result.AddMeal(meal));
            }

            var id = Guid.NewGuid();
            InMemoryDatabase.Database[id] = result;

            return Task.FromResult(new Response(id));
        }
        catch (Exception exception)
        {
            return Task.FromException<Response>(exception);
        }
    }
}
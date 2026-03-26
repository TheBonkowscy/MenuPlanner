using MealPlanner.Domain;
using MealPlanner.Shared.DailyMenus.Requests;
using MealPlanner.Shared.DailyMenus.Responses;

namespace MealPlanner.Services.DailyMenus.Create;

public interface ICreateDailyMenu
{
    Task<CreateDailyMenuResponse> Create(CreateDailyMenuRequest createDailyMenuRequest);
}

public class DailyMenuCreator(InMemoryDatabase ctx) : ICreateDailyMenu
{
    public Task<CreateDailyMenuResponse> Create(CreateDailyMenuRequest createDailyMenuRequest)
    {
        try
        {
            var existingMenuForDay = ctx.Database.Values.FirstOrDefault(x => x.Date == createDailyMenuRequest.Date);
            if (existingMenuForDay is not null)
            {
                throw new InvalidOperationException($"There is already a Daily Menu defined for {createDailyMenuRequest.Date}.");
            }
            
            var result = DailyMenu.Create(createDailyMenuRequest.Date);
            if (createDailyMenuRequest.Meals is not null)
            {
                var mappedMeals = createDailyMenuRequest.Meals.Select(Meal.Create).ToList();
                mappedMeals.ForEach(meal => result.AddMeal(meal));
            }

            ctx.Database[result.Id] = result;

            return Task.FromResult(new CreateDailyMenuResponse(result.Id));
        }
        catch (Exception exception)
        {
            return Task.FromException<CreateDailyMenuResponse>(exception);
        }
    }
}
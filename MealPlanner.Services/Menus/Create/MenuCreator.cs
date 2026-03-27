using MealPlanner.Domain;
using MealPlanner.Shared.Menus.Requests;
using MealPlanner.Shared.Menus.Responses;

namespace MealPlanner.Services.Menus.Create;

public interface ICreateMenu
{
    Task<CreateMenuResponse> Create(CreateMenuRequest createMenuRequest);
}

public class MenuCreator(InMemoryDatabase ctx) : ICreateMenu
{
    public Task<CreateMenuResponse> Create(CreateMenuRequest createMenuRequest)
    {
        try
        {
            var existingMenuForDay = ctx.Database.Values.FirstOrDefault(x => x.Date == createMenuRequest.Date);
            if (existingMenuForDay is not null)
            {
                throw new InvalidOperationException($"There is already a Menu defined for {createMenuRequest.Date}.");
            }
            
            var result = Menu.Create(createMenuRequest.Date);
            if (createMenuRequest.Meals is not null)
            {
                var mappedMeals = createMenuRequest.Meals.Select(Meal.Create).ToList();
                mappedMeals.ForEach(meal => result.AddMeal(meal));
            }

            ctx.Database[result.Id] = result;

            return Task.FromResult(new CreateMenuResponse(result.Id));
        }
        catch (Exception exception)
        {
            return Task.FromException<CreateMenuResponse>(exception);
        }
    }
}
using MealPlanner.Services.DailyMenus.Create;
using MealPlanner.Services.DailyMenus.Read;

namespace MealPlanner.API.DailyMenus;

public static class Extensions
{
    public static Task RegisterDailyMenuServices(this IServiceCollection services)
    {
        services.AddTransient<ICreateDailyMenu, DailyMenuCreator>();
        services.AddTransient<IReadDailyMenu, DailyMenuReader>();
        return Task.CompletedTask;
    }
}
using MealPlanner.Services.Menus.Create;
using MealPlanner.Services.Menus.Read;

namespace MealPlanner.API.Menus;

public static class Extensions
{
    public static Task RegisterMenuServices(this IServiceCollection services)
    {
        services.AddTransient<ICreateMenu, MenuCreator>();
        services.AddTransient<IReadMenu, MenuReader>();
        return Task.CompletedTask;
    }
}
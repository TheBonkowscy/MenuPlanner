using MealPlanner.Services.DailyMenus.Create;

namespace MealPlanner.API.DailyMenu;

public static class Extensions
{
    private const string OpenApiGroupName = "Daily Menu";

    public static Task UseDailyMenuEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(Create.Endpoint.Address, Create.Endpoint.Create)
            .WithDisplayName("Create Daily Menu")
            .WithGroupName(OpenApiGroupName);

        return Task.CompletedTask;
    }

    public static Task RegisterDailyMenuServices(this IServiceCollection services)
    {
        services.AddTransient<ICreateDailyMenu, DailyMenuCreator>();
        return Task.CompletedTask;
    }
}
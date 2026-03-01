using MealPlanner.Services.DailyMenus.Create;
using MealPlanner.Services.DailyMenus.Read;

namespace MealPlanner.API.DailyMenus;

public static class Extensions
{
    private const string OpenApiGroupName = "Daily Menu";

    public static Task UseDailyMenuEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(Create.Endpoint.Address, Create.Endpoint.Create)
            .WithDisplayName("Create Daily Menu")
            .WithGroupName(OpenApiGroupName);
        
        app.MapGet(Read.Endpoint.Address, Read.Endpoint.Read)
            .WithDisplayName("Read Daily Menu")
            .WithGroupName(OpenApiGroupName);

        return Task.CompletedTask;
    }

    public static Task RegisterDailyMenuServices(this IServiceCollection services)
    {
        services.AddTransient<ICreateDailyMenu, DailyMenuCreator>();
        services.AddTransient<IReadDailyMenu, DailyMenuReader>();
        return Task.CompletedTask;
    }
}
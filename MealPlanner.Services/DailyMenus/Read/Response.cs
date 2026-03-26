namespace MealPlanner.Services.DailyMenus.Read;

public record Response(Guid Id, DateOnly Date, IEnumerable<string> Meals);
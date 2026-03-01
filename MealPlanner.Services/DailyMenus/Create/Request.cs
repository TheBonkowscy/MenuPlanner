namespace MealPlanner.Services.DailyMenus.Create;

public record Request(DateOnly Date, string[]? Meals = null);
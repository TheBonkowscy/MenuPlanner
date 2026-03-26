namespace MealPlanner.Shared.DailyMenus.Requests;

public record CreateDailyMenuRequest(DateOnly Date, string[]? Meals = null);
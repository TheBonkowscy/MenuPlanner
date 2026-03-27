namespace MealPlanner.Shared.Menus.Requests;

public record CreateMenuRequest(DateOnly Date, string[]? Meals = null);
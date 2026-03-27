namespace MealPlanner.Shared.Menus.Responses;

public record GetMenuResponse(Guid Id, DateOnly Date, IEnumerable<string> Meals);
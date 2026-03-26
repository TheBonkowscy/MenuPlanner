namespace MealPlanner.Shared.DailyMenus.Responses;

public record GetDailyMenuResponse(Guid Id, DateOnly Date, IEnumerable<string> Meals);
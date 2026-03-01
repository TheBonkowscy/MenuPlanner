using System.Collections.Concurrent;
using MealPlanner.Domain;

namespace MealPlanner.Services.DailyMenus;

public static class InMemoryDatabase
{
    public static IDictionary<Guid, DailyMenu> Database { get; } = new ConcurrentDictionary<Guid, DailyMenu>();
}
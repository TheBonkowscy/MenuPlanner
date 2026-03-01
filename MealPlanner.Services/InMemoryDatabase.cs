using System.Collections.Concurrent;
using MealPlanner.Domain;

namespace MealPlanner.Services;

public class InMemoryDatabase
{
    public IDictionary<Guid, DailyMenu> Database { get; } = new ConcurrentDictionary<Guid, DailyMenu>();
}
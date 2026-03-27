using System.Collections.Concurrent;
using MealPlanner.Domain;

namespace MealPlanner.Services;

public class InMemoryDatabase
{
    public IDictionary<int, Menu> Database { get; } = new ConcurrentDictionary<int, Menu>();
}
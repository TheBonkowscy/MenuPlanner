using System.Collections.Concurrent;
using MealPlanner.Domain;

namespace MealPlanner.Services;

public class InMemoryDatabase
{
    public IDictionary<Guid, Menu> Database { get; } = new ConcurrentDictionary<Guid, Menu>();
}
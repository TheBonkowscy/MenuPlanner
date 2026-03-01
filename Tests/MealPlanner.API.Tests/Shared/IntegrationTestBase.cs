using MealPlanner.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MealPlanner.API.Tests.Shared;

public class IntegrationTestBase : IClassFixture<MealPlannerWebApplicationFactory>, IDisposable, IAsyncDisposable
{
    private readonly MealPlannerWebApplicationFactory _factory = new();
    private readonly WebApplicationFactoryClientOptions _options = new()
    {
        BaseAddress = new Uri("https://localhost:5001"),
        AllowAutoRedirect = true
    };
    
    protected IServiceScope ServiceScope => _factory.Services.CreateScope();

    public HttpClient Client => _factory.CreateClient(_options);
    
    protected InMemoryDatabase ctx => ServiceScope.ServiceProvider.GetRequiredService<InMemoryDatabase>() ?? throw new Exception("Could not retrieve database instance");

    public void Dispose()
    {
        _factory.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _factory.DisposeAsync();
    }
}
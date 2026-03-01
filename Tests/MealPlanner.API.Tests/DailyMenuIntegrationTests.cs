using AwesomeAssertions;
using MealPlanner.API.Tests.Shared;
using MealPlanner.Services.DailyMenus.Create;
using Xunit;
using Endpoint = MealPlanner.API.DailyMenu.Create.Endpoint;

namespace MealPlanner.API.Tests;

public class DailyMenuIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task Post_DailyMenu_ReturnsId()
    {
        // Arrange
        var request = new Request(DateOnly.FromDateTime(DateTime.Today));
        
        // Act
        var result = await Client.PostAsJsonAsync(Endpoint.Address, request);
        
        // Assert
        result.EnsureSuccessStatusCode();
        var response = await result.Content.ReadFromJsonAsync<Response>();
        response.Should().NotBeNull();
        response.Id.Should().NotBe(Guid.Empty);
    }
}
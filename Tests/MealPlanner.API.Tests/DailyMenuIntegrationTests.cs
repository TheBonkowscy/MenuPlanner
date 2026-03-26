using System.Net;
using AwesomeAssertions;
using MealPlanner.API.Tests.Shared;
using MealPlanner.Domain;
using MealPlanner.Shared.DailyMenus.Requests;
using MealPlanner.Shared.DailyMenus.Responses;
using Xunit;
using Create = MealPlanner.API.DailyMenus.Create;
using Read = MealPlanner.API.DailyMenus.Read;

namespace MealPlanner.API.Tests;

public class DailyMenuIntegrationTests : IntegrationTestBase
{
    private static readonly DateOnly Today = DateOnly.FromDateTime(DateTime.Today);
    private static readonly DateOnly Tomorrow = Today.AddDays(1);
    private static readonly DateOnly SpecificDate = new(2026, 03, 26);
    
    [Fact]
    public async Task Post_ReturnsId()
    {
        // Arrange
        var request = new CreateDailyMenuRequest(Tomorrow);
        
        // Act
        var result = await Client.PostAsJsonAsync(Create.Endpoint.Address, request);
        
        // Assert
        result.EnsureSuccessStatusCode();
        var response = await result.Content.ReadFromJsonAsync<CreateDailyMenuResponse>();
        response.Should().NotBeNull();
        response.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Get_ById_ReturnsDailyMenuIfExists()
    {
        // Arrange
        var dailyMenu = DailyMenu.Create(Tomorrow);
        ctx.Database[dailyMenu.Id] = dailyMenu;
        
        // Act
        var result = await Client.GetAsync(Read.ByIdEndpoint.Address.Replace("{id:guid}", dailyMenu.Id.ToString()));
        
        // Assert
        result.EnsureSuccessStatusCode();
        var response = await result.Content.ReadFromJsonAsync<GetDailyMenuResponse>();
        response.Should().NotBeNull();
        response.Id.Should().Be(dailyMenu.Id);
        response.Date.Should().Be(dailyMenu.Date);
    }
    
    [Fact]
    public async Task Get_ById_ReturnsNotFound_WhenDailyMenuDoesNotExist()
    {
        // Act
        var result = await Client.GetAsync(Read.ByIdEndpoint.Address.Replace("{id:guid}", Guid.NewGuid().ToString()));
        
        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task Get_ForSpecificDate_ReturnsNotFound_WhenDailyMenuDoesNotExist()
    {
        // Act
        var result = await Client.GetAsync(Read.ForSpecificDateEndpoint.Address.Replace("{day:datetime}", SpecificDate.ToShortDateString()));
        
        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_ForSpecificDate_ReturnsDailyMenuIfExists()
    {
        // Arrange
        var dailyMenu = DailyMenu.Create(SpecificDate);
        ctx.Database[dailyMenu.Id] = dailyMenu;
        
        // Act
        var result = await Client.GetAsync(Read.ForSpecificDateEndpoint.Address.Replace("{day:datetime}", dailyMenu.Date.ToString("O")));
        
        // Assert
        result.EnsureSuccessStatusCode();
        var response = await result.Content.ReadFromJsonAsync<GetDailyMenuResponse>();
        response.Should().NotBeNull();
        response.Id.Should().Be(dailyMenu.Id);
        response.Date.Should().Be(dailyMenu.Date);
    }
    
    [Fact]
    public async Task Get_ForToday_ReturnsNotFound_WhenDailyMenuDoesNotExist()
    {
        // Act
        var result = await Client.GetAsync(Read.ForTodayEndpoint.Address);
        
        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_ForToday_ReturnsDailyMenuIfExists()
    {
        // Arrange
        var dailyMenu = DailyMenu.Create(Today);
        ctx.Database[dailyMenu.Id] = dailyMenu;
        
        // Act
        var result = await Client.GetAsync(Read.ForTodayEndpoint.Address);
        
        // Assert
        result.EnsureSuccessStatusCode();
        var response = await result.Content.ReadFromJsonAsync<GetDailyMenuResponse>();
        response.Should().NotBeNull();
        response.Id.Should().Be(dailyMenu.Id);
        response.Date.Should().Be(Today);
    }
}
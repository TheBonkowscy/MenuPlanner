using AwesomeAssertions;
using MealPlanner.Domain;
using MealPlanner.Services.DailyMenus.Read;

namespace MealPlanner.Services.Tests;

public class DailyMenuReaderTests
{
    private readonly InMemoryDatabase _ctx;
    private readonly DailyMenuReader _sut;

    public DailyMenuReaderTests()
    {
        _ctx = new InMemoryDatabase();
        _sut = new DailyMenuReader(_ctx);
    }
    
    [Fact]
    public async Task GetById_ReturnsNull_WhenDailyMenuDoesNotExist()
    {
        // Arrange 
        var id = Guid.NewGuid();
        
        // Act
        var result = await _sut.Get(id);
        
        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task GetById_ReturnsDailyMenuIfExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dailyMenu = DailyMenu.Create(DateOnly.FromDateTime(DateTime.Today));
        _ctx.Database[id] = dailyMenu;
        
        // Act
        var result = await _sut.Get(id);
        
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.Date.Should().Be(dailyMenu.Date);
    }
}
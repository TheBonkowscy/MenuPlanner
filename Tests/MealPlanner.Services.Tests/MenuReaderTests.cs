using AwesomeAssertions;
using MealPlanner.Domain;
using MealPlanner.Services.Menus.Read;

namespace MealPlanner.Services.Tests;

public class MenuReaderTests
{
    private static readonly DateOnly Today = DateOnly.FromDateTime(DateTime.Today);
    
    private readonly InMemoryDatabase _ctx;
    private readonly MenuReader _sut;

    public MenuReaderTests()
    {
        _ctx = new InMemoryDatabase();
        _sut = new MenuReader(_ctx);
    }
    
    [Fact]
    public async Task GetById_ReturnsNull_WhenMenuDoesNotExist()
    {
        // Arrange 
        var id = Guid.NewGuid();
        
        // Act
        var result = await _sut.Get(id);
        
        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task GetById_ReturnsMenuIfExists()
    {
        // Arrange
        var menu = Menu.Create(Today);
        _ctx.Database[menu.Id] = menu;
        
        // Act
        var result = await _sut.Get(menu.Id);
        
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(menu.Id);
        result.Date.Should().Be(menu.Date);
    }

    [Fact]
    public async Task GetForDate_ReturnsNull_WhenMenuDoesNotExist()
    {
        // Arrange 
        var id = Guid.NewGuid();
        
        // Act
        var result = await _sut.Get(Today);
        
        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task GetForDate_ReturnsMenuIfExists()
    {
        // Arrange
        var menu = Menu.Create(Today);
        _ctx.Database[menu.Id] = menu;
        
        // Act
        var result = await _sut.Get(Today);
        
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(menu.Id);
        result.Date.Should().Be(menu.Date);
    }
    
}
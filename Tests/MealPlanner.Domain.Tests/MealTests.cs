using AwesomeAssertions;

namespace MealPlanner.Domain.Tests;

public class MealTests
{
    [Fact]
    public void Create_WithoutName_ThrowsException()
    {   
        // Act
        Action<string> createMeal = (name) => Meal.Create(name);
        
        // Assert
        createMeal.Invoking(x => x.Invoke(""))
            .Should().Throw<ArgumentNullException>()
            .WithMessage("Please specify a name of the meal");
    }

    [Fact]
    public void Create_WithName_Succeeds()
    {
        // Arrange
        const string name = "Fish and chips";
        
        // Act
        var meal = Meal.Create(name);
        
        // Assert
        meal.Should().NotBeNull();
        meal.Name.Should().Be(name);
    }
}
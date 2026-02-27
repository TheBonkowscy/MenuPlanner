using AwesomeAssertions;

namespace MealPlanner.Domain.Tests;

public class DailyMenuTests
{
    private static readonly DateOnly SharedDate = DateOnly.FromDateTime(DateTime.UtcNow);
    private static readonly Meal SharedFirstMeal = Meal.Create("Fish and chips");
    private static readonly Meal SharedSecondMeal = Meal.Create("Pierogi");
    private static readonly string InvalidDateExceptionMessage = $"Invalid date specified. The date can not be before {DailyMenu.MinDateInThePast} and must be in the near future.";

    [Theory]
    [MemberData(nameof(InvalidDatesSource))]
    public void Create_ThrowsForInvalidDate(DateOnly invalidDate)
    {
        // Act
        Action<DateOnly> createNewDailyMenu = date => DailyMenu.Create(date);
        
        // Assert
        createNewDailyMenu.Invoking(x => x.Invoke(invalidDate))
            .Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage(InvalidDateExceptionMessage);
    }

    [Theory]
    [MemberData(nameof(ValidDatesSource))]
    public void Create_CreatesSuccessfully(DateOnly validDate)
    {
        // Act
        var result = DailyMenu.Create(validDate);
        
        // Assert
        result.Date.Should().Be(validDate);
    }

    [Fact]
    public void AddMeal_WithoutOrder_SuccessfullyAddsMeal()
    {
        // Arrange
        var dailyMenu = DailyMenu.Create(SharedDate);
        
        // Act
        dailyMenu.AddMeal(SharedFirstMeal);
        
        // Assert
        dailyMenu.Meals.Should().HaveCount(1);
        dailyMenu.Meals.Values.First().Should().Be(SharedFirstMeal);
    }

    [Fact]
    public void AddMeal_WithoutOrder_KeepsOrder()
    {
        // Arrange
        var dailyMenu = DailyMenu.Create(SharedDate);
        dailyMenu.AddMeal(SharedFirstMeal);
        
        // Act
        dailyMenu.AddMeal(SharedSecondMeal);
        
        // Assert
        dailyMenu.Meals.Should().HaveCount(2);
        var actualFirstMeal = dailyMenu.Meals[0];
        actualFirstMeal.Should().Be(SharedFirstMeal);
        var actualSecondMeal = dailyMenu.Meals[1];
        actualSecondMeal.Should().Be(SharedSecondMeal);
    }

    [Fact]
    public void AddMeal_WithMealAlreadyAdded_ThrowsException()
    {
        // Arrange
        var dailyMenu = DailyMenu.Create(SharedDate);
        dailyMenu.AddMeal(SharedFirstMeal);
        
        // Act
        Action<Meal> addMealToDailyMenu = newMeal => dailyMenu.AddMeal(newMeal);
        
        // Assert
        addMealToDailyMenu.Invoking(x => x.Invoke(SharedFirstMeal))
            .Should().Throw<InvalidOperationException>()
            .WithMessage($"Meal '{SharedFirstMeal}' is already present in the daily menu for {dailyMenu.Date}.");
    }

    [Fact]
    public void AddMeal_WithOrder_SuccessfullyAddsMeal()
    {
        // Arrange
        var dailyMenu = DailyMenu.Create(SharedDate);
        
        // Act
        dailyMenu.AddMeal(0, SharedFirstMeal);
        
        // Assert
        dailyMenu.Meals.Should().HaveCount(1);
        dailyMenu.Meals.Values.Should().Contain(x => x.Name == SharedFirstMeal.Name);
    }

    [Fact]
    public void AddMeal_WithOrder_WhenOrderAlreadyTaken_ThrowsException()
    {
        // Arrange
        var dailyMenu = DailyMenu.Create(SharedDate);
        const int mealOrder = 0;
        dailyMenu.AddMeal(mealOrder, SharedFirstMeal);
        
        // Act
        Action<int, Meal> addMealToDailyMenu = (order, meal) =>  dailyMenu.AddMeal(order, meal);
        
        // Assert
        addMealToDailyMenu.Invoking(x => x.Invoke(mealOrder, SharedSecondMeal))
            .Should().Throw<InvalidOperationException>()
            .WithMessage($"There is already a meal added as #{mealOrder + 1} in the day");
    }

    [Fact]
    public void AddMeal_WithOrder_WithMealAlreadyAdded_ThrowsException()
    {
        // Arrange
        var dailyMenu = DailyMenu.Create(SharedDate);
        dailyMenu.AddMeal(SharedFirstMeal);
        
        // Act
        Action<int, Meal> addMealToDailyMenu = (order, meal) =>  dailyMenu.AddMeal(order, meal);
        
        // Assert
        addMealToDailyMenu.Invoking(x => x.Invoke(1, SharedFirstMeal))
            .Should().Throw<InvalidOperationException>()
            .WithMessage($"Meal '{SharedFirstMeal}' is already present in the daily menu for {dailyMenu.Date}.");
    }

    [Fact]
    public void AddMeal_WithOrder_ThrowsExceptionForNegativeOrder()
    {
        // Arrange
        var dailyMenu = DailyMenu.Create(SharedDate);
        
        // Act
        Action<int, Meal> addMealToDailyMenu = (order, meal) => dailyMenu.AddMeal(order, meal);
        
        // Assert
        addMealToDailyMenu.Invoking(x => x.Invoke(-1, SharedFirstMeal))
            .Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Order must be a positive number.");
    }

    public static TheoryData<DateOnly> InvalidDatesSource
    {
        get
        {
            var data = new TheoryData<DateOnly>
            {
                DateOnly.MinValue,
                DateOnly.MaxValue, 
                DailyMenu.MinDateInThePast.AddDays(-1),
                DateOnly.FromDateTime(DateTime.UtcNow).AddYears(100).AddDays(1)
                
            };
            return data;
        }
    }

    public static TheoryData<DateOnly> ValidDatesSource
    {
        get
        {
            var data = new TheoryData<DateOnly>
            {
                DateOnly.FromDateTime(DateTime.UtcNow),
                DailyMenu.MinDateInThePast,
                DateOnly.FromDateTime(DateTime.UtcNow).AddYears(100)
                
            };
            return data;
        }
    }
}
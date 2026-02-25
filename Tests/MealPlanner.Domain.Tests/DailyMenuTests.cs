using AwesomeAssertions;

namespace MealPlanner.Domain.Tests;

public class DailyMenuTests
{
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
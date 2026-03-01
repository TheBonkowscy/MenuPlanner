using AwesomeAssertions;
using MealPlanner.Services.DailyMenus.Create;

namespace MealPlanner.Services.Tests;

public class DailyMenuCreatorTests
{
    private const string MealName = "Fish and chips";
    
    private readonly DailyMenuCreator _sut = new();
    
    [Theory]
    [MemberData(nameof(ValidCreateRequests))]
    public async Task Create_CreatesSuccessfully_ReturnsId(Request request)
    {
        // Act
        var result = await _sut.Create(request);
        
        // Assert
        result.Should().NotBeSameAs(Guid.Empty);
    }

    [Fact]
    public async Task Create_ThrowsWhenDailyMenuAlreadyPresentForSpecifiedDay()
    {
        // Arrange
        var tomorrow =  DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        var request = new Request(tomorrow, [MealName]);
        var conflictingRequest = new Request(tomorrow, ["Pierogi"]);
        await _sut.Create(request);

        // Act
        var createWithConflict = async (Request req) => await _sut.Create(req);
        
        // Assert
        await createWithConflict.Awaiting(x => x.Invoke(conflictingRequest))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"There is already a Daily Menu defined for {conflictingRequest.Date}.");
    }

    public static TheoryData<Request> ValidCreateRequests
    {
        get
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var data = new TheoryData<Request>
            {
                new(today, [MealName]),
                new(today.AddDays(1), []),
                new(today.AddDays(2))
            };

            return data;
        }
    }
}
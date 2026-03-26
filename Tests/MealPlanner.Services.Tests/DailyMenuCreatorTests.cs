using AwesomeAssertions;
using MealPlanner.Services.DailyMenus.Create;
using MealPlanner.Shared.DailyMenus.Requests;

namespace MealPlanner.Services.Tests;

public class DailyMenuCreatorTests
{
    private const string MealName = "Fish and chips";

    private readonly InMemoryDatabase _ctx;
    private readonly DailyMenuCreator _sut;

    public DailyMenuCreatorTests()
    {
        _ctx = new InMemoryDatabase();
        _sut = new DailyMenuCreator(_ctx);
    }
    
    [Theory]
    [MemberData(nameof(ValidCreateRequests))]
    public async Task Create_CreatesSuccessfully_ReturnsId(CreateDailyMenuRequest createDailyMenuRequest)
    {
        // Act
        var result = await _sut.Create(createDailyMenuRequest);
        
        // Assert
        result.Should().NotBeSameAs(Guid.Empty);
        
        // Cleanup
        _ctx.Database.Clear();
    }

    [Fact]
    public async Task Create_ThrowsWhenDailyMenuAlreadyPresentForSpecifiedDay()
    {
        // Arrange
        var tomorrow =  DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        var request = new CreateDailyMenuRequest(tomorrow, [MealName]);
        await _sut.Create(request);
        var conflictingRequest = new CreateDailyMenuRequest(tomorrow, ["Pierogi"]);

        // Act
        var createWithConflict = async (CreateDailyMenuRequest req) => await _sut.Create(req);
        
        // Assert
        await createWithConflict.Awaiting(x => x.Invoke(conflictingRequest))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"There is already a Daily Menu defined for {conflictingRequest.Date}.");
        
        // Cleanup
        _ctx.Database.Clear();
    }

    public static TheoryData<CreateDailyMenuRequest> ValidCreateRequests
    {
        get
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var data = new TheoryData<CreateDailyMenuRequest>
            {
                new(today, [MealName]),
                new(today.AddDays(1), []),
                new(today.AddDays(2))
            };

            return data;
        }
    }
}
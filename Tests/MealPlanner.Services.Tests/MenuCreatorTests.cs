using AwesomeAssertions;
using MealPlanner.Services.Menus.Create;
using MealPlanner.Shared.Menus.Requests;

namespace MealPlanner.Services.Tests;

public class MenuCreatorTests
{
    private const string MealName = "Fish and chips";

    private readonly InMemoryDatabase _ctx;
    private readonly MenuCreator _sut;

    public MenuCreatorTests()
    {
        _ctx = new InMemoryDatabase();
        _sut = new MenuCreator(_ctx);
    }
    
    [Theory]
    [MemberData(nameof(ValidCreateRequests))]
    public async Task Create_CreatesSuccessfully_ReturnsId(CreateMenuRequest createMenuRequest)
    {
        // Act
        var result = await _sut.Create(createMenuRequest);
        
        // Assert
        result.Should().NotBeSameAs(Guid.Empty);
        
        // Cleanup
        _ctx.Database.Clear();
    }

    [Fact]
    public async Task Create_ThrowsWhenMenuAlreadyPresentForSpecifiedDay()
    {
        // Arrange
        var tomorrow =  DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        var request = new CreateMenuRequest(tomorrow, [MealName]);
        await _sut.Create(request);
        var conflictingRequest = new CreateMenuRequest(tomorrow, ["Pierogi"]);

        // Act
        var createWithConflict = async (CreateMenuRequest req) => await _sut.Create(req);
        
        // Assert
        await createWithConflict.Awaiting(x => x.Invoke(conflictingRequest))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"There is already a Menu defined for {conflictingRequest.Date}.");
        
        // Cleanup
        _ctx.Database.Clear();
    }

    public static TheoryData<CreateMenuRequest> ValidCreateRequests
    {
        get
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var data = new TheoryData<CreateMenuRequest>
            {
                new(today, [MealName]),
                new(today.AddDays(1), []),
                new(today.AddDays(2))
            };

            return data;
        }
    }
}
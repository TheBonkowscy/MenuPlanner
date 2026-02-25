namespace MealPlanner.Domain;

public class DailyMenu
{
    public static readonly DateOnly MinDateInThePast = new(2019, 9, 28);

    public DateOnly Date { get; private set; }

    private DailyMenu(DateOnly date)
    {
        Date = date;
    }

    public static DailyMenu Create(DateOnly date)
    {
        ValidateDateAndThrow(date);
        return new DailyMenu(date);
    }

    private static void ValidateDateAndThrow(DateOnly date)
    {
        DateOnly[] invalidDates = [DateOnly.MinValue, DateOnly.MaxValue];
        var dateTooFarInThePast = date < MinDateInThePast;
        var dateTooFarInTheFuture = DateOnly.FromDateTime(DateTime.UtcNow).AddYears(100) < date;
        var dateIsInvalid = invalidDates.Contains(date) || dateTooFarInThePast || dateTooFarInTheFuture;
        if (dateIsInvalid)
        {
            throw new ArgumentOutOfRangeException(null, $"Invalid date specified. The date can not be before {MinDateInThePast} and must be in the near future.");
        }
    }
}
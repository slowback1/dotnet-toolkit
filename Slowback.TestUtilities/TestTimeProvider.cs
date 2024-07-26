using Slowback.Time;

namespace Slowback.TestUtilities;

public class TestTimeProvider : ITimeProvider
{
    private readonly DateTime _now;

    public TestTimeProvider(DateTime now)
    {
        _now = now;
    }

    public DateTime Now()
    {
        return _now;
    }

    public DateTime Today()
    {
        return _now.Date;
    }

    public DateTime FromString(string value)
    {
        return DateTime.Parse(value);
    }

    public DateTime UtcNow()
    {
        return _now.ToUniversalTime();
    }

    public DateTime AddDays(DateTime date, int days)
    {
        return date.AddDays(days);
    }

    public DateTime AddHours(DateTime date, int hours)
    {
        return date.AddHours(hours);
    }

    public DateTime AddMinutes(DateTime date, int minutes)
    {
        return date.AddMinutes(minutes);
    }

    public DateTime AddSeconds(DateTime date, int seconds)
    {
        return date.AddSeconds(seconds);
    }
}


using Application.Services;

namespace Tests;

public class FakeDateProvider : IDateProvider
{
    private DateTimeOffset injectedDate;

    public FakeDateProvider(DateTimeOffset injectedDate)
    {
        this.injectedDate = injectedDate;
    }

    public DateTimeOffset CurrentDate()
    {
        return injectedDate;
    }
}
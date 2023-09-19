using ExpenseReportCSharp.Services;

namespace Tests;

public class FakeDateProvider : DateProvider
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
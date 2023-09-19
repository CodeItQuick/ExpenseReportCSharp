namespace ExpenseReportCSharp.Services;

public class RealDateProvider : DateProvider
{
    public RealDateProvider() {
    }

    public DateTimeOffset CurrentDate() {
        return DateTimeOffset.Now;
    }
}
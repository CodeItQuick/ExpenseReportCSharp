using ExpenseReport.ApplicationServices;

namespace Application.Adapter;

public class RealDateProvider : IDateProvider
{
    public DateTimeOffset CurrentDate() {
        return DateTimeOffset.Now;
    }
}
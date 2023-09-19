namespace ExpenseReportCSharp.Services;

public interface DateProvider
{
    DateTimeOffset CurrentDate();
}
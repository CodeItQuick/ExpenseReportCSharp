namespace ExpenseReport.ApplicationServices;

public interface IDateProvider
{

    public DateTimeOffset CurrentDate();
}
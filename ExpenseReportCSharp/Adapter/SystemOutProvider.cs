namespace ExpenseReportCSharp.Adapter;

public class SystemOutProvider
{
    public virtual void ServicePrint(String message) {
        Console.WriteLine(message);
    }

}
using System.Collections;

namespace ExpenseReportCSharp.Services;

public class ExpensesDatabase
{
    public List<ExpenseDto> expenses = new List<ExpenseDto>() { 
        new ExpenseDto(ExpenseType.BREAKFAST, 500),
        new ExpenseDto(ExpenseType.DINNER, 5010),
        new ExpenseDto(ExpenseType.CAR_RENTAL, 1010)
    };

}
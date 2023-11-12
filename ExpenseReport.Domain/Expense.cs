namespace Domain;

public class Expense
{
    private readonly ExpenseType type;
    private readonly int amount;

    public Expense(ExpenseType type, int amount) {
        this.type = type;
        this.amount = amount;
    }

    public int CalculateMealExpenses() {
        if (type == Domain.ExpenseType.DINNER || type == Domain.ExpenseType.BREAKFAST) {
            return amount;
        }
        return 0;
    }

    public String IsOverExpensedMeal() {
        bool dinnerOverExpensed = type == Domain.ExpenseType.DINNER && amount > 5000;
        bool breakfastOverExpensed = type == Domain.ExpenseType.BREAKFAST && amount > 1000;
        return dinnerOverExpensed || breakfastOverExpensed ? "X" : " ";
    }

    public String? ExpenseType() {
        return type.ToString();
    }
    public ExpenseType ExpenseTypes() {
        return type;
    }

    public int Amount() {
        return amount;
    }
}
namespace Domain;

// Entity - has an id
public class Expense
{
    private readonly ExpenseType type;
    private readonly int amount;
    // this should have an id

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

    // Used Internally by domain
    public bool IsOverExpensedMeal() {
        bool dinnerOverExpensed = type == Domain.ExpenseType.DINNER && amount > 5000;
        bool breakfastOverExpensed = type == Domain.ExpenseType.BREAKFAST && amount > 1000;
        return dinnerOverExpensed || breakfastOverExpensed;
    }
    // Used Internally by domain
    public ExpenseType ExpenseTypes() {
        return type;
    }

    // Used Internally by domain
    public int Amount() {
        return amount;
    }
}
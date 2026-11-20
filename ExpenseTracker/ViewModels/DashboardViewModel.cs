namespace ExpenseTracker.ViewModels
{
    public class DashboardViewModel
    {
        public decimal TotalSpend { get; set; }

        public decimal MonthlySpend { get; set; }

        public decimal LastMonthlySpend { get; set; }

        public int TotalTransactions { get; set; }

        public List<MonthlyExpenseViewModel> MonthlyExpenses { get; set; } = [];
        public List<CategoryExpenseViewModel> CategoryExpenses { get; set; } = [];
    }

    public class MonthlyExpenseViewModel
    {
        public int Month { get; set; }
        public decimal Total { get; set; }
    }

    public class CategoryExpenseViewModel
    {
        public string Category { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }
}

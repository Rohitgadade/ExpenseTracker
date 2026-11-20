using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ExpenseTracker.Controllers
{
    public class HomeController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index()
        {
            //total expenses
            var totalSpend = _context.Expenses.Sum(e => e.Amount);

            //Total Spend this month
            var today = DateTime.Today;
            var totalSpendMonth = _context.Expenses.Where(e => e.Date.HasValue &&
            e.Date.Value.Month == today.Month &&
            e.Date.Value.Year == today.Year).Sum(e => e.Amount);

            //Total SpendLst month
            var lastMonth = today.AddMonths(-1);

            var totalSpendMonthLast = _context.Expenses
                .Where(e => e.Date.HasValue &&
                            e.Date.Value.Month == lastMonth.Month &&
                            e.Date.Value.Year == lastMonth.Year)
                .Sum(e => e.Amount);

            //Total transactions
            var totalTransactions = _context.Expenses.Count();

            // Get current year's monthly expenses
            var currentYear = DateTime.Today.Year;

            var monthlyData = _context.Expenses
                .Where(e => e.Date.HasValue &&
                            e.Date.Value.Year == currentYear)
                .GroupBy(e => e.Date!.Value.Month)
                .Select(g => new MonthlyExpenseViewModel
                {
                    Month = g.Key,
                    Total = g.Sum(e => e.Amount)
                })
                .ToList();

            // Add all 12 months, including months with zero expenses
            var monthlyExpenses = Enumerable.Range(1, 12)
                .Select(month => new MonthlyExpenseViewModel
                {
                    Month = month,
                    Total = monthlyData
                        .FirstOrDefault(x => x.Month == month)?.Total ?? 0
                })
                .ToList();

            var categoryExpenses = await _context.Categories
                .AsNoTracking()
                .Select(category => new CategoryExpenseViewModel
                {
                    Category = category.Name,
                    Total = category.Expenses.Sum(expense => (decimal?)expense.Amount) ?? 0
                })
                .ToListAsync();

            var totalSummary = new DashboardViewModel
            {
                TotalSpend = totalSpend,
                MonthlySpend = totalSpendMonth,
                LastMonthlySpend = totalSpendMonthLast,
                TotalTransactions = totalTransactions,
                MonthlyExpenses = monthlyExpenses,
                CategoryExpenses = categoryExpenses
            };

            return View(totalSummary);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

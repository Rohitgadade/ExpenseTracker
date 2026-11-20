using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    public class ExpenseController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index()
        {
            var expenses = await _context.Expenses
                .AsNoTracking()
                .Include(expense => expense.Category)
                .ToListAsync();

            return View(expenses);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new ExpenseViewModel
            {
                CategoriesList = await GetCategoriesAsync()
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Create(ExpenseViewModel model)
        {
            if (model.Date.HasValue && model.Date.Value.Date > DateTime.Today)
            {
                ModelState.AddModelError("Date", "Future dates are not allowed.");
            }

            if (!ModelState.IsValid)
            {
                model.CategoriesList = await GetCategoriesAsync();
                return View(model);
            }

            _context.Expenses.Add(new Expense
            {
                Title = model.Title,
                Amount = model.Amount,
                Date = model.Date,
                Description = model.Description,
                CategoryId = model.CategoryId
            });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            var model = new ExpenseViewModel
            {
                Id = expense.Id,
                Title = expense.Title,
                Amount = expense.Amount,
                Date = expense.Date,
                Description = expense.Description,
                CategoryId = expense.CategoryId,
                CategoriesList = await GetCategoriesAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ExpenseViewModel model)
        {
            if (model.Date.HasValue && model.Date.Value.Date > DateTime.Today)
            {
                ModelState.AddModelError("Date", "Future dates are not allowed.");
            }

            if (!ModelState.IsValid)
            {
                model.CategoriesList = await GetCategoriesAsync();
                return View(model);
            }

            var expense = await _context.Expenses.FindAsync(model.Id);
            if (expense == null)
            {
                return NotFound();
            }

            expense.Title = model.Title;
            expense.Amount = model.Amount;
            expense.Date = model.Date;
            expense.Description = model.Description;
            expense.CategoryId = model.CategoryId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<List<SelectListItem>> GetCategoriesAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .Select(category => new SelectListItem(category.Name, category.Id.ToString()))
                .ToListAsync();
        }

    }
}

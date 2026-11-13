using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    public class CategoryController(ApplicationDbContext context) : Controller
    {
        public readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index()
        {
            var category = await _context.Categories.AsNoTracking().ToListAsync();
            return View(category);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

              return  RedirectToAction("Index");
            }

            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return View(category);

        }

        [HttpPost]
        public async Task<IActionResult> Update(Category category)
        {

            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(int Id)
        {
            var category = await _context.Categories.FindAsync(Id);
           if(category == null)
            {
                return NotFound();
            }
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
        }

    }
}

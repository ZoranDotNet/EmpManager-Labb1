using EmpManager.Data;
using EmpManager.Models;
using EmpManager.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmpManager.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly AppDbContext _context;

        public StatisticsController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult SearchByName()
        {
            ViewData["FkEmployeeId"] = new SelectList(_context.Employees, "Id", "Fullname");
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> SearchByName(int id)
        {
            if (ModelState.IsValid)
            {
                var employee = await _context.Employees
               .Include(e => e.LeaveHistory)
               .FirstOrDefaultAsync(x => x.Id == id);

                if (employee == null)
                {
                    TempData["error"] = "Could not find Employee";
                    return RedirectToAction("Index");
                }
                if (employee.LeaveHistory.Count == 0)
                {
                    TempData["error"] = "Employee have no history";
                    return RedirectToAction(nameof(Index));
                }

                return View("SearchResultFromNameSearch", employee);
            }


            TempData["error"] = "Something went wrong";
            return View();
        }


        public IActionResult SearchByDate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SearchByDate(DateTime searchDate)
        {
            if (ModelState.IsValid)
            {
                var userinput = searchDate.Month;

                //only get employees who has Leave created in searchDate
                List<Employee> employee = await _context.Employees
                    .Where(e => e.LeaveHistory.Any(l => l.Created.Month == userinput))
                    .Include(e => e.LeaveHistory).ToListAsync();

                if (employee == null)
                {
                    TempData["error"] = "No employee found in search";
                    return NotFound();
                }
                var model = new SearchResultVM()
                {
                    SearchDate = searchDate,
                    Employees = employee
                };
                return View("SearchResult", model);
            }
            return View(searchDate);
        }

    }
}

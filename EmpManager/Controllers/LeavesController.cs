using EmpManager.Data;
using EmpManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmpManager.Controllers
{
    public class LeavesController : Controller
    {
        private readonly AppDbContext _context;

        public LeavesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Leaves
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Leaves.Include(l => l.Employee);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Leaves/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leave = await _context.Leaves
                .Include(l => l.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leave == null)
            {
                TempData["error"] = "Application not found";
                return NotFound();
            }

            return View(leave);
        }

        // GET: Leaves/Create
        public IActionResult Create()
        {
            ViewData["FkEmployeeId"] = new SelectList(_context.Employees, "Id", "Fullname");
            return View();
        }

        // POST: Leaves/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,StartDate,EndDate,FkEmployeeId")] Leave leave)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leave);
                await _context.SaveChangesAsync();
                TempData["success"] = "Application created successfully";
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkEmployeeId"] = new SelectList(_context.Employees, "Id", "Fullname", leave.FkEmployeeId);
            return View(leave);
        }

        // GET: Leaves/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leave = await _context.Leaves.FindAsync(id);
            if (leave == null)
            {
                TempData["error"] = "Application not found";
                return NotFound();
            }
            ViewData["FkEmployeeId"] = new SelectList(_context.Employees, "Id", "Fullname", leave.FkEmployeeId);
            return View(leave);
        }

        // POST: Leaves/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,StartDate,EndDate, Handled, IsApproved, FkEmployeeId")] Leave leave)
        {
            if (id != leave.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leave);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveExists(leave.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["success"] = "Application updated";
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkEmployeeId"] = new SelectList(_context.Employees, "Id", "Fullname", leave.FkEmployeeId);
            return View(leave);
        }

        // GET: Leaves/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leave = await _context.Leaves
                .Include(l => l.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leave == null)
            {
                TempData["error"] = "Application not found";
                return NotFound();
            }

            return View(leave);
        }

        // POST: Leaves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leave = await _context.Leaves.FindAsync(id);
            if (leave != null)
            {
                _context.Leaves.Remove(leave);
            }

            await _context.SaveChangesAsync();
            TempData["success"] = "Application deleted";
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveExists(int id)
        {
            return _context.Leaves.Any(e => e.Id == id);
        }


        public async Task<IActionResult> ViewApplication()
        {
            var leaves = await _context.Leaves.Include(l => l.Employee).ToListAsync();
            if (leaves.Count == 0)
            {
                TempData["error"] = "No Data was found";
                return View();
            }
            return View(leaves);
        }

        public async Task<IActionResult> Approve(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var leave = await _context.Leaves.FirstOrDefaultAsync(x => x.Id == id);
            if (leave == null)
            {
                return NotFound();
            }
            leave.IsApproved = true;
            leave.Handled = true;

            _context.Leaves.Update(leave);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Deny(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var leave = await _context.Leaves.FirstOrDefaultAsync(x => x.Id == id);
            if (leave == null)
            {
                return NotFound();
            }

            leave.Handled = true;

            _context.Leaves.Update(leave);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

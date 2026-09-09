using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sports_Facility.Data;
using Sports_Facility.Models;

namespace Sports_Facility.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // READ 
        public async Task<IActionResult> Index(string? search)
        {
            var query = _context.Facilities
                .Include(f => f.Sport)
                .Include(f => f.Location)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(f => f.FacilityName.Contains(search));
            }

            var facilities = await query.ToListAsync();
            ViewBag.Search = search;
            return View(facilities);
        }

        // CREATE -  form
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Sports = await _context.Sports.ToListAsync();
            ViewBag.Locations = await _context.Locations.ToListAsync();
            return View(new Facility { FacilityName = string.Empty });
        }

        // CREATE - save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Facility facility)
        {
            _context.Facilities.Add(facility);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // UPDATE - show filled form
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null) return NotFound();

            ViewBag.Sports = await _context.Sports.ToListAsync();
            ViewBag.Locations = await _context.Locations.ToListAsync();
            return View(facility);
        }

        // UPDATE - save changes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Facility facility)
        {
            _context.Facilities.Update(facility);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // DELETE - show confirmation
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var facility = await _context.Facilities
                .Include(f => f.Sport)
                .Include(f => f.Location)
                .FirstOrDefaultAsync(f => f.FacilityId == id);
            if (facility == null) return NotFound();
            return View(facility);
        }

        // DELETE - remove it
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var facility = await _context.Facilities.FindAsync(id);
            if (facility != null)
            {
                _context.Facilities.Remove(facility);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
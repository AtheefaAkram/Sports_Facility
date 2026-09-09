using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sports_Facility.Data;

namespace Sports_Facility.Controllers
{
    public class FacilityController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FacilityController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var facilities = await _context.Facilities
                .Include(f => f.Location)
                .Include(f => f.Sport)
                .ToListAsync();

            return View(facilities);
        }
    }
}
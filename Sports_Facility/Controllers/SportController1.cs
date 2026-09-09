using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sports_Facility.Data;

namespace Sports_Facility.Controllers
{
    public class SportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SportController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sports = await _context.Sports.ToListAsync();
            return View(sports);
        }
    }
}
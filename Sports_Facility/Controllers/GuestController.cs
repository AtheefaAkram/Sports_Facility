using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sports_Facility.Data;
using Sports_Facility.Models;

namespace Sports_Facility.Controllers
{
    public class GuestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GuestController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ----- Log in as Guest -----
        [HttpGet]
        public IActionResult GuestLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuestLogin(string guestName, string guestEmail)
        {
            var guest = await _context.Guests.FirstOrDefaultAsync(g => g.GuestEmail == guestEmail);

            if (guest == null)
            {
                guest = new Guest { GuestName = guestName, GuestEmail = guestEmail };
                _context.Guests.Add(guest);
                await _context.SaveChangesAsync();
            }

            HttpContext.Session.SetInt32("GuestId", guest.GuestId);
            HttpContext.Session.SetString("GuestName", guest.GuestName);
            HttpContext.Session.SetString("GuestEmail", guest.GuestEmail);

            return RedirectToAction("Dashboard");
        }

        // ----- Guest landing page -----
        public async Task<IActionResult> Dashboard(FacilitySearchViewModel filters)
        {
            if (HttpContext.Session.GetInt32("GuestId") == null)
                return RedirectToAction("GuestLogin");

            var query = _context.Facilities
                .Include(f => f.Sport)
                .Include(f => f.Location)
                .AsQueryable();

            if (filters.SportId.HasValue)
                query = query.Where(f => f.SportId == filters.SportId.Value);

            if (filters.LocationId.HasValue)
                query = query.Where(f => f.LocationId == filters.LocationId.Value);

            filters.Results = await query.ToListAsync();
            filters.Sports = await _context.Sports.ToListAsync();
            filters.Locations = await _context.Locations.ToListAsync();

            ViewBag.Reviews = await _context.Reviews
                .Include(r => r.Booking)
                    .ThenInclude(b => b!.Facility)
                .ToListAsync();

            return View(filters);
        }

        // ----- Become a Member (from the Guest dashboard) -----
        public IActionResult BecomeMember()
        {
            TempData["PrefillName"] = HttpContext.Session.GetString("GuestName");
            TempData["PrefillEmail"] = HttpContext.Session.GetString("GuestEmail");

            return RedirectToAction("Register", "Account");
        }

        // ----- Send Inquiry -----
        [HttpGet]
        public IActionResult SendInquiry()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendInquiry(string guestName, string guestEmail, string message)
        {
            var guest = await _context.Guests.FirstOrDefaultAsync(g => g.GuestEmail == guestEmail);

            if (guest == null)
            {
                guest = new Guest { GuestName = guestName, GuestEmail = guestEmail };
                _context.Guests.Add(guest);
                await _context.SaveChangesAsync();
            }

            _context.Inquiries.Add(new Inquiry
            {
                GuestId = guest.GuestId,
                Message = message
            });
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }
    }
}
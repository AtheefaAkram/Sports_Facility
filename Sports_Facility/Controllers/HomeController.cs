using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sports_Facility.Data;
using Sports_Facility.Models;

namespace Sports_Facility.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(FacilitySearchViewModel filters)
        {
            if (HttpContext.Session.GetInt32("MemberId") == null)
            {
                return View();
            }

            var query = _context.Facilities
                .Include(f => f.Sport)
                .Include(f => f.Location)
                .AsQueryable();

            if (filters.SportId.HasValue)
                query = query.Where(f => f.SportId == filters.SportId.Value);

            if (filters.LocationId.HasValue)
                query = query.Where(f => f.LocationId == filters.LocationId.Value);

            var facilities = await query.ToListAsync();

            if (filters.Date.HasValue && !string.IsNullOrEmpty(filters.Time))
            {
                var start = filters.Date.Value.Date.Add(TimeSpan.Parse(filters.Time));
                var end = start.AddHours(1);

                var busyFacilityIds = await _context.Bookings
                    .Where(b => b.BookingDate.Date == filters.Date.Value.Date
                             && (b.BookingStatus == "Pending" || b.BookingStatus == "Confirmed")
                             && start < b.EndTime && end > b.StartTime)
                    .Select(b => b.FacilityId)
                    .ToListAsync();

                facilities = facilities.Where(f => !busyFacilityIds.Contains(f.FacilityId)).ToList();
            }

            filters.Sports = await _context.Sports.ToListAsync();
            filters.Locations = await _context.Locations.ToListAsync();
            filters.Results = facilities;

            return View("MemberHome", filters);
        }
    }
}
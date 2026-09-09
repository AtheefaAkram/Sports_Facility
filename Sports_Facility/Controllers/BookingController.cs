using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sports_Facility.Data;
using Sports_Facility.Models;

namespace Sports_Facility.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int facilityId)
        {
            if (HttpContext.Session.GetInt32("MemberId") == null)
                return RedirectToAction("Login", "Account");

            var facility = await _context.Facilities.FindAsync(facilityId);
            if (facility == null) return NotFound();

            var model = new BookingCreateViewModel
            {
                FacilityId = facility.FacilityId,
                FacilityName = facility.FacilityName,
                BookingDate = DateTime.Today
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingCreateViewModel model)
        {
            var memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId == null) return RedirectToAction("Login", "Account");

            var facility = await _context.Facilities.FindAsync(model.FacilityId);
            if (facility == null) return NotFound();

            var start = model.BookingDate.Date.Add(TimeSpan.Parse(model.StartTime));
            var end = model.BookingDate.Date.Add(TimeSpan.Parse(model.EndTime));

            if (end <= start)
            {
                ModelState.AddModelError(string.Empty, "End time must be after start time.");
                model.FacilityName = facility.FacilityName;
                return View(model);
            }

            var clash = await _context.Bookings.AnyAsync(b =>
                b.FacilityId == model.FacilityId &&
                b.BookingDate.Date == model.BookingDate.Date &&
                (b.BookingStatus == "Pending" || b.BookingStatus == "Confirmed") &&
                start < b.EndTime && end > b.StartTime);

            if (clash)
            {
                ModelState.AddModelError(string.Empty, "This facility is already booked for that time. Please choose a different slot.");
                model.FacilityName = facility.FacilityName;
                return View(model);
            }

            var booking = new Booking
            {
                MemberId = memberId.Value,
                FacilityId = model.FacilityId,
                BookingDate = model.BookingDate.Date,
                StartTime = start,
                EndTime = end,
                BookingStatus = "Pending"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Booking request submitted.";
            return RedirectToAction("Create", "Payment", new { bookingId = booking.BookingId });
        }

        public async Task<IActionResult> MyBookings()
        {
            var memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId == null) return RedirectToAction("Login", "Account");

            var bookings = await _context.Bookings
                .Include(b => b.Facility)
                .Where(b => b.MemberId == memberId.Value)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return View(bookings);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sports_Facility.Data;
using Sports_Facility.Models;

namespace Sports_Facility.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int bookingId)
        {
            var memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId == null) return RedirectToAction("Login", "Account");

            var booking = await _context.Bookings
                .Include(b => b.Facility)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId && b.MemberId == memberId.Value);

            if (booking == null) return NotFound();

            var model = new ReviewCreateViewModel
            {
                BookingId = booking.BookingId,
                FacilitySummary = $"{booking.Facility?.FacilityName} on {booking.BookingDate:d}"
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewCreateViewModel model)
        {
            var memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId == null) return RedirectToAction("Login", "Account");

            var booking = await _context.Bookings
                .Include(b => b.Facility)
                .FirstOrDefaultAsync(b => b.BookingId == model.BookingId && b.MemberId == memberId.Value);

            if (booking == null) return NotFound();

            if (!ModelState.IsValid)
            {
                model.FacilitySummary = $"{booking.Facility?.FacilityName} on {booking.BookingDate:d}";
                return View(model);
            }

            _context.Reviews.Add(new Review
            {
                BookingId = model.BookingId,
                Rating = model.Rating,
                Comment = model.Comment
            });
            await _context.SaveChangesAsync();

            TempData["Success"] = "Thank you for your review.";
            return RedirectToAction("MyBookings", "Booking");
        }
    }
}
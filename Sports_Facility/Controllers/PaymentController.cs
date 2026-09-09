using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sports_Facility.Data;
using Sports_Facility.Models;

namespace Sports_Facility.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
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

            if (booking == null || booking.Facility == null) return NotFound();

            var hours = (decimal)(booking.EndTime - booking.StartTime).TotalHours;
            var amount = Math.Round(booking.Facility.HourlyRate * hours, 2);

            var model = new PaymentCreateViewModel
            {
                BookingId = booking.BookingId,
                FacilityName = booking.Facility.FacilityName,
                Amount = amount
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentCreateViewModel model)
        {
            var memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId == null) return RedirectToAction("Login", "Account");

            var booking = await _context.Bookings
                .Include(b => b.Facility)
                .FirstOrDefaultAsync(b => b.BookingId == model.BookingId && b.MemberId == memberId.Value);

            if (booking == null || booking.Facility == null) return NotFound();

            var hours = (decimal)(booking.EndTime - booking.StartTime).TotalHours;
            var amount = Math.Round(booking.Facility.HourlyRate * hours, 2);

            if (!ModelState.IsValid)
            {
                model.FacilityName = booking.Facility.FacilityName;
                model.Amount = amount;
                return View(model);
            }

            _context.Payments.Add(new Payment
            {
                BookingId = booking.BookingId,
                Amount = amount,
                PaymentMethod = model.PaymentMethod,
                PaymentStatus = "Paid",
                PayDate = DateTime.Now
            });

            booking.BookingStatus = "Confirmed";
            await _context.SaveChangesAsync();

            TempData["Success"] = "Payment successful. Your booking is now confirmed.";
            return RedirectToAction("MyBookings", "Booking");
        }
    }
}
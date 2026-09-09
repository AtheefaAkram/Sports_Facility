using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sports_Facility.Data;
using Sports_Facility.Models;

namespace Sports_Facility.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var model = new RegisterViewModel
            {
                AvailableSports = await _context.Sports.ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableSports = await _context.Sports.ToListAsync();
                return View(model);
            }

            bool emailExists = await _context.Members.AnyAsync(m => m.Email == model.Email);
            if (emailExists)
            {
                ModelState.AddModelError(nameof(model.Email), "An account with this email already exists.");
                model.AvailableSports = await _context.Sports.ToListAsync();
                return View(model);
            }

            var member = new Member
            {
                MemberName = model.MemberName,
                Email = model.Email,
                PhoneNo = model.PhoneNo,
                Address = model.Address,
                PasswordHash = PasswordHelper.Hash(model.Password)
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            foreach (var sportId in model.PreferredSportIds)
            {
                _context.MemberSports.Add(new MemberSport
                {
                    MemberId = member.MemberId,
                    SportId = sportId
                });
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var member = await _context.Members.FirstOrDefaultAsync(m => m.Email == email);

            if (member == null || !PasswordHelper.Verify(password, member.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View();
            }

            HttpContext.Session.SetInt32("MemberId", member.MemberId);
            HttpContext.Session.SetString("MemberName", member.MemberName);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}


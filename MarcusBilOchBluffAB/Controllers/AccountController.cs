using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarcusBilOchBluffAB.Data;
using MarcusBilOchBluffAB.Models;

namespace MarcusBilOchBluffAB.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string returnUrl = null)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == email && c.Password == password);

            if (customer != null)
            {
                // Spara inloggad användare i session
                HttpContext.Session.SetInt32("CustomerId", customer.Id);
                HttpContext.Session.SetString("CustomerEmail", customer.Email);

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Fel e-post eller lösenord";
            return View();
        }


        public async Task<IActionResult> Profile()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login");
            }

            var bookings = await _context.Bookings
                .Include(b => b.Car)
                .Where(b => b.CustomerId == customerId)
                .ToListAsync();

            var upcomingBookings = bookings.Where(b => b.StartDate >= DateTime.Today).ToList();
            var pastBookings = bookings.Where(b => b.EndDate < DateTime.Today).ToList();

            var model = new ProfileViewModel
            {
                UpcomingBookings = upcomingBookings,
                PastBookings = pastBookings
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);

            if (booking == null || booking.StartDate < DateTime.Today)
            {
                return RedirectToAction("Profile");
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction("Profile");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }


    }
}

using MarcusBilOchBluffAB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarcusBilOchBluffAB.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MarcusBilOchBluffAB.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        [HttpGet("")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("AdminEmail") == null)
            {
                return RedirectToAction("Login");
            }

            // Om inloggad, gå till Dashboard
            return RedirectToAction("Dashboard");
        }

        // Login-sidan som kan nås utan att vara inloggad
        [HttpGet("login")]
        public IActionResult Login()
        {
            // Om användaren redan är inloggad, omdirigera till Dashboard
            if (HttpContext.Session.GetString("AdminEmail") != null)
            {
                return RedirectToAction("Dashboard");
            }

            return View();
        }

        // Hanterar inloggningen av admin
        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var admin = await _unitOfWork.AdminRepository.AuthenticateAsync(email, password);
            if (admin != null)
            {
                HttpContext.Session.SetString("AdminEmail", admin.Email);
                Console.WriteLine("Admin logged in successfully."); // Lägg till detta för att debugga
                return RedirectToAction("Dashboard");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }

        
        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var adminEmail = HttpContext.Session.GetString("AdminEmail");
            if (string.IsNullOrEmpty(adminEmail))
            {
                return RedirectToAction("Login");
            }

            var dashboardData = new AdminDashboardViewModel
            {
                Cars = await _unitOfWork.Cars.GetAllAsync(),
                Bookings = await _unitOfWork.Bookings.GetAllAsync(),
                Customers = await _unitOfWork.Customers.GetAllAsync()
            };

            return View(dashboardData);
        }


        
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();  // Töm sessionen vid utloggning
            return RedirectToAction("Login");
        }

        
        [HttpGet("manage-cars")]
        public async Task<IActionResult> ManageCars()
        {
            if (HttpContext.Session.GetString("AdminEmail") == null)
            {
                return RedirectToAction("Login");
            }

            var cars = await _unitOfWork.Cars.GetAllAsync();
            return View(cars);
        }

        [HttpGet("manage-bookings")]
        public async Task<IActionResult> ManageBookings()
        {
            if (HttpContext.Session.GetString("AdminEmail") == null)
            {
                return RedirectToAction("Login");
            }

            // Hämta bokningar med relaterade entiteter
            var bookings = await _unitOfWork.GetBookingsWithRelatedEntitiesAsync();

            var bookingViewModels = bookings.Select(b => new BookingViewModel
            {
                Id= b.Id,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                CarId = b.CarId,
                CustomerId = b.CustomerId,
                
            }).ToList();

            return View(bookingViewModels);
        }

        
        [HttpGet("manage-customers")]
        public async Task<IActionResult> ManageCustomers()
        {
            if (HttpContext.Session.GetString("AdminEmail") == null)
            {
                return RedirectToAction("Login");
            }

            var customers = await _unitOfWork.Customers.GetAllAsync();
            return View(customers);
        }


    }
}

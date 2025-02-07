using MarcusBilOchBluffAB.Data;
using MarcusBilOchBluffAB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MarcusBilOchBluffAB.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HomeController> _logger;

        // Konstruktor för att injicera UnitOfWork och Logger
        public HomeController(IUnitOfWork unitOfWork, ILogger<HomeController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // Index-metod som hämtar alla bilar och skickar till vyn
        public async Task<IActionResult> Index()
        {
            var userEmail = HttpContext.Session.GetString("CustomerEmail");
            ViewData["UserEmail"] = userEmail;  // Lägg till detta

            var cars = await _unitOfWork.Cars.GetAllAsync();

            if (cars == null || !cars.Any())
            {
                _logger.LogWarning("No cars found in database.");
            }
            else
            {
                _logger.LogInformation($"Found {cars.Count()} cars in database.");
            }

            return View(cars);
        }

        // Booking-metod som visar bokningssidan för vald bil
        public async Task<IActionResult> Booking(int carId)
        {
            var car = await _unitOfWork.Cars.GetByIdAsync(carId);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);  // Skickar den valda bilen till booking.cshtml
        }

        // Post-metod för att skapa en bokning
        [HttpPost]
        public async Task<IActionResult> BookCar(int carId, DateTime startDate, DateTime endDate)
        {
            // Kontrollera om användaren är inloggad
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
            {
                // Om ej inloggad skicka till inloggning och sedan tillbaka till bokningen
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("BookCar", "Home", new { carId }) });
            }

            // Hämta kundinformation
            var customer = await _unitOfWork.Customers.GetByIdAsync(customerId.Value);
            if (customer == null)
            {
                _logger.LogWarning("Invalid customer session.");
                return View("Error", new ErrorViewModel { RequestId = "Invalid session." });
            }

            // Skapa bokningen
            var booking = new Booking
            {
                CarId = carId,
                CustomerId = customer.Id,
                StartDate = startDate,
                EndDate = endDate,
                
            };

            await _unitOfWork.Bookings.AddAsync(booking);
            

            return RedirectToAction("BookingConfirmation");
        }



        // Bekräftelsesida efter bokning
        public IActionResult BookingConfirmation()
        {
            return View();
        }

       

    }
}

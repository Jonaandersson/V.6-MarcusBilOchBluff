using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarcusBilOchBluffAB.Data;
using MarcusBilOchBluffAB.Models;
using System.Runtime.ConstrainedExecution;

namespace MarcusBilOchBluffAB.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        // GET: Booking/Create
        [HttpGet]
        public async Task<IActionResult> CreateBooking()
        {
            if (HttpContext.Session.GetString("AdminEmail") == null)
            {
                return RedirectToAction("Login");
            }

            var bookingViewModel = new BookingViewModel
            {
                Cars = await _unitOfWork.Cars.GetAllAsync(),
                Customers = await _unitOfWork.Customers.GetAllAsync()
            };

            return View(bookingViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBooking(BookingViewModel bookingViewModel)
        {
            if (!ModelState.IsValid)
            {
                
                bookingViewModel.Cars = await _unitOfWork.Cars.GetAllAsync();
                bookingViewModel.Customers = await _unitOfWork.Customers.GetAllAsync();
                return View(bookingViewModel);
            }

            var booking = new Booking
            {
                StartDate = bookingViewModel.StartDate,
                EndDate = bookingViewModel.EndDate,
                CarId = bookingViewModel.CarId,
                CustomerId = bookingViewModel.CustomerId,
                IsConfirmed = bookingViewModel.IsConfirmed
            };

            await _unitOfWork.Bookings.AddAsync(booking);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("ManageBookings", "Admin");
        }


        
        [HttpGet]
        public async Task<IActionResult> EditBooking(int id)
        {
            if (id == 0)
            {
                return BadRequest("Booking ID is missing or invalid.");
            }

            var booking = await _unitOfWork.Bookings.GetByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }


            var bookingViewModel = new BookingViewModel
            {
                Id = booking.Id,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                CarId = booking.CarId,
                CustomerId = booking.CustomerId,
                IsConfirmed = booking.IsConfirmed
            };

            // Kontrollera att Id är giltigt innan jag hämtar bokningen
            if (bookingViewModel.Id == 0)
            {
                return BadRequest("Booking ID is required and must be greater than 0.");
            }

            bookingViewModel.Cars = await _unitOfWork.Cars.GetAllAsync(); 
            bookingViewModel.Customers = await _unitOfWork.Customers.GetAllAsync(); 

            return View(bookingViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> EditBooking(int id, BookingViewModel bookingViewModel)
        {
            // Debug för att kontrollera värdena i bookingViewModel
            Console.WriteLine($"Id: {bookingViewModel.Id}, StartDate: {bookingViewModel.StartDate}, EndDate: {bookingViewModel.EndDate}, CarId: {bookingViewModel.CarId}, CustomerId: {bookingViewModel.CustomerId}");


            if (!ModelState.IsValid)
            {
                // Skicka med listor på bilar och kunder ifall användaren måste rätta sina inmatningar
                bookingViewModel.Cars = await _unitOfWork.Cars.GetAllAsync();
                bookingViewModel.Customers = await _unitOfWork.Customers.GetAllAsync();
                return View(bookingViewModel);
            }

            

            var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingViewModel.Id);
            if (booking == null)
            {
                return NotFound();
            }

            
            booking.StartDate = bookingViewModel.StartDate;
            booking.EndDate = bookingViewModel.EndDate;
            booking.CarId = bookingViewModel.CarId;
            booking.CustomerId = bookingViewModel.CustomerId;
            booking.IsConfirmed = bookingViewModel.IsConfirmed;

            
            await _unitOfWork.Bookings.UpdateAsync(booking);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("ManageBookings", "Admin");
        }

        
        [HttpGet("admin/delete-booking/{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            if (HttpContext.Session.GetString("AdminEmail") == null)
            {
                return RedirectToAction("Login");
            }

            var booking = await _unitOfWork.Bookings.GetByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            
            var bookingViewModel = new BookingViewModel
            {
                Id = booking.Id,
                CarId = booking.CarId,
                CustomerId = booking.CustomerId,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                IsConfirmed = booking.IsConfirmed
            };

            return View(bookingViewModel);
        }


        
        [HttpPost("admin/delete-booking/{id}")]
        public async Task<IActionResult> DeleteBookingConfirmed(int id)
        {
            if (HttpContext.Session.GetString("AdminEmail") == null)
            {
                return RedirectToAction("Login");
            }

            var booking = await _unitOfWork.Bookings.GetByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            
            await _unitOfWork.Bookings.DeleteAsync(id);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("ManageBookings", "Admin");
        }


    }
}



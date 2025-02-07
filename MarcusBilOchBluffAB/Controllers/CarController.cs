using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarcusBilOchBluffAB.Data;
using MarcusBilOchBluffAB.Models;

namespace MarcusBilOchBluffAB.Controllers
{
    public class CarController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CarController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        // GET: Admin/CreateCar
        [HttpGet("/Admin/CreateCar")]
        public IActionResult CreateCar()
        {
            if (HttpContext.Session.GetString("AdminEmail") == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        // POST: Admin/CreateCar
        [HttpPost("/Admin/CreateCar")]
        public async Task<IActionResult> CreateCar([Bind("Make,Model,PricePerDay,ImagePath")] CarViewModel carViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(carViewModel);
            }

            var car = new Car
            {
                Make = carViewModel.Make,
                Model = carViewModel.Model,
                PricePerDay = carViewModel.PricePerDay,
                ImagePath = carViewModel.ImagePath,
                Bookings = new List<Booking>()
            };

            await _unitOfWork.Cars.AddAsync(car);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("ManageCars", "Admin");
        }

        // GET: Admin/EditCar/{id}
        [HttpGet("Admin/EditCar/{id}")]
        public async Task<IActionResult> EditCar(int id)
        {
            var car = await _unitOfWork.Cars.GetByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            var carViewModel = new CarViewModel
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                PricePerDay = car.PricePerDay,
                ImagePath = car.ImagePath
            };

            return View(carViewModel);
        }

        // POST: Admin/EditCar/{id}
        [HttpPost("Admin/EditCar/{id}")]
        public async Task<IActionResult> EditCar(CarViewModel carViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(carViewModel);
            }

            var car = await _unitOfWork.Cars.GetByIdAsync(carViewModel.Id.Value);
            if (car == null)
            {
                return NotFound();
            }

            // Uppdatera bilens egenskaper
            car.Make = carViewModel.Make;
            car.Model = carViewModel.Model;
            car.PricePerDay = carViewModel.PricePerDay;
            car.ImagePath = carViewModel.ImagePath;

            await _unitOfWork.Cars.UpdateAsync(car);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("ManageCars", "Admin");
        }

        // GET: Admin/DeleteCar/{id}
        [HttpGet("Admin/DeleteCar/{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            if (HttpContext.Session.GetString("AdminEmail") == null)
            {
                return RedirectToAction("Login");
            }

            var car = await _unitOfWork.Cars.GetByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            var carViewModel = new CarViewModel
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                PricePerDay = car.PricePerDay,
                ImagePath = car.ImagePath
            };

            return View(carViewModel);
        }

        // POST: Admin/DeleteCar/{id}
        [HttpPost("Admin/DeleteCar/{id}")]
        public async Task<IActionResult> DeleteCarConfirmed(int id)
        {
            if (HttpContext.Session.GetString("AdminEmail") == null)
            {
                return RedirectToAction("Login");
            }

            var car = await _unitOfWork.Cars.GetByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            await _unitOfWork.Cars.DeleteAsync(id);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("ManageCars","Admin");
        }

    }

}

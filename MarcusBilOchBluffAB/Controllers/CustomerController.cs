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
    public class CustomerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Admin/CreateCustomer
        [HttpGet("Admin/CreateCustomer")]
        public IActionResult CreateCustomer()
        {
            if (HttpContext.Session.GetString("AdminEmail") == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        // POST: Admin/CreateCustomer
        [HttpPost("Admin/CreateCustomer")]
        public async Task<IActionResult> CreateCustomer(CustomerViewModel customerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(customerViewModel);
            }

            var customer = new Customer
            {
                Email = customerViewModel.Email,
                Password = customerViewModel.Password,
                
            };

            await _unitOfWork.Customers.AddAsync(customer);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("ManageCustomers", "Admin");
        }

        // GET: Admin/EditCustomer/{id}
        [HttpGet("admin/edit-customer/{id}")]
        public async Task<IActionResult> EditCustomer(int id)
        {
            if (HttpContext.Session.GetString("AdminEmail") == null)
            {
                return RedirectToAction("Login");
            }

            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var customerViewModel = new CustomerViewModel
            {
                Id = customer.Id,
                Email = customer.Email,
                Password = customer.Password
            };

            return View(customerViewModel);
        }

        // POST: Admin/EditCustomer/{id}
        [HttpPost("admin/edit-customer/{id}")]
        public async Task<IActionResult> EditCustomer(CustomerViewModel customerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(customerViewModel);
            }

            var customer = await _unitOfWork.Customers.GetByIdAsync(customerViewModel.Id);
            if (customer == null)
            {
                return NotFound();
            }

            // Uppdatera kundens egenskaper
            customer.Email = customerViewModel.Email;
            customer.Password = customerViewModel.Password;

            await _unitOfWork.Customers.UpdateAsync(customer);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("ManageCustomers", "Admin");
        }

        // GET: Admin/DeleteCustomer/{id}
        [HttpGet("admin/delete-customer/{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            if (HttpContext.Session.GetString("AdminEmail") == null)
            {
                return RedirectToAction("Login");
            }

            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var customerViewModel = new CustomerViewModel
            {
                Id = customer.Id,
                Email = customer.Email,
                Password = customer.Password
            };

            return View(customerViewModel);
        }

        // POST: Admin/DeleteCustomer/{id}
        [HttpPost("admin/delete-customer/{id}")]
        public async Task<IActionResult> DeleteCustomerConfirmed(int id)
        {
            if (HttpContext.Session.GetString("AdminEmail") == null)
            {
                return RedirectToAction("Login");
            }

            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            await _unitOfWork.Customers.DeleteAsync(id);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("ManageCustomers", "Admin");
        }

        
    }

}

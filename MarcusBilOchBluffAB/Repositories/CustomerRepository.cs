using MarcusBilOchBluffAB.Data;
using MarcusBilOchBluffAB.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarcusBilOchBluffAB.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Denna metod används nu direkt från Repository<T>
        public async Task<IEnumerable<Customer>> GetAllAsync(string? includeProperties = null)
        {
            return await base.GetAllAsync(includeProperties); // Anropar basklassens GetAllAsync
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

        // Denna metod är specifik för CustomerRepository och kan behållas
        public async Task<Customer> GetCustomersByEmailAsync(string email)
        {
            return await _context.Customers
                                 .FirstOrDefaultAsync(c => c.Email == email);
        }
    }
}

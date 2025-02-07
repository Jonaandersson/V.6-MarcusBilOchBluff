using MarcusBilOchBluffAB.Data;
using MarcusBilOchBluffAB.Models;
using MarcusBilOchBluffAB.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MarcusBilOchBluffAB
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IRepository<Car> _cars;
        private IRepository<Booking> _bookings;
        private IAdminRepository _admins;
        private ICustomerRepository _customers;



        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<Car> Cars => _cars ??= new Repository<Car>(_context);
        public IRepository<Booking> Bookings => _bookings ??= new Repository<Booking>(_context);
        public ICustomerRepository Customers => _customers ??= new CustomerRepository(_context);
        public IAdminRepository Admins => _admins ??= new AdminRepository(_context);

        private IAdminRepository _adminRepository;



        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IAdminRepository AdminRepository
        {
            get
            {
                if (_adminRepository == null)
                {
                    _adminRepository = new AdminRepository(_context);
                }
                return _adminRepository;
            }
        }

        public async Task<IEnumerable<Booking>> GetBookingsWithRelatedEntitiesAsync()
        {
            return await _context.Bookings
                .Include(b => b.Car)        // Inkluderar relaterade Car-objekt
                .Include(b => b.Customer)   // Inkluderar relaterade Customer-objekt
                .ToListAsync();             // Hämtar resultaten som en lista
        }

    }
}

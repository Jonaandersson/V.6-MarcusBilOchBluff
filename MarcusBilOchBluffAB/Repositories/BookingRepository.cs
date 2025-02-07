using MarcusBilOchBluffAB.Data;
using MarcusBilOchBluffAB.Models;
using Microsoft.EntityFrameworkCore;

namespace MarcusBilOchBluffAB.Repositories
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Booking>> GetAllWithDetailsAsync()
        {
            return await _context.Bookings
                .Include(b => b.Car)         // Inkludera relaterade Car
                .Include(b => b.Customer)    // Inkludera relaterade Customer
                .ToListAsync();
        }
    }
}
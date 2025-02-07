using MarcusBilOchBluffAB.Models;

namespace MarcusBilOchBluffAB.Repositories
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<List<Booking>> GetAllWithDetailsAsync(); // Lägg till denna metod
    }


}

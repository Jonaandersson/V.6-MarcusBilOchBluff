using MarcusBilOchBluffAB.Models;
using MarcusBilOchBluffAB.Repositories;
using System.Threading.Tasks;

namespace MarcusBilOchBluffAB.Data
{
    public interface IUnitOfWork
    {
        IRepository<Car> Cars { get; }
        IRepository<Booking> Bookings { get; }
        IAdminRepository Admins { get; }
        ICustomerRepository Customers { get; }

        IAdminRepository AdminRepository { get; }


        Task<IEnumerable<Booking>> GetBookingsWithRelatedEntitiesAsync();

        Task SaveAsync();
    }
}

using MarcusBilOchBluffAB.Data;
using MarcusBilOchBluffAB.Models;
using Microsoft.EntityFrameworkCore;

namespace MarcusBilOchBluffAB.Repositories
{
    namespace MarcusBilOchBluffAB.Repositories
    {
        public class CarRepository : Repository<Car>, IRepository<Car>
        {
            public CarRepository(ApplicationDbContext context) : base(context)
            {
            }
        }
    }
}

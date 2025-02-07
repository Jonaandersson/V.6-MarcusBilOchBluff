using MarcusBilOchBluffAB.Models;
using System.Linq.Expressions;

namespace MarcusBilOchBluffAB.Repositories
{
    public interface IAdminRepository : IRepository<Admin>
    {
        Task<Admin?> FindAsync(Expression<Func<Admin, bool>> predicate);

        Task<Admin> AuthenticateAsync(string email, string password);

    }
}

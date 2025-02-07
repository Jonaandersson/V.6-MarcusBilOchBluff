using MarcusBilOchBluffAB.Data;
using MarcusBilOchBluffAB.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MarcusBilOchBluffAB.Repositories
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        public AdminRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Admin?> FindAsync(Expression<Func<Admin, bool>> predicate)
        {
            return await _context.Admins.FirstOrDefaultAsync(predicate);
        }

        public async Task<Admin> AuthenticateAsync(string email, string password)
        {
            return await _context.Admins
                .FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
        }

    }
}

using MarcusBilOchBluffAB.Models;

namespace MarcusBilOchBluffAB.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null);
        

        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);

        Task<IEnumerable<T>> GetAllWithDetailsAsync();

    }

}

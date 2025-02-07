using MarcusBilOchBluffAB.Models;
using MarcusBilOchBluffAB.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer> GetCustomersByEmailAsync(string email);
}


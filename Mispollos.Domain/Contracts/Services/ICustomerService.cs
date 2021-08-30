using Mispollos.Domain.Entities;
using Mispollos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mispollos.Domain.Contracts.Services
{
    public interface ICustomerService
    {
        Task<List<Cliente>> GetCustomers();

        Task<Cliente> GetCustomerById(Guid id);

        Task<PagedResult<Cliente>> GetCustomersPaged(int page, string search);

        Task<Cliente> CreateCustomer(Cliente customer);

        Task UpdateCustomer(Cliente customer);

        Task DeleteCustomer(Guid id);
    }
}
using Mispollos.Domain.Contracts.Repositories;
using Mispollos.Domain.Contracts.Services;
using Mispollos.Domain.Entities;
using Mispollos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mispollos.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IAsyncRepository<Cliente> _customerRepository;

        public CustomerService(IAsyncRepository<Cliente> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<Cliente>> GetCustomers()

        {
            return (await _customerRepository.ListAllAsync()).OrderBy(x => x.UpdatedOn).ToList();
        }

        public async Task<PagedResult<Cliente>> GetCustomersPaged(int page, string search)
        {
            var result = new PagedResult<Cliente>();
            if (!string.IsNullOrEmpty(search))
            {
                result.Data = _customerRepository
                    .Query(x => x.Nombre.Contains(search))
                    .OrderByDescending(x => x.UpdatedOn)
                    .Skip((page - 1) * 10)
                    .Take(10)
                    .ToList();

                result.Total = await _customerRepository.CountByQuery(x => x.Nombre.Contains(search));
            }
            else
            {
                result.Data = (await _customerRepository
                    .ListAllAsync())
                    .OrderByDescending(x => x.UpdatedOn)
                    .Skip((page - 1) * 10)
                    .Take(10)
                    .ToList();

                result.Total = await _customerRepository.Count();
            }

            return result;
        }

        public async Task<Cliente> GetCustomerById(Guid id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        public async Task<Cliente> CreateCustomer(Cliente customer)
        {
            customer.CreatedOn = DateTime.Now;
            return await _customerRepository.AddAsync(customer);
        }

        public async Task UpdateCustomer(Cliente customer)
        {
            await _customerRepository.UpdateAsync(customer);
        }

        public async Task DeleteCustomer(Guid id)
        {
            await _customerRepository.DeleteAsync(id);
        }
    }
}
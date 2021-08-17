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
    public class ProviderService : IProviderService
    {
        private readonly IAsyncRepository<Proveedor> _providerRepository;

        public ProviderService(IAsyncRepository<Proveedor> providerRepository)
        {
            _providerRepository = providerRepository;
        }

        public async Task<List<Proveedor>> GetProviders()
        {
            return (await _providerRepository.ListAllAsync()).OrderBy(x => x.UpdatedOn).ToList();
        }

        public async Task<PagedResult<Proveedor>> GetProvidersPaged(int page, string search)
        {
            var result = new PagedResult<Proveedor>();
            if (!string.IsNullOrEmpty(search))
            {
                result.Data = _providerRepository
                    .Query(x => x.Nombre.Contains(search))
                    .OrderByDescending(x => x.UpdatedOn)
                    .Skip((page - 1) * 10)
                    .Take(10)
                    .ToList();

                result.Total = await _providerRepository.CountByQuery(x => x.Nombre.Contains(search));
            }
            else
            {
                result.Data = (await _providerRepository
                    .ListAllAsync())
                    .OrderByDescending(x => x.UpdatedOn)
                    .Skip((page - 1) * 10)
                    .Take(10)
                    .ToList();

                result.Total = await _providerRepository.Count();
            }

            return result;
        }

        public async Task<Proveedor> GetProviderById(Guid id)
        {
            return await _providerRepository.GetByIdAsync(id);
        }

        public async Task<Proveedor> CreateProvider(Proveedor provider)
        {
            provider.CreatedOn = DateTime.Now;
            return await _providerRepository.AddAsync(provider);
        }

        public void UpdateProvider(Proveedor provider)
        {
            _providerRepository.UpdateAsync(provider);
        }

        public void DeleteProvider(Guid id)
        {
            _providerRepository.DeleteAsync(id);
        }
    }
}
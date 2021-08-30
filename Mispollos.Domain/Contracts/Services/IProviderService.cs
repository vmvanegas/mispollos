using Mispollos.Domain.Entities;
using Mispollos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mispollos.Domain.Contracts.Services
{
    public interface IProviderService
    {
        Task<List<Proveedor>> GetProviders();

        Task<Proveedor> GetProviderById(Guid id);

        Task<PagedResult<Proveedor>> GetProvidersPaged(int page, string search);

        Task<Proveedor> CreateProvider(Proveedor provider);

        Task UpdateProvider(Proveedor provider);

        Task DeleteProvider(Guid id);
    }
}
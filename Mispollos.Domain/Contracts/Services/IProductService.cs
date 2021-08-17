using Mispollos.Domain.Entities;
using Mispollos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mispollos.Domain.Contracts.Services
{
    public interface IProductService
    {
        Task<List<Producto>> GetProducts();

        Task<Producto> GetProductById(Guid id);

        Task<PagedResult<Producto>> GetProductsPaged(int page, string search);

        Task<Producto> CreateProduct(Producto product);

        void UpdateProduct(Producto product);

        void DeleteProduct(Guid id);
    }
}
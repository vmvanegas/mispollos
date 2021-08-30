using Microsoft.EntityFrameworkCore;
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
    public class ProductService : IProductService
    {
        private readonly IAsyncRepository<Producto> _productRepository;

        public ProductService(IAsyncRepository<Producto> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Producto>> GetProducts()
        {
            return (await _productRepository.ListAllAsync()).OrderBy(x => x.Nombre).ToList();
        }

        public async Task<PagedResult<Producto>> GetProductsPaged(int page, string search)
        {
            var result = new PagedResult<Producto>();
            if (!string.IsNullOrEmpty(search))
            {
                result.Data = _productRepository
                    .Query(x => x.Nombre.Contains(search))
                    .OrderByDescending(x => x.UpdatedOn)
                    .Include(x => x.Proveedor).Include(x => x.Categoria)
                    .Skip((page - 1) * 10)
                    .Take(10)
                    .ToList();

                result.Total = await _productRepository.CountByQuery(x => x.Nombre.Contains(search));
            }
            else
            {
                result.Data = _productRepository.Query()
                    .OrderByDescending(x => x.UpdatedOn)
                    .Include(x => x.Proveedor).Include(x => x.Categoria)
                    .Skip((page - 1) * 10)
                    .Take(10)
                    .ToList();

                result.Total = await _productRepository.Count();
            }

            return result;
        }

        public async Task<Producto> GetProductById(Guid id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<Producto> CreateProduct(Producto product)
        {
            product.CreatedOn = DateTime.Now;
            return await _productRepository.AddAsync(product);
        }

        public async Task UpdateProduct(Producto product)
        {
            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteProduct(Guid id)
        {
            await _productRepository.DeleteAsync(id);
        }
    }
}
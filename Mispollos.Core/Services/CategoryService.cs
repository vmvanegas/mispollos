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
    public class CategoryService : ICategoryService
    {
        private readonly IAsyncRepository<Categoria> _categoryRepository;

        public CategoryService(IAsyncRepository<Categoria> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<Categoria>> GetCategories()
        {
            return (await _categoryRepository.ListAllAsync()).OrderBy(x => x.UpdatedOn).ToList();
        }

        public async Task<PagedResult<Categoria>> GetCategoriesPaged(int page, string search)
        {
            var result = new PagedResult<Categoria>();
            if (!string.IsNullOrEmpty(search))
            {
                result.Data = _categoryRepository
                    .Query(x => x.Nombre.Contains(search))
                    .OrderByDescending(x => x.UpdatedOn)
                    .Skip((page - 1) * 10)
                    .Take(10)
                    .ToList();

                result.Total = await _categoryRepository.CountByQuery(x => x.Nombre.Contains(search));
            }
            else
            {
                result.Data = (await _categoryRepository
                    .ListAllAsync())
                    .OrderByDescending(x => x.UpdatedOn)
                    .Skip((page - 1) * 10)
                    .Take(10)
                    .ToList();

                result.Total = await _categoryRepository.Count();
            }

            return result;
        }

        public async Task<Categoria> GetCategoryById(Guid id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<Categoria> CreateCategory(Categoria category)
        {
            category.CreatedOn = DateTime.Now;
            return await _categoryRepository.AddAsync(category);
        }

        public async Task UpdateCategory(Categoria category)
        {
            await _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteCategory(Guid id)
        {
            await _categoryRepository.DeleteAsync(id);
        }
    }
}
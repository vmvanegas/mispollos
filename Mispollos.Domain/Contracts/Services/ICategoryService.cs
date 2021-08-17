using Mispollos.Domain.Entities;
using Mispollos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mispollos.Domain.Contracts.Services
{
    public interface ICategoryService
    {
        Task<List<Categoria>> GetCategories();

        Task<Categoria> GetCategoryById(Guid id);

        Task<PagedResult<Categoria>> GetCategoriesPaged(int page, string search);

        Task<Categoria> CreateCategory(Categoria category);

        void UpdateCategory(Categoria category);

        void DeleteCategory(Guid id);
    }
}
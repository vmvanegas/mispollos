using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mispollos.Domain.Contracts.Services;
using Mispollos.Domain.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mispollos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriaController(ICategoryService service)
        {
            _service = service;
        }

        // Metodo traer lista de categorias

        // GET: api/<CateogoriaController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetCategories();
            return Ok(result);
        }

        // GET: api/<CateogoriaController>/p/pagina
        [HttpGet("p/{page}")]
        public async Task<IActionResult> Get(int page, string search = null)
        {
            var result = await _service.GetCategoriesPaged(page, search);
            return Ok(result);
        }

        // GET: api/categoria/search

        // Traer un categoria por id
        // GET api/<CateogoriaController>/{id}
        [HttpGet("{id}")]
        public async Task<Categoria> Get(Guid id)
        {
            return await _service.GetCategoryById(id);
        }

        //// Crear categoria
        //// POST api/<CateogoriaController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Categoria categoria)
        {
            var result = await _service.CreateCategory(categoria);
            return Ok(result);
        }

        //// Actualizar categoria
        //// PUT api/<CateogoriaController>/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Categoria categoria)
        {
            await _service.UpdateCategory(categoria);

            return Ok();
        }

        //// Borrar categoria
        //// DELETE api/<CateogoriaController>/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteCategory(id);

            return Ok();
        }
    }
}
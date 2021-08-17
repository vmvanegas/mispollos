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
    [Authorize]
    public class ProductoController : ControllerBase
    {
        //private readonly MisPollosContext _context = new MisPollosContext();

        // Metodo traer lista de usuarios

        // GET: api/<ProductoController>

        private readonly IProductService _service;

        public ProductoController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetProducts();
            return Ok(result);
        }

        // GET: api/<ProductoController>/p/{page}

        [HttpGet("p/{page}")]
        public async Task<IActionResult> Get(int page, string search = null)
        {
            var result = await _service.GetProductsPaged(page, search);
            return Ok(result);
        }

        // Traer un usuario por id
        // GET api/<ProductoController>/5
        [HttpGet("{id}")]
        public async Task<Producto> Get(Guid id)
        {
            return await _service.GetProductById(id);
        }

        // Crear usuario
        // POST api/<ProductoController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Producto product)
        {
            var result = await _service.CreateProduct(product);
            return Ok(result);
        }

        // Actualizar usuario
        // PUT api/<ProductoController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Producto product)
        {
            _service.UpdateProduct(product);
            return Ok();
        }

        // Borrar usuario
        // DELETE api/<ProductoController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _service.DeleteProduct(id);
        }
    }
}
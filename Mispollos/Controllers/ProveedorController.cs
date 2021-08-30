using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Mispollos.Configuration;
using Mispollos.Domain.Contracts.Services;
using Mispollos.Domain.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mispollos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProveedorController : ControllerBase
    {
        private readonly IProviderService _service;

        public ProveedorController(IProviderService service)
        {
            _service = service;
        }

        // Metodo traer lista de proveedores

        // GET: api/<ProveedorController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetProviders();
            return Ok(result);
        }

        // GET: api/<ProveedorController>/1
        [HttpGet("p/{page}")]
        public async Task<IActionResult> Get(int page, string search = null)
        {
            var result = await _service.GetProvidersPaged(page, search);
            return Ok(result);
        }

        // Traer un proveedor por id
        // GET api/<ProveedorController>/5
        [HttpGet("{id}")]
        public async Task<Proveedor> Get(Guid id)
        {
            return await _service.GetProviderById(id);
        }

        // Crear proveedor
        // POST api/<ProveedorController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Proveedor provider)
        {
            var result = await _service.CreateProvider(provider);
            return Ok(result);
        }

        // Actualizar proveedor
        // PUT api/<ProveedorController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Proveedor provider)
        {
            await _service.UpdateProvider(provider);

            return Ok();
        }

        // Borrar proveedor
        // DELETE api/<ProveedorController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteProvider(id);
            return Ok();
        }
    }
}
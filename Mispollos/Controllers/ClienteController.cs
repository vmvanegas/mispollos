using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mispollos.Configuration;
using Mispollos.Domain.Contracts.Services;
using Mispollos.Domain.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mispollos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClienteController : ControllerBase
    {
        private readonly ICustomerService _service;

        public ClienteController(ICustomerService service)
        {
            _service = service;
        }

        // Metodo traer lista de clientes

        // GET: api/<ClienteController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetCustomers();
            return Ok(result);
        }

        // GET: api/<ClienteController>/p/{page}

        [HttpGet("p/{page}")]
        public async Task<IActionResult> Get(int page, string search = null)
        {
            var result = await _service.GetCustomersPaged(page, search);
            return Ok(result);
        }

        // Traer un cliente por id
        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<Cliente> Get(Guid id)
        {
            return await _service.GetCustomerById(id);
        }

        // Crear cliente
        // POST api/<ClienteController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cliente customer)
        {
            var result = await _service.CreateCustomer(customer);
            return Ok(result);
        }

        // Actualizar usuario
        // PUT api/<ClienteController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Cliente customer)
        {
            _service.UpdateCustomer(customer);

            return Ok();
        }

        // Borrar usuario
        // DELETE api/<ClienteController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _service.DeleteCustomer(id);
        }
    }
}
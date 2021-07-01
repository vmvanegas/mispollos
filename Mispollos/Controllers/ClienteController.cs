using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mispollos.Configuration;
using Mispollos.DataAccess;
using Mispollos.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mispollos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClienteController : ControllerBase
    {
        private readonly MisPollosContext _context = new MisPollosContext();

        // Metodo traer lista de clientes

        // GET: api/<ClienteController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { data = _context.Cliente.OrderBy(x => x.Nombre).AsEnumerable() });
        }

        // GET: api/<ClienteController>/p/{page}

        [HttpGet("p/{page}")]
        public IActionResult Get(int page)
        {
            return Ok(new { data = _context.Cliente.OrderByDescending(x => x.UpdatedOn).Skip((page - 1) * 10).Take(10).AsEnumerable(), total = _context.Cliente.Count() });
        }

        // Traer un cliente por id
        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public Cliente Get(Guid id)
        {
            return _context.Cliente.FirstOrDefault(x => x.Id == id);
        }

        // Crear cliente
        // POST api/<ClienteController>
        [HttpPost]
        public IActionResult Post([FromBody] Cliente cliente)
        {
            cliente.CreatedOn = DateTime.Now;
            var result = _context.Cliente.Add(cliente);
            _context.SaveChanges();

            return Created("", result.Entity);
        }

        // Actualizar usuario
        // PUT api/<ClienteController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Cliente cliente)
        {
            var result = _context.Cliente.Attach(cliente);
            _context.Entry(cliente).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(result.Entity);
        }

        // Borrar usuario
        // DELETE api/<ClienteController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            var cliente = _context.Cliente.FirstOrDefault(x => x.Id == id);
            if (_context.Entry(cliente).State == EntityState.Detached)
            {
                _context.Cliente.Attach(cliente);
            }
            _context.Cliente.Remove(cliente);
            _context.SaveChanges();
        }
    }
}
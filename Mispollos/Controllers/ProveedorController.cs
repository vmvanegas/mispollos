using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Mispollos.Configuration;
using Mispollos.DataAccess;
using Mispollos.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mispollos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProveedorController : ControllerBase
    {
        private readonly MisPollosContext _context = new MisPollosContext();

        // Metodo traer lista de proveedores

        // GET: api/<ProveedorController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { data = _context.Proveedor.AsEnumerable() });
        }

        // GET: api/<ProveedorController>/1
        [HttpGet("p/{page}")]
        public IActionResult Get(int page)
        {
            return Ok(new { data = _context.Proveedor.Skip((page - 1) * 10).Take(10).AsEnumerable(), total = _context.Proveedor.Count() });
        }

        // Traer un proveedor por id
        // GET api/<ProveedorController>/5
        [HttpGet("{id}")]
        public Proveedor Get(Guid id)
        {
            return _context.Proveedor.FirstOrDefault(x => x.Id == id);
        }

        // Crear proveedor
        // POST api/<ProveedorController>
        [HttpPost]
        public IActionResult Post([FromBody] Proveedor proveedor)
        {
            var result = _context.Proveedor.Add(proveedor);
            _context.SaveChanges();

            return Created("", result.Entity);
        }

        // Actualizar proveedor
        // PUT api/<ProveedorController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Proveedor proveedor)
        {
            var result = _context.Proveedor.Attach(proveedor);
            _context.Entry(proveedor).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(result.Entity);
        }

        // Borrar proveedor
        // DELETE api/<ProveedorController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            var proveedor = _context.Proveedor.FirstOrDefault(x => x.Id == id);
            if (_context.Entry(proveedor).State == EntityState.Detached)
            {
                _context.Proveedor.Attach(proveedor);
            }
            _context.Proveedor.Remove(proveedor);
            _context.SaveChanges();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mispollos.DataAccess;
using Mispollos.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mispollos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriaController : ControllerBase
    {
        private readonly MisPollosContext _context = new MisPollosContext();

        // Metodo traer lista de categorias

        // GET: api/<CateogoriaController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { data = _context.Categoria.AsEnumerable() });
        }

        // GET: api/<CateogoriaController>/p/pagina
        [HttpGet("p/{page}")]
        public IActionResult Get(int page)
        {
            return Ok(new { data = _context.Categoria.Skip((page - 1) * 10).Take(10).AsEnumerable(), total = _context.Categoria.Count() });
        }

        // Traer un categoria por id
        // GET api/<CateogoriaController>/5
        [HttpGet("{id}")]
        public Categoria Get(Guid id)
        {
            return _context.Categoria.FirstOrDefault(x => x.Id == id);
        }

        // Crear categoria
        // POST api/<CateogoriaController>
        [HttpPost]
        public IActionResult Post([FromBody] Categoria categoria)
        {
            var result = _context.Categoria.Add(categoria);
            _context.SaveChanges();

            return Created("", result.Entity);
        }

        // Actualizar categoria
        // PUT api/<CateogoriaController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Categoria categoria)
        {
            var result = _context.Categoria.Attach(categoria);
            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(result.Entity);
        }

        // Borrar categoria
        // DELETE api/<CateogoriaController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            var categoria = _context.Categoria.FirstOrDefault(x => x.Id == id);
            if (_context.Entry(categoria).State == EntityState.Detached)
            {
                _context.Categoria.Attach(categoria);
            }
            _context.Categoria.Remove(categoria);
            _context.SaveChanges();
        }
    }
}
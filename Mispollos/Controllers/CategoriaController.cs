using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mispollos.DataAccess;
using Mispollos.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mispollos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly MisPollosContext _context = new MisPollosContext();

        // Metodo traer lista de usuarios
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<Categoria> Get()
        {
            return _context.Categoria.AsEnumerable();
        }

        // Traer un usuario por id
        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public Categoria Get(Guid id)
        {
            return _context.Categoria.FirstOrDefault(x => x.Id == id);
        }

        // Crear usuario
        // POST api/<UserController>
        [HttpPost]
        public IActionResult Post([FromBody] Categoria categoria)
        {
            var result = _context.Categoria.Add(categoria);
            _context.SaveChanges();

            return Created("", result.Entity);
        }

        // Actualizar usuario
        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Categoria categoria)
        {
            var result = _context.Categoria.Attach(categoria);
            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(result.Entity);
        }

        // Borrar usuario
        // DELETE api/<UserController>/5
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
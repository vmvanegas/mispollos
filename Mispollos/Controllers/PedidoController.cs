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
    public class PedidoController : ControllerBase
    {
        private readonly MisPollosContext _context = new MisPollosContext();

        // Metodo traer lista de usuarios
        // GET: api/<UserController>

        [HttpGet("p/{page}")]
        public IActionResult Get(int page)
        {
            return Ok(new { data = _context.Pedido.Include(x => x.Cliente).Include(x => x.Usuario).Include(x => x.PedidoProducto).Skip((page - 1) * 10).Take(10).AsEnumerable(), total = _context.Pedido.Count() });
        }

        // Traer un usuario por id
        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public Pedido Get(Guid id)
        {
            return _context.Pedido.FirstOrDefault(x => x.Id == id);
        }

        // Crear usuario
        // POST api/<UserController>
        [HttpPost]
        public IActionResult Post([FromBody] Pedido pedido)
        {
            var result = _context.Pedido.Add(pedido);
            _context.SaveChanges();

            return Created("", result.Entity);
        }

        // Actualizar usuario
        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Pedido pedido)
        {
            var result = _context.Pedido.Attach(pedido);
            _context.Entry(pedido).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(result.Entity);
        }

        // Borrar usuario
        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            var pedido = _context.Pedido.FirstOrDefault(x => x.Id == id);
            if (_context.Entry(pedido).State == EntityState.Detached)
            {
                _context.Pedido.Attach(pedido);
            }
            _context.Pedido.Remove(pedido);
            _context.SaveChanges();
        }
    }
}
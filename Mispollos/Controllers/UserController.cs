using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mispollos.DataAccess;
using Mispollos.Entities;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mispollos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MisPollosContext _context = new MisPollosContext();

        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<Usuario> Get()
        {
            return _context.Usuarios.Include(x => x.Tienda).Include(x => x.Rol).AsEnumerable();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public Usuario Get(Guid id)
        {
            return _context.Usuarios.FirstOrDefault(x => x.Id == id);
        }

        // POST api/<UserController>
        [HttpPost]
        public Usuario Post([FromBody] Usuario usuario)
        {
            usuario.IdRol = new Guid("a4c50173-f674-42ae-8959-5b8e2b773148");
            var result = _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return result.Entity;
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public Usuario Put(Usuario usuario)
        {
            var result = _context.Usuarios.Attach(usuario);
            _context.Entry(usuario).State = EntityState.Modified;
            _context.SaveChanges();

            return result.Entity;
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(x => x.Id == id);
            if (_context.Entry(usuario).State == EntityState.Detached)
            {
                _context.Usuarios.Attach(usuario);
            }
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
        }
    }
}
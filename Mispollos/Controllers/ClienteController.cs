﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class ClienteController : ControllerBase
    {
        private readonly MisPollosContext _context = new MisPollosContext();

        // Metodo traer lista de clientes
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<Cliente> Get()
        {
            return _context.Cliente.AsEnumerable();
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
        public IActionResult Post([FromBody] Usuario usuario)
        {
            var result = _context.Usuarios.Add(usuario);
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
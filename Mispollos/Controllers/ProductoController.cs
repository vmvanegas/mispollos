﻿using System;
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
    public class ProductoController : ControllerBase
    {
        private readonly MisPollosContext _context = new MisPollosContext();

        // Metodo traer lista de usuarios

        // GET: api/<ProductoController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { data = _context.Producto.OrderBy(x => x.Nombre).AsEnumerable() });
        }

        // GET: api/<ProductoController>/p/{page}

        [HttpGet("p/{page}")]
        public IActionResult Get(int page)
        {
            return Ok(new { data = _context.Producto.OrderByDescending(x => x.UpdatedOn).Skip((page - 1) * 10).Include(x => x.Proveedor).Include(x => x.Categoria).Take(10).AsEnumerable(), total = _context.Producto.Count() });
        }

        // Traer un usuario por id
        // GET api/<ProductoController>/5
        [HttpGet("{id}")]
        public Producto Get(Guid id)
        {
            return _context.Producto.Include(x => x.Proveedor).Include(x => x.Categoria).FirstOrDefault(x => x.Id == id);
        }

        // Crear usuario
        // POST api/<ProductoController>
        [HttpPost]
        public IActionResult Post([FromBody] Producto producto)
        {
            producto.CreatedOn = DateTime.Now;
            var result = _context.Producto.Add(producto);
            _context.SaveChanges();

            return Created("", result.Entity);
        }

        // Actualizar usuario
        // PUT api/<ProductoController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Producto producto)
        {
            var result = _context.Producto.Attach(producto);
            _context.Entry(producto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(result.Entity);
        }

        // Borrar usuario
        // DELETE api/<ProductoController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            var producto = _context.Producto.FirstOrDefault(x => x.Id == id);
            if (_context.Entry(producto).State == EntityState.Detached)
            {
                _context.Producto.Attach(producto);
            }
            _context.Producto.Remove(producto);
            _context.SaveChanges();
        }
    }
}
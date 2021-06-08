using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mispollos.DataAccess;
using Mispollos.Entities;
using Mispollos.Models;
using Mispollos.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mispollos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly MisPollosContext _context = new MisPollosContext();

        // Metodo traer lista de pedidos

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { data = _context.Pedido.Include(x => x.Cliente).Include(x => x.Usuario).Include(x => x.PedidoProducto).AsEnumerable() });
        }

        // GET: api/<PedidoController>

        [HttpGet("p/{page}")]
        public IActionResult Get(int page)
        {
            return Ok(new { data = _context.Pedido.Include(x => x.Cliente).Include(x => x.Usuario).Include(x => x.PedidoProducto).Skip((page - 1) * 10).Take(10).AsEnumerable(), total = _context.Pedido.Count() });
        }

        // Traer un pedido por id
        // GET api/<PedidoController>/5
        [HttpGet("{id}")]
        public Pedido Get(Guid id)
        {
            return _context.Pedido.Include(x => x.Cliente).Include(x => x.Usuario).Include(x => x.PedidoProducto).FirstOrDefault(x => x.Id == id);
        }

        // Crear pedido
        // POST api/<PedidoController>
        [HttpPost]
        public IActionResult Post([FromBody] PedidoDto dto)
        {
            // Consulta los id de producto enviados por el frontend
            var products = _context.Producto.Where(x => dto.ListaProductos.Select(s => s.IdProducto).Contains(x.Id));
            decimal total = 0;

            // Calcula el valor total del pedido multiplicando el precio de cada producto por su cantidad y sumandolo
            foreach (var producto in products)
            {
                var cantidad = dto.ListaProductos.First(x => x.IdProducto == producto.Id).Cantidad;
                total += cantidad * producto.Precio;
            }

            var pedido = new Pedido
            {
                IdCliente = dto.IdCliente,
                IdUsuario = dto.IdUsuario,
                ValorTotal = total
            };

            var result = _context.Pedido.Add(pedido);
            _context.SaveChanges();

            foreach (var item in dto.ListaProductos)
            {
                Producto producto = _context.Producto.First(x => x.Id == item.IdProducto);

                producto.Stock -= item.Cantidad;
                _context.Producto.Attach(producto);
                _context.Entry(producto).State = EntityState.Modified;
                _context.SaveChanges();

                var productItem = new PedidoProducto
                {
                    IdPedido = result.Entity.Id,
                    IdProducto = item.IdProducto,
                    Cantidad = item.Cantidad,
                    ValorTotal = item.Cantidad * producto.Precio
                };

                _context.PedidoProducto.Add(productItem);
                _context.SaveChanges();
            }

            return Created("", result.Entity);
        }

        // Actualizar pedido
        // PUT api/<PedidoController>/5
        [HttpPut("{id}")]
        public IActionResult Put(PedidoDto dto)
        {
            decimal total = 0;
            // Consulta los id de producto enviados por el frontend
            var products = _context.Producto.Where(x => dto.ListaProductos.Select(s => s.IdProducto).Contains(x.Id));

            // Calcula el valor total del pedido multiplicando el precio de cada producto por su cantidad y sumandolo
            foreach (var producto in products)
            {
                var cantidad = dto.ListaProductos.First(x => x.IdProducto == producto.Id).Cantidad;
                total += cantidad * producto.Precio;
            }

            var pedido = new Pedido
            {
                Id = dto.Id,
                IdCliente = dto.IdCliente,
                IdUsuario = dto.IdUsuario,
                ValorTotal = total,
            };

            var result = _context.Pedido.Attach(pedido);
            _context.Entry(pedido).State = EntityState.Modified;
            _context.SaveChanges();

            var productItems = _context.PedidoProducto.Where(x => x.IdPedido == pedido.Id);

            foreach (var productItem in productItems)
            {
                if (!dto.ListaProductos.Any(x => x.IdProducto == productItem.IdProducto))
                {
                    using (MisPollosContext context = new MisPollosContext())
                    {
                        Producto product = context.Producto.First(x => x.Id == productItem.IdProducto);
                        product.Stock += productItem.Cantidad;
                        context.SaveChanges();
                    }
                    _context.PedidoProducto.Remove(productItem);
                }
            }
            _context.SaveChanges();

            foreach (var item in dto.ListaProductos)
            {
                Producto producto = _context.Producto.First(x => x.Id == item.IdProducto);
                var cantidad = productItems.FirstOrDefault(x => x.IdProducto == item.IdProducto)?.Cantidad ?? 0;
                //                   5            10
                producto.Stock += cantidad - item.Cantidad;
                //                   10            5
                _context.Producto.Attach(producto);
                _context.Entry(producto).State = EntityState.Modified;
                _context.SaveChanges();

                var pedidoProducto = productItems.FirstOrDefault(x => x.IdProducto == item.IdProducto);
                if (pedidoProducto == null)
                {
                    pedidoProducto = new PedidoProducto
                    {
                        IdPedido = pedido.Id,
                        IdProducto = item.IdProducto,
                        Cantidad = item.Cantidad,
                        ValorTotal = item.Cantidad * producto.Precio
                    };

                    _context.PedidoProducto.Add(pedidoProducto);
                    _context.SaveChanges();
                }
                else
                {
                    pedidoProducto.Cantidad = item.Cantidad;
                    _context.PedidoProducto.Attach(pedidoProducto);
                    _context.Entry(pedidoProducto).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }

            return Ok(result.Entity);
        }

        // Borrar pedido
        // DELETE api/<PedidoController>/5
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
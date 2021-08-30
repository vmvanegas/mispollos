using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mispollos.Domain.Contracts.Services;
using Mispollos.Domain.Entities;
using Mispollos.Domain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mispollos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PedidoController : ControllerBase
    {
        private readonly IOrderService _service;

        public PedidoController(IOrderService service)
        {
            _service = service;
        }

        // Metodo traer lista de pedidos

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetOrders();
            return Ok(result);
        }

        // GET: api/<PedidoController>

        [HttpGet("p/{page}")]
        public async Task<IActionResult> Get(int page, string search = null)
        {
            var result = await _service.GetOrdersPaged(page, search);
            return Ok(result);
        }

        // Traer un pedido por id
        // GET api/<PedidoController>/5
        [HttpGet("{id}")]
        public async Task<Pedido> Get(Guid id)
        {
            return await _service.GetOrderById(id);
        }

        // Crear pedido
        // POST api/<PedidoController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PedidoDto dto)
        {
            var result = await _service.CreateOrder(dto);
            return Ok(result);
        }

        // Actualizar pedido
        // PUT api/<PedidoController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(PedidoDto dto)
        {
            await _service.UpdateOrder(dto);
            return Ok();
        }

        // Borrar pedido
        // DELETE api/<PedidoController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteOrder(id);
            return Ok();
        }
    }
}
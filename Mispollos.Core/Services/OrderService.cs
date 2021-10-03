using Microsoft.EntityFrameworkCore;
using Mispollos.Domain.Contracts.Repositories;
using Mispollos.Domain.Contracts.Services;
using Mispollos.Domain.Entities;
using Mispollos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mispollos.Application.Services
{
    internal class OrderService : IOrderService
    {
        private readonly IAsyncRepository<Pedido> _orderRepository;
        private readonly IAsyncRepository<Producto> _productRepository;
        private readonly IAsyncRepository<PedidoProducto> _orderItemRepository;

        public OrderService(IAsyncRepository<Pedido> orderRepository, IAsyncRepository<Producto> productRepository, IAsyncRepository<PedidoProducto> orderItemRepository)
        {
            _productRepository = productRepository;
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
        }

        public async Task<List<Pedido>> GetOrders()
        {
            return await _orderRepository.ListAllAsync();
        }

        public async Task<PagedResult<Pedido>> GetOrdersPaged(int page, string search)
        {
            var result = new PagedResult<Pedido>();
            if (!string.IsNullOrEmpty(search))
            {
                result.Data = _orderRepository
                    .Query(x => x.Cliente.Nombre.Contains(search))
                    .OrderByDescending(x => x.UpdatedOn)
                    .Include(x => x.Cliente).Include(x => x.Usuario).Include(x => x.PedidoProducto)
                    .Skip((page - 1) * 10)
                    .Take(10)
                    .ToList();

                result.Total = await _orderRepository.CountByQuery(x => x.Cliente.Nombre.Contains(search));
            }
            else
            {
                result.Data = _orderRepository.Query()
                    .OrderByDescending(x => x.UpdatedOn)
                    .Include(x => x.Cliente).Include(x => x.Usuario).Include(x => x.PedidoProducto)
                    .Skip((page - 1) * 10)
                    .Take(10)
                    .ToList();

                result.Total = await _orderRepository.Count();
            }

            return result;
        }

        public Pedido GetOrderById(Guid id)
        {
            return _orderRepository.Query().Include(x => x.PedidoProducto).FirstOrDefault(x => x.Id == id);
        }

        public async Task<Pedido> CreateOrder(PedidoDto dto)
        {
            // Consulta los id de producto enviados por el frontend
            var products = _productRepository.Query(x => dto.ListaProductos.Select(s => s.IdProducto).Contains(x.Id));
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
                ValorTotal = total,
                CreatedOn = DateTime.Now
            };

            var result = await _orderRepository.AddAsync(pedido);

            foreach (var item in dto.ListaProductos)
            {
                Producto producto = _productRepository.Query().First(x => x.Id == item.IdProducto);

                producto.Stock -= item.Cantidad;

                await _productRepository.UpdateAsync(producto);

                var productItem = new PedidoProducto
                {
                    IdPedido = result.Id,
                    IdProducto = item.IdProducto,
                    Cantidad = item.Cantidad,
                    ValorTotal = item.Cantidad * producto.Precio
                };

                await _orderItemRepository.AddAsync(productItem);
            }

            return result;
        }

        public async Task UpdateOrder(PedidoDto dto)
        {
            decimal total = 0;
            // consulta los id de producto enviados por el frontend
            var products = _productRepository.Query(x => dto.ListaProductos.Select(s => s.IdProducto).Contains(x.Id));

            // calcula el valor total del pedido multiplicando el precio de cada producto por su cantidad y sumandolo
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

            await _orderRepository.UpdateAsync(pedido);

            var productitems = _orderItemRepository.Query(x => x.IdPedido == pedido.Id).ToList();

            foreach (var productitem in productitems)
            {
                if (!dto.ListaProductos.Any(x => x.IdProducto == productitem.IdProducto))
                {
                    Producto product = _productRepository.Query().First(x => x.Id == productitem.IdProducto);
                    product.Stock += productitem.Cantidad;
                    await _productRepository.UpdateAsync(product);

                    await _orderItemRepository.DeleteCompositeAsync(productitem.IdPedido, productitem.IdProducto);
                }
            }

            foreach (var item in dto.ListaProductos)
            {
                Producto producto = _productRepository.Query().First(x => x.Id == item.IdProducto);
                var cantidad = productitems.FirstOrDefault(x => x.IdProducto == item.IdProducto)?.Cantidad ?? 0;
                //                   5            10
                producto.Stock += cantidad - item.Cantidad;
                //                   10            5

                await _productRepository.UpdateAsync(producto);

                var pedidoproducto = productitems.FirstOrDefault(x => x.IdProducto == item.IdProducto);
                if (pedidoproducto == null)
                {
                    pedidoproducto = new PedidoProducto
                    {
                        IdPedido = pedido.Id,
                        IdProducto = item.IdProducto,
                        Cantidad = item.Cantidad,
                        ValorTotal = item.Cantidad * producto.Precio
                    };

                    await _orderItemRepository.AddAsync(pedidoproducto);
                }
                else
                {
                    pedidoproducto.Cantidad = item.Cantidad;

                    await _orderItemRepository.UpdateAsync(pedidoproducto);
                }
            }
        }

        public async Task DeleteOrder(Guid id)
        {
            await _orderRepository.DeleteAsync(id);
        }
    }
}
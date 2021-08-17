using Mispollos.Domain.Contracts.Repositories;
using Mispollos.Domain.Contracts.Services;
using Mispollos.Domain.Entities;
using Mispollos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mispollos.Application.Services
{
    internal class OrderItemService : IOrderItemService
    {
        private readonly IAsyncRepository<PedidoProducto> _orderItem;

        public async Task<PedidoProducto> CreateOrderItem(PedidoProducto orderItem)
        {
            return await _orderItem.AddAsync(orderItem);
        }

        public void DeleteOrderItem(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PedidoProducto> GetOrderItemById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<PedidoProducto>> GetOrderItems()
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<PedidoProducto>> GetOrderItemsPaged(int page, string search)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrderItem(PedidoProducto orderItem)
        {
            throw new NotImplementedException();
        }
    }
}
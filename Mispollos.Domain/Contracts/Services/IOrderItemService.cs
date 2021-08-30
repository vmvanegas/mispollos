using Mispollos.Domain.Entities;
using Mispollos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mispollos.Domain.Contracts.Services
{
    public interface IOrderItemService
    {
        Task<List<PedidoProducto>> GetOrderItems();

        Task<PedidoProducto> GetOrderItemById(Guid id);

        Task<PagedResult<PedidoProducto>> GetOrderItemsPaged(int page, string search);

        Task<PedidoProducto> CreateOrderItem(PedidoProducto orderItem);

        Task UpdateOrderItem(PedidoProducto orderItem);

        Task DeleteOrderItem(Guid id);
    }
}
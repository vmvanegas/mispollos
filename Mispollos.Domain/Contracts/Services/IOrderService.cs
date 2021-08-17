using Mispollos.Domain.Entities;
using Mispollos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mispollos.Domain.Contracts.Services
{
    public interface IOrderService
    {
        Task<List<Pedido>> GetOrders();

        Task<Pedido> GetOrderById(Guid id);

        Task<PagedResult<Pedido>> GetOrdersPaged(int page, string search);

        Task<Pedido> CreateOrder(PedidoDto pedido);

        Task UpdateOrder(PedidoDto pedido);

        Task DeleteOrder(Guid id);
    }
}
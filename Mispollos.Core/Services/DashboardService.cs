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
    internal class DashboardService : IDashboardService
    {
        private readonly IAsyncRepository<Pedido> _orderService;
        private readonly IAsyncRepository<Producto> _productService;

        public DashboardService(IAsyncRepository<Pedido> orderService, IAsyncRepository<Producto> productService)
        {
            _orderService = orderService;
            _productService = productService;
        }

        public Producto BestSellingProduct()
        {
            var product = _orderService.Query()
                .Include("PedidoProducto.Producto")
                .Where(x => x.Fecha > DateTime.Now.AddDays(-7))
                .SelectMany(x => x.PedidoProducto)
                .AsEnumerable()
                .GroupBy(x => x.IdProducto)
                .OrderByDescending(x => x.Sum(y => y.Cantidad))
                .FirstOrDefault()?
                .FirstOrDefault().Producto;

            return product;
        }

        public Producto CloseDueDate()
        {
            var product = _productService.Query(x => x.FechaVencimiento > DateTime.Now).OrderByDescending(x => x.FechaVencimiento).Last();
            return product;
        }

        public Producto LeastSoldProduct()
        {
            var product = _orderService.Query()
                .Where(x => x.Fecha > DateTime.Now.AddDays(-7))
                .SelectMany(x => x.PedidoProducto).AsEnumerable().GroupBy(x => x.IdProducto)
                .OrderByDescending(x => x.Sum(y => y.Cantidad)).LastOrDefault()?.FirstOrDefault().Producto;

            return product;
        }

        public Producto LowStock()
        {
            var product = _productService.Query().OrderByDescending(x => x.Stock).Last();
            return product;
        }

        public SalesPercentageDto SalesPercentage()
        {
            var labels = new List<DateTime>();
            var dateNow = DateTime.Now;
            double percent;
            for (int i = 0; i < 6; i++)
            {
                labels.Add(dateNow.AddDays(-i));
            }

            double currentWeek = _orderService.Query(x => x.Fecha > DateTime.Now.AddDays(-7)).Count();
            double lastWeek = _orderService.Query(x => x.Fecha > DateTime.Now.AddDays(-14) && x.Fecha < DateTime.Now.AddDays(-7)).Count();

            if (lastWeek > 0)
            {
                percent = ((currentWeek - lastWeek) / lastWeek) * 100;
            }
            else
            {
                percent = -1;
            }

            var increase = false;
            if (currentWeek > lastWeek)
            {
                increase = true;
            }

            SalesPercentageDto salesPercentage = new SalesPercentageDto
            {
                Percent = Math.Round(percent, 2),
                CurrentWeek = currentWeek,
                LastWeek = lastWeek,
                Increase = increase
            };

            return salesPercentage;
        }

        public ChartDto LastWeekSales()
        {
            var labels = new List<DateTime>();
            var dateNow = DateTime.Now;
            for (int i = 0; i < 7; i++)
            {
                labels.Add(dateNow.AddDays(-i));
            }
            var pedidos = _orderService.Query(x => x.Fecha > DateTime.Now.AddDays(-7));

            ChartDto chartDto = new ChartDto
            {
                Labels = labels.Select(x => x.ToString("dddd")).Reverse(),
                Data = labels.Select(x => pedidos.Where(y => y.Fecha.Day == x.Day).Count()).Reverse()
            };

            return chartDto;
        }
    }
}
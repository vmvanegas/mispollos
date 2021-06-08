using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mispollos.DataAccess;

namespace Mispollos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly MisPollosContext _context = new MisPollosContext();

        [HttpGet("last-week")]
        public IActionResult GetLastWeek()
        {
            var labels = new List<DateTime>();
            var dateNow = DateTime.Now;
            for (int i = 0; i < 6; i++)
            {
                labels.Add(dateNow.AddDays(-i));
            }
            var pedidos = _context.Pedido.Where(x => x.Fecha > DateTime.Now.AddDays(-7)).AsEnumerable();

            return Ok(new
            {
                labels = labels.Select(x => x.ToString("dddd")).Reverse(),
                data = labels.Select(x => pedidos.Where(y => y.Fecha.Day == x.Day).Count()).Reverse()
            });
        }

        [HttpGet("sales-increase-percentage")]
        public IActionResult GetSalesIncreasePercentage()
        {
            var labels = new List<DateTime>();
            var dateNow = DateTime.Now;
            for (int i = 0; i < 6; i++)
            {
                labels.Add(dateNow.AddDays(-i));
            }

            double currentWeek = _context.Pedido.Where(x => x.Fecha > DateTime.Now.AddDays(-7)).Count();
            double lastWeek = _context.Pedido.Where(x => x.Fecha > DateTime.Now.AddDays(-14) && x.Fecha < DateTime.Now.AddDays(-7)).Count();

            double percent = ((currentWeek - lastWeek) / lastWeek) * 100;
            var increase = false;
            if (currentWeek > lastWeek)
            {
                increase = true;
            }

            return Ok(new
            {
                percent = Math.Round(percent, 2),
                currentWeek = currentWeek,
                lastWeek = lastWeek,
                increase = increase
            });
        }

        [HttpGet("best-selling-product")]
        public IActionResult bestSellingProduct()
        {
            var product = _context.Pedido.Include("PedidoProducto.Producto")
                .Where(x => x.Fecha > DateTime.Now.AddDays(-7))
                .SelectMany(x => x.PedidoProducto).AsEnumerable().GroupBy(x => x.IdProducto)
                .OrderByDescending(x => x.Sum(y => y.Cantidad)).FirstOrDefault().FirstOrDefault().Producto;

            return Ok(product);
        }

        [HttpGet("least-sold-product")]
        public IActionResult leastSoldProduct()
        {
            var product = _context.Pedido.Include("PedidoProducto.Producto")
                .Where(x => x.Fecha > DateTime.Now.AddDays(-7))
                .SelectMany(x => x.PedidoProducto).AsEnumerable().GroupBy(x => x.IdProducto)
                .OrderByDescending(x => x.Sum(y => y.Cantidad)).LastOrDefault().FirstOrDefault().Producto;

            return Ok(product);
        }

        [HttpGet("close-due-date")]
        public IActionResult closeDueDate()
        {
            var product = _context.Producto.Where(x => x.FechaVencimiento > DateTime.Now).OrderByDescending(x => x.FechaVencimiento).Last();

            return Ok(product);
        }

        [HttpGet("low-stock")]
        public IActionResult lowStock()
        {
            var product = _context.Producto.OrderByDescending(x => x.Stock).Last();

            return Ok(product);
        }
    }
}
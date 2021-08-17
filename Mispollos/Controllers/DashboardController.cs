using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mispollos.Domain.Contracts.Services;
using Mispollos.Domain.Entities;
using Mispollos.Domain.Models;

namespace Mispollos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;

        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        [HttpGet("last-week")]
        public IActionResult GetLastWeek()
        {
            ChartDto result = _service.LastWeekSales();

            return Ok(result);
        }

        [HttpGet("sales-increase-percentage")]
        public IActionResult GetSalesIncreasePercentage()
        {
            SalesPercentageDto salesPercentage = _service.SalesPercentage();
            return Ok(salesPercentage);
        }

        [HttpGet("best-selling-product")]
        public IActionResult bestSellingProduct()
        {
            Producto product = _service.BestSellingProduct();

            return Ok(product);
        }

        [HttpGet("least-sold-product")]
        public IActionResult leastSoldProduct()
        {
            Producto product = _service.LeastSoldProduct();

            return Ok(product);
        }

        [HttpGet("close-due-date")]
        public IActionResult closeDueDate()
        {
            Producto product = _service.CloseDueDate();

            return Ok(product);
        }

        [HttpGet("low-stock")]
        public IActionResult lowStock()
        {
            Producto product = _service.LowStock();
            return Ok(product);
        }
    }
}
using Mispollos.Domain.Entities;
using Mispollos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mispollos.Domain.Contracts.Services
{
    public interface IDashboardService
    {
        ChartDto LastWeekSales();

        SalesPercentageDto SalesPercentage();

        Producto BestSellingProduct();

        Producto LeastSoldProduct();

        Producto CloseDueDate();

        Producto LowStock();
    }
}
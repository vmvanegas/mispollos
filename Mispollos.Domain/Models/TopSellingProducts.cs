using Mispollos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mispollos.Domain.Models
{
    public class TopSellingProducts
    {
        public Producto Product { get; set; }
        public int Quantity { get; set; }
    }
}
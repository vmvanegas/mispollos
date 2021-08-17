using System;
using System.Collections.Generic;
using System.Text;

namespace Mispollos.Domain.Models
{
    public class SalesPercentageDto
    {
        public double Percent { get; set; }
        public double CurrentWeek { get; set; }
        public double LastWeek { get; set; }
        public bool Increase { get; set; }
    }
}
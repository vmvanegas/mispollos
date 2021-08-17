using System;
using System.Collections.Generic;
using System.Text;

namespace Mispollos.Domain.Models
{
    public class ChartDto
    {
        public IEnumerable<string> Labels { get; set; }
        public IEnumerable<int> Data { get; set; }
    }
}
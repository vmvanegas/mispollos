using System;
using System.Collections.Generic;
using System.Text;

namespace Mispollos.Domain.Models
{
    public class PagedResult<T>
    {
        public int Total { get; set; }
        public List<T> Data { get; set; }
    }
}
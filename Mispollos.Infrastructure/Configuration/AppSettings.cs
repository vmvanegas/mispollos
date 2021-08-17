using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mispollos.Infrastructure.Configuration
{
    public class AppSettings
    {
        public Guid IdRolAdmin { get; set; }
        public Guid IdRolUser { get; set; }
        public string Secret { get; set; }
    }
}
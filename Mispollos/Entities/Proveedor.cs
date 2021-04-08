using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mispollos.Entities
{
    [Table("Producto")]
    public class Proveedor
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Nombre { get; set; }

        public string Telefono { get; set; }
    }
}
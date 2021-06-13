using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mispollos.Entities
{
    [Table("Proveedor")]
    public class Proveedor
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Nombre { get; set; }

        public string Telefono { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}
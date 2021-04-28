using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mispollos.Entities
{
    [Table("Categoria")]
    public class Categoria
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey("Tienda")]
        public Guid IdTienda { get; set; }

        public string Nombre { get; set; }

        public Tienda Tienda { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mispollos.Domain.Entities
{
    [Table("Proveedor")]
    public class Proveedor
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Nombre { get; set; }

        public string Telefono { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}
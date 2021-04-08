using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mispollos.Entities
{
    [Table("Producto")]//Atributos para que lo mapee en la base de datos
    public class Producto
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Llave foranea
        [ForeignKey("Tienda")]
        public Guid IdTienda { get; set; }

        [ForeignKey("Categoria")]
        public Guid IdCategoria { get; set; }

        [ForeignKey("Proveedor")]
        public Guid IdProveedor { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }
        public int Stock { get; set; }
        public string FechaVencimiento { get; set; }
        public string Precio { get; set; }

        public Categoria Categoria { get; set; }//Esto es una propiedad de navegacion
        public Proveedor Proveedor { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mispollos.Entities
{
    [Table("Usuario")]//Atributos para que lo mapee en la base de datos
    public class Usuario
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey("Rol")]//Apunta a la propiedad de navegacion
        public Guid IdRol { get; set; }

        [ForeignKey("Tienda")]
        public Guid IdTienda { get; set; }

        // [StringLength(100)]
        public string Nombre { get; set; }

        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }

        public Rol Rol { get; set; }//Esto es una propiedad de navegacion
        public Tienda Tienda { get; set; }
    }
}
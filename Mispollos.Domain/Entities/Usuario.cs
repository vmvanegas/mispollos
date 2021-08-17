using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mispollos.Domain.Entities
{
    [Table("Usuario")]//Atributos para que lo mapee en la base de datos
    public class Usuario
    {
        // Primary key || llave primaria
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Llave foranea
        [ForeignKey("Rol")]//Apunta a la propiedad de navegacion
        // Guid tipo de dato para id ejemplo: "4D0971A0-4131-4EF8-849A-3ADC7B9C5A86"
        public Guid IdRol { get; set; }

        [ForeignKey("Tienda")]
        public Guid IdTienda { get; set; }

        // [StringLength(100)]
        public string Nombre { get; set; }

        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public Guid Token { get; set; }
        public DateTime TokenExpiration { get; set; }
        public Rol Rol { get; set; }//Esto es una propiedad de navegacion
        public Tienda Tienda { get; set; }

        //Update database
    }
}
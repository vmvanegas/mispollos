using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mispollos.Entities
{
    [Table("Pedido")]
    public class Pedido
    {
        public Pedido()
        {
            PedidoProducto = new HashSet<PedidoProducto>();
        }

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Crear modelo con esta propiedad
        [ForeignKey("Usuario")]
        public Guid IdUsuario { get; set; }

        // Crear modelo con esta propiedad
        [ForeignKey("Cliente")]
        public Guid IdCliente { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;
        public decimal ValorTotal { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public Usuario Usuario { get; set; }
        public Cliente Cliente { get; set; }
        public ICollection<PedidoProducto> PedidoProducto { get; set; }
    }
}
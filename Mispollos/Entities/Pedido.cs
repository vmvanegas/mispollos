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
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey("Usuario")]
        public Guid IdUsuario { get; set; }

        [ForeignKey("Cliente")]
        public Guid IdCliente { get; set; }

        public string Fecha { get; set; }
        public decimal ValorTotal { get; set; }
        public Usuario Usuario { get; set; }
        public Cliente Cliente { get; set; }
        public IList<PedidoProducto> PedidoProducto { get; set; }
    }
}
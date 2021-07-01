using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mispollos.Entities
{
    [Table("PedidoProducto")]
    public class PedidoProducto
    {
        [Key, ForeignKey("Pedido"), Column(Order = 1)]
        public Guid IdPedido { get; set; }

        [Key, ForeignKey("Producto"), Column(Order = 2)]
        public Guid IdProducto { get; set; }

        public int Cantidad { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public Pedido Pedido { get; set; }
        public Producto Producto { get; set; }
    }
}
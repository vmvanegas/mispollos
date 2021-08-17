using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mispollos.Domain.Models
{
    public class PedidoDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid IdUsuario { get; set; }
        public Guid IdCliente { get; set; }
        public List<ProductoDto> ListaProductos { get; set; }
    }

    public class ProductoDto
    {
        public Guid IdProducto { get; set; }

        public int Cantidad { get; set; }
    }
}
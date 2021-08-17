using System;
using System.Collections.Generic;
using System.Text;

namespace Mispollos.Domain.Models
{
    public class AuthenticatedUserInfo
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }

        public string Apellido { get; set; }
        public string Correo { get; set; }
        public Guid IdTienda { get; set; }
        public string rol { get; set; }
        public string Token { get; set; }
    }
}
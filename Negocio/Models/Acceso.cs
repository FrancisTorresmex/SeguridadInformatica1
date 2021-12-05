using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Models
{
    public class Acceso
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }
    }
}

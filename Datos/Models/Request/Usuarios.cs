using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Models.Request
{
    public class Usuarios
    {        
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre requerido")]
        [MaxLength(20)]        
        public string Name { get; set; }

        [Required(ErrorMessage = "Correo requerido")]
        [MaxLength(60)]        
        public string Email { get; set; }

        [Required(ErrorMessage = "Contraseña requerido")]
        [MaxLength(256)]        
        public string Password { get; set; }

        [Required(ErrorMessage = "Rol requerido")]        
        public int Role { get; set; }     
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class Usuario
    {
        [Key]        
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre requerido")]
        [MaxLength(20)]
        [Display(Name = "Nombre de usuario")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Correo requerido")]
        [MaxLength(60)]
        [Display(Name = "Correo de usuario")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contraseña requerido")]
        [MaxLength(256)]
        [Display(Name = "Contraseña de usuario")]
        public string Password { get; set; }        

        [Required(ErrorMessage = "Rol requerido")]        
        [Display(Name = "Rol")]
        public int Role { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Models
{
    public class Rol
    {
        [Key]

        public int Id { get; set; }

        [Required(ErrorMessage = "Rol requerido")]
        [Display(Name = "Rol")]
        public string Role { get; set; }
    }
}

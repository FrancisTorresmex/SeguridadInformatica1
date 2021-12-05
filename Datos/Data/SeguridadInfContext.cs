using Datos.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Data
{
    public class SeguridadInfContext : DbContext
    {
        public SeguridadInfContext()
        {
        }

        public SeguridadInfContext(DbContextOptions<SeguridadInfContext> options) : base(options)
        {

        }        

        //Tablas
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Rol> Roles { get; set; }

        //Conexión con base de datos
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=SeguridadInformatica1;Trusted_Connection=True;");
            }
        }
    }
}

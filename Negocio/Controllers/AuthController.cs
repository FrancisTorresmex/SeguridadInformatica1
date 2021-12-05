
using Datos;
using Datos.Data;
using Datos.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocio.Models;
using Utilidades;

namespace SeguridadInformatica1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AltaUsuarioController : ControllerBase 
    {

        //Agregar usuarios
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Add(Usuarios model)
        {
            using (SeguridadInfContext db = new SeguridadInfContext())
            {
                try
                {
                    if (!ModelState.IsValid) return BadRequest("Modelo no valido");

                    var buscar = db.Usuario.Where(s => s.Email == model.Email || s.Name == model.Name).FirstOrDefault();
                    Usuario usuario = new Usuario();

                    if (buscar != null) return Ok("Usuario o correo en uso");

                    //encriptar contraseña
                    var encriptar = Encriptar.ConvertirSHA256(model.Password);

                    //Si no existe se crean                                        
                    usuario.Email = model.Email;
                    usuario.Name = model.Name;
                    usuario.Password = encriptar;
                    usuario.Role = model.Role;
                    
                    db.Usuario.Add(usuario);
                    await db.SaveChangesAsync();

                    return Ok();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }


        //Ver usuarios
        [HttpGet]
        [Authorize(Roles = "admin, empleado")]
        public async Task<IActionResult> Get()
        {
            using(SeguridadInfContext db = new SeguridadInfContext())
            {
                try
                {
                    if (!ModelState.IsValid) return BadRequest("Modelo no valido");

                    List<Usuario> lst = db.Usuario.OrderBy(i => i.Id).ToList();
                    return Ok(lst);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }  
        
        //Ver datos de u usuario
        [HttpGet("Usuario")]
        [Authorize(Roles = "usuario")]
        public async Task<IActionResult> GetUser(int id)
        {
            using (SeguridadInfContext db = new SeguridadInfContext())
            {
                try
                {                    
                    Usuario usuario = db.Usuario.Find(id);
                    if (usuario == null) return BadRequest("El usuario no existe");

                    var nombreRole = db.Roles.Find(usuario.Role); //buscar el nombre del rol mediante la id

                    DatosUsuario usuarios = new DatosUsuario();
                    usuarios.Id = usuario.Id;
                    usuarios.Email = usuario.Email;
                    usuarios.Role = nombreRole.Role;
                    usuarios.Name = usuario.Name;

                    return Ok(usuarios);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

    }
}
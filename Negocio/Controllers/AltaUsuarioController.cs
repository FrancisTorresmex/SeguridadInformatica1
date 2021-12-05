
using Datos;
using Datos.Data;
using Datos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Negocio.Models;
using Negocio.Models.Commons;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Utilidades;

namespace SeguridadInformatica1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase 
    {
        private readonly AppSettingsCommon _appSettingsCommon;
        public AuthController(IOptions<AppSettingsCommon> appSettingsCommon)
        {
            this._appSettingsCommon = appSettingsCommon.Value; //queremos el valor que viene de IOptions de sistema
        }

        //Iniciar sesión
        [HttpPost]
        public async Task<IActionResult> Login(Auth model)
        {
            using (SeguridadInfContext db = new SeguridadInfContext())
            {
                try
                {
                    if (!ModelState.IsValid) return BadRequest("Modelo no valido");

                    //Se encripta la contraseña recibida del modelo
                    var encriptar = Encriptar.ConvertirSHA256(model.Password);

                    var usuario = db.Usuario.Where(s => s.Password == encriptar && s.Email == model.Email).FirstOrDefault();
                    if (usuario == null) return Ok("Correo o contraseña incorrectos");

                    var nombreRol = db.Roles.Find(usuario.Role); //Buscamos el id del rol para remplazarlo por el nombre
                    Acceso acceso = new Acceso();
                    acceso.Id = usuario.Id;
                    acceso.Role = nombreRol.Role;
                    acceso.Token = GenerarToken(usuario);

                    return Ok(acceso);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        //Generar token
        private string GenerarToken(Usuario model)
        {
            SeguridadInfContext db = new SeguridadInfContext();
            var nombreRol = db.Roles.Find(model.Role); //buscamos el id del rol, esto para asignar el nombre del rol mas adelante

            var tokenHandler = new JwtSecurityTokenHandler();
            var llave = Encoding.ASCII.GetBytes(_appSettingsCommon.Secreto);
            var tokenDesriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity
                (
                    new Claim[] 
                    {
                        new Claim(ClaimTypes.NameIdentifier, model.Id.ToString()),
                        new Claim(ClaimTypes.Role, nombreRol.Role)
                    }
                ),
                Expires = DateTime.UtcNow.AddDays(5), //Expiración
                //encriptar todo con tipo SecurityAlgorithms.HmacSha256Signature
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(llave), SecurityAlgorithms.HmacSha256Signature)
            };
            //Crear token
            var token = tokenHandler.CreateToken(tokenDesriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}
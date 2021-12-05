using Datos;
using Microsoft.AspNetCore.Mvc;
using SeguridadInformatica1.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace SeguridadInformatica1.Controllers
{
    public class HomeController : Controller
    {


        public  AltaUsuarioController _altaUsuario = new AltaUsuarioController();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Usuarios()
        {
            return View();
        }

        
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
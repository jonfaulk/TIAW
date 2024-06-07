using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TIAW.Models;
using System.Collections.Generic;

namespace TIAW.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static Cadastro cadastro = new Cadastro();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Cadastro()
        {
            ViewBag.Clientes = cadastro.ListarClientes();
            return View();
        }

        public IActionResult Teste()
        {
            ViewBag.Clientes = cadastro.ListarClientes();
            return View();
        }


        [HttpPost]
        public IActionResult Cadastro(string fullName, string email, string password, int idade, string sexo, string injury, string conte, string injuryDetails)
        {

            ClienteModel cliente = new ClienteModel(fullName, email, password, idade, sexo, injury, conte, injuryDetails);
            cadastro.AdicionarCliente(cliente);

            ViewBag.Clientes = cadastro.ListarClientes();
            return View();
        }

        public IActionResult Contato()
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

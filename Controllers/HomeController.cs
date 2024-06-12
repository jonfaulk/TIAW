using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TIAW.Models;
using System.Collections.Generic;
using System.Linq;

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

        public IActionResult Sobre()
        {
            return View();
        }

        public IActionResult Instrutor(string searchTerm)
        {
            var clientes = cadastro.ListarClientes();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                clientes = clientes.Where(c => c.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            else
            {
                clientes = new List<ClienteModel>();
            }
            FichaTreino fichaTreino = new FichaTreino();
            ViewBag.Fichas = fichaTreino.Fichas;
            ViewBag.Clientes = clientes;
            ViewBag.SearchTerm = searchTerm;
            return View();
        }

       
        [HttpPost]
        public JsonResult GetOpcoesFichaTreino(string opcoesMarcadas)
        {
            var opcoesFichaTreino = TIAW.Models.FichaTreino.MontarFicha(opcoesMarcadas);
            return Json(opcoesFichaTreino);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

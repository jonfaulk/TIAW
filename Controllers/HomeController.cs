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

        [HttpPost]
        public IActionResult Cadastro(string fullName, string email, string password, int idade, string sexo, string injury, string conte, string injuryDetails)
        {
            ViewBag.Clientes = cadastro.ListarClientes();
            ViewBag.Injury = injury;

            if (injury == "sim" && string.IsNullOrEmpty(injuryDetails))
            {
                ModelState.AddModelError("injuryDetails", "Por favor, descreva sua lesão.");
            }

            if (ModelState.IsValid)
            {
                ClienteModel cliente = new ClienteModel(fullName, email, password, idade, sexo, injury, conte, injuryDetails);
                cadastro.AdicionarCliente(cliente);
                return RedirectToAction("Index");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

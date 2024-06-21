using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TIAW.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace TIAW.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static Cadastro cadastro = new Cadastro();
        private static List<FichaTreino> fichasTreino = new List<FichaTreino>();

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

            var clientes = cadastro.ListarClientes();
            bool contemnome = false;

            foreach (ClienteModel c1 in clientes)
            {
                if (c1.FullName == "admin")
                {
                    contemnome = true;
                    break; // Se encontrarmos um cliente com o nome "admin", podemos sair do loop
                }
            }

            if (!contemnome)
            {
                ClienteModel cliente = new ClienteModel("admin", "jon@g.com", "1,2,3,4,5,6,7,8", 31, "masculino", "Não", "Opa", "Não");
                cadastro.AdicionarCliente(cliente);
            }

            return View();
        }


        [HttpPost]
        public IActionResult Cadastro(string fullName, string email, string password, int idade, string sexo, string injury, string conte, string injuryDetails)
        {
            ClienteModel cliente = new ClienteModel(fullName, email, password, idade, sexo, injury, conte, injuryDetails);
            cadastro.AdicionarCliente(cliente);

            return RedirectToAction("Index");
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
            List<ClienteModel> clientesFiltrados = new List<ClienteModel>();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                clientesFiltrados = clientes.Where(c => c.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            ViewBag.Clientes = clientesFiltrados;
            ViewBag.SearchTerm = searchTerm;

            if (clientesFiltrados.Any())
            {
                ViewBag.ID = clientesFiltrados.First().Id;
            }
            else
            {
                ViewBag.ID = null;
            }

            return View();
        }


        [HttpPost]
        public JsonResult GetOpcoesFichaTreino(string opcoesMarcadas)
        {
            var opcoesFichaTreino = TIAW.Models.FichaTreino.MontarFicha(opcoesMarcadas);
            return Json(opcoesFichaTreino);
        }

        public IActionResult Aluno()
        {
            List<ClienteModel> clientes = cadastro.Clientes;
            bool fichaEncontrada = false;

            foreach (FichaTreino ficha in fichasTreino)
            {
                foreach (ClienteModel cliente in clientes)
                {
                    if (ficha.id == cliente.Id)
                    {
                        ViewBag.Nome = cliente.FullName;
                        ViewBag.Email = cliente.Email;
                        ViewBag.Idade = cliente.Idade;

                        var fichaPersonalizada = TIAW.Models.FichaTreino.FichaPersonalizada(ficha.TipoSelecionado, ficha.Exercicios);
                        ViewBag.ListaPerson = fichaPersonalizada;

                        fichaEncontrada = true;
                        break;
                    }
                }
            }

            if (!fichaEncontrada)
            {
                ViewBag.ErrorMessage = "Ficha de treino não encontrada. bb";
            }

            ViewBag.fichasTreino = fichasTreino;

            return View();
        }


        [HttpPost]
        public IActionResult EditarCadastro(string fullName, string email, string password, int idade, string sexo)
        {
            List<ClienteModel> c1 = cadastro.Clientes;

            for (int i = 0; i < c1.Count; i++)
            {
                if (fullName == c1[i].FullName)
                {
                    c1[i].Email = email;
                    c1[i].Password = password;
                    c1[i].Idade = idade;
                    c1[i].Sexo = sexo;
                }
            }
            return RedirectToAction("Login");
        }
    

        [HttpPost]
        public IActionResult SaveFichaTreino(string nome, List<string> tipoSelecionado, List<string> exercicios, int id)
        {

            FichaTreino ficha = new FichaTreino(nome, tipoSelecionado, exercicios, id);
            fichasTreino.Add(ficha);

            return Json(new { success = true, ID = id });
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var cliente = cadastro.Clientes.FirstOrDefault(c => c.Email == email && c.Password == password);

            if (cliente != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, cliente.FullName),
                    new Claim(ClaimTypes.Email, cliente.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                TempData["SuccessMessage"] = "Login realizado com sucesso!";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Email ou senha inválidos.";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        public IActionResult Admin()
        {
            ViewBag.Clientes = cadastro.ListarClientes();
            return View();
        }

        public IActionResult EditUser(int id)
        {
            var user = cadastro.Clientes.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(ClienteModel model)
        {
            if (ModelState.IsValid)
            {
                var user = cadastro.Clientes.FirstOrDefault(u => u.Id == model.Id);
                if (user != null)
                {
                    user.FullName = model.FullName;
                    user.Email = model.Email;
                    user.Idade = model.Idade;
                    user.Sexo = model.Sexo;
                    user.Injury = model.Injury;
                    user.InjuryDetails = model.InjuryDetails;
                    user.Role = model.Role;
                }
                return RedirectToAction("Admin");
            }
            return View(model);
        }

        public IActionResult DeleteUser(int id)
        {
            var cliente = cadastro.Clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        public IActionResult DeleteUserConfirmed(string email)
        {
            var cliente = cadastro.Clientes.FirstOrDefault(c => c.Email == email);
            if (cliente != null)
            {
                cadastro.Clientes.Remove(cliente);
            }
            return RedirectToAction("Admin");
        }

        public IActionResult ChangeRole(int id)
        {
            var cliente = cadastro.Clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        public IActionResult ChangeRole(string email, string role)
        {
            var cliente = cadastro.Clientes.FirstOrDefault(c => c.Email == email);
            if (cliente != null)
            {
                cliente.Role = role;
            }
            return RedirectToAction("Admin");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

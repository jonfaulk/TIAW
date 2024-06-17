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

        public IActionResult Aluno(string nome)
        {
            var fichaTreino = fichasTreino.FirstOrDefault(f => f.NomeAluno == nome);

            if (fichaTreino != null)
            {
                ViewBag.nome = fichaTreino.NomeAluno;
                ViewBag.tipoTreino = fichaTreino.TipoSelecionado;
                ViewBag.Exercicios = fichaTreino.Exercicios;
                return View();
            }

            ViewBag.ErrorMessage = "Ficha de treino não encontrada.";
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

        [HttpPost]
        public ActionResult SuaAcao(string nome, string tipoSelecionado, List<string> exercicios)
        {
            // Cria um objeto FichaTreino usando os dados recebidos
            var fichaTreino = new FichaTreino(nome, tipoSelecionado, exercicios);

            // Verifica se já existe uma ficha para o aluno
            var fichaExistente = fichasTreino.FirstOrDefault(f => f.NomeAluno == nome);
            if (fichaExistente != null)
            {
                // Atualiza a ficha existente
                fichaExistente.TipoSelecionado = tipoSelecionado;
                fichaExistente.Exercicios = exercicios;
            }
            else
            {
                // Adiciona a nova ficha à lista
                fichasTreino.Add(fichaTreino);
            }

            return Json(new { success = true });
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var cliente = cadastro.ListarClientes().FirstOrDefault(c => c.Email == email && c.Password == password);

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
            var user = cadastro.ListarClientes().FirstOrDefault(u => u.Id == id);
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
                var user = cadastro.ListarClientes().FirstOrDefault(u => u.Id == model.Id);
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
            var cliente = cadastro.ListarClientes().FirstOrDefault(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        public IActionResult DeleteUserConfirmed(int id)
        {
            cadastro.RemoverCliente(id);
            return RedirectToAction("Admin");
        }

        public IActionResult ChangeRole(int id)
        {
            var cliente = cadastro.ListarClientes().FirstOrDefault(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        public IActionResult ChangeRole(int id, string role)
        {
            var cliente = cadastro.ListarClientes().FirstOrDefault(c => c.Id == id);
            if (cliente != null)
            {
                cliente.Role = role;
                // Atualizar o papel no banco de dados
                // Implementar a lógica para atualizar a role do cliente no banco de dados aqui
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

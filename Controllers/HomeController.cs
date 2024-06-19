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
using System.Data.SqlClient;

namespace TIAW.Controllers
{
    public class HomeController : Controller
    {
        private readonly static string _conn = @"Data Source=localhost\MSSQLSERVER01;Initial Catalog=db_Academia;Integrated Security=True;Encrypt=False";
        private readonly ILogger<HomeController> _logger;
        private readonly Cadastro _cadastro;
        private static List<FichaTreino> fichasTreino = new List<FichaTreino>();

        public HomeController(ILogger<HomeController> logger, Cadastro cadastro)
        {
            _logger = logger;
            _cadastro = cadastro;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Teste()
        {
            ViewBag.Clientes = _cadastro.ListarClientes();
            return View();
        }

        public IActionResult Cadastro()
        {
            ViewBag.Clientes = _cadastro.ListarClientes();
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(string fullName, string email, string password, int idade, string sexo, string injury, string conte, string injuryDetails)
        {
            ClienteModel cliente = new ClienteModel(fullName, email, password, idade, sexo, injury, conte, injuryDetails);
            _cadastro.AdicionarCliente(cliente);

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
            var clientes = _cadastro.ListarClientes();

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
            var fichaTreino = new FichaTreino(nome, tipoSelecionado, exercicios);

            var fichaExistente = fichasTreino.FirstOrDefault(f => f.NomeAluno == nome);
            if (fichaExistente != null)
            {
                fichaExistente.TipoSelecionado = tipoSelecionado;
                fichaExistente.Exercicios = exercicios;
            }
            else
            {
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
            var cliente = _cadastro.ListarClientes().FirstOrDefault(c => c.Email == email && c.Password == password);

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
            ViewBag.Clientes = _cadastro.ListarClientes();
            return View();
        }

        public IActionResult EditUser(int id)
        {
            ClienteModel cliente = _cadastro.ListarClientes().FirstOrDefault(c => c.Id == id);

            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(ClienteModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                await conn.OpenAsync();
                string query = @"
                UPDATE usuarios SET 
                    nome = @FullName,
                    email = @Email,
                    idade = @Idade,
                    sexo = @Sexo,
                    lesao = @Injury,
                    detalhes_lesao = @InjuryDetails,
                    tipo = @Role
                WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", model.FullName);
                    cmd.Parameters.AddWithValue("@Email", model.Email);
                    cmd.Parameters.AddWithValue("@Idade", model.Idade);
                    cmd.Parameters.AddWithValue("@Sexo", model.Sexo);
                    cmd.Parameters.AddWithValue("@Injury", model.Injury ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@InjuryDetails", model.InjuryDetails ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Role", model.Role);
                    cmd.Parameters.AddWithValue("@Id", model.Id);

                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return RedirectToAction("Admin");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string email, string role)
        {
            using (SqlConnection conn = new SqlConnection(_conn))
            {
                await conn.OpenAsync();
                string query = "UPDATE usuarios SET role = @role WHERE email = @Email";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@role", role);
                    cmd.Parameters.AddWithValue("@Email", email);

                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return RedirectToAction("Admin");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserConfirmed(string email)
        {
            using (SqlConnection conn = new SqlConnection(_conn))
            {
                await conn.OpenAsync();
                string query = "DELETE FROM usuarios WHERE email = @Email";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    await cmd.ExecuteNonQueryAsync();
                }
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
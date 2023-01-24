using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ToDoMVC.Data;
using ToDoMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace ToDoMVC.Controllers;

[Authorize]
public class TarefasController : Controller
{
    private readonly ILogger<TarefasController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly SignInManager<UserModel> _singInManager;
    private readonly UserManager<UserModel> _userManager;

    public TarefasController(ILogger<TarefasController> logger, 
                            ApplicationDbContext context, 
                            SignInManager<UserModel> singInManager,
                            UserManager<UserModel> userManager)
    {
        _logger = logger;
        _context = context;
        _singInManager = singInManager;
        _userManager = userManager;
    }

    public IActionResult TarefasAFazer()
    {
        return View();
    }

    public IActionResult Cadastrar()
    {
        return View();
    }

    public async Task<IActionResult> CadastrarTarefa(Tarefa TarefaACadastrar){

        if (User.Identity.IsAuthenticated)
        {
            var novatarefa = TarefaACadastrar;
            var nomeUsuario = User.Identity.Name;
            var Usuario = _userManager.Users.FirstOrDefault(x => x.UserName == nomeUsuario);
            novatarefa.Manager = Usuario;
            var resultado = _context.Tasks.Add(novatarefa);
            _context.SaveChanges();
            TempData["SucessfulMessage"] = "Tarefa Cadastrada com sucesso!";
            return RedirectToAction(nameof(TarefasAFazer));
        }
        else{
            TempData["ErrorMessage"] = "Faça Login para continuar!";
            return RedirectToAction(nameof(TarefasAFazer));
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }

}
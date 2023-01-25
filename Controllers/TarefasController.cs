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
        var nomeUsuario = User.Identity.Name;
        var Usuario = _userManager.Users.FirstOrDefault(x => x.UserName == nomeUsuario);

        if (Usuario.showChecked == true)
        {
            var tarefas = _context.Tasks.Where(x => x.Manager == Usuario ).ToList();
            ViewData["switchView"] = "Retirar marcados";
            return View(tarefas);
        }
        else{
            var tarefas = _context.Tasks.Where(x => x.Manager == Usuario && x.Check == false).ToList();
            ViewData["switchView"] = "Exibir marcados";
            return View(tarefas);
        }
    }

    public IActionResult Cadastrar()
    {
        return View();
    }

    public async Task<IActionResult> CadastrarTarefa(Tarefa TarefaACadastrar){

        if (User.Identity.IsAuthenticated)
        {
            var novatarefa = TarefaACadastrar;
            var Usuario = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
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

    [HttpPost]
    public async Task<IActionResult> Checkar(int id) { 
        var usuario = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
        var task = _context.Tasks.FirstOrDefault(x => x.Id == id);

        if (task.Manager == usuario){
            if (task.Check == true){
                task.Check = false;
            }else{
                task.Check = true;
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(TarefasAFazer));
        }else{
            return Unauthorized();
        }
    }

    [HttpPost]
    public async Task<IActionResult> switchView(){
        var usuario = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

        if (usuario.showChecked == true){
            usuario.showChecked = false;
        }else{
            usuario.showChecked = true;
        }
        
        var result = await _userManager.UpdateAsync(usuario);

        return RedirectToAction(nameof(TarefasAFazer));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }

}
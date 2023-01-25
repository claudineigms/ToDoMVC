using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoMVC.Models;

namespace ToDoMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<UserModel> _userManager;
    private readonly SignInManager<UserModel> _singInManager;

    public HomeController(ILogger<HomeController> logger, 
                          UserManager<UserModel> userManager,
                          SignInManager<UserModel> signInManager )
    {
        _logger = logger;
        _userManager = userManager;
        _singInManager = signInManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    // public IActionResult Privacy()
    // {
    //     return View();
    // }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar(UserModel NovoUsuario){
        if (NovoUsuario.UserName == null ||
            NovoUsuario.Nome == null ||
            NovoUsuario.Sobrenome == null ||
            NovoUsuario.Email == null ||
            NovoUsuario.DataNascimento == null ||
            NovoUsuario.CPF == null ||
            NovoUsuario.Senha == null)
        {
            TempData["ErrorMessage"] = "Há dados incompletos, verifique!";
            return RedirectToAction(nameof(Register));
        }
        else if(NovoUsuario.Senha == NovoUsuario.ConfirmaSenha && NovoUsuario.Senha != "")
        {
            UserModel UsuarioACadastar = new UserModel();
            UsuarioACadastar = NovoUsuario;
            UsuarioACadastar.NormalizedEmail = NovoUsuario.Email.ToUpper();
            UsuarioACadastar.showChecked = true;
            try
            {
                var resultado = await _userManager.CreateAsync(UsuarioACadastar, NovoUsuario.Senha);
                if (resultado.Succeeded)
                {
                    TempData["SucessfulMessage"] = "Usuario Cadastrado com Sucesso!";
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    TempData["ErrorMessage"] = resultado.ToString();
                    return RedirectToAction(nameof(Register));
                }
            } catch (Exception e){
                Console.WriteLine(e.Message);
                TempData["ErrorMessage"] = "E-mail já cadastrado!";
                return RedirectToAction(nameof(Register));
            }
        }else{
            TempData["ErrorMessage"] = "Senha e conta senha não são identicas";
            return RedirectToAction(nameof(Register));
        }
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logar(UserModel Usuario)
    {
        Console.WriteLine("Teste");
        var resultado = await _singInManager.PasswordSignInAsync(Usuario.UserName, Usuario.Senha, Usuario.ManterConectado, false);
        if (resultado.Succeeded){
            TempData["SucessfulMessage"] = "Login efetuado com Sucesso!";
            return RedirectToAction(nameof(Index));
        }else{
            TempData["ErrorMessage"] = "Login ou senha incorretos!";
            return RedirectToAction(nameof(Login));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _singInManager.SignOutAsync();
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {

        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
}

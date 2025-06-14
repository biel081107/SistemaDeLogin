/*CONTROLLER PARA MEXER COM O ASP.NET CORE IDENTITY*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SistemaDeLogin.Models;
using SistemaDeLogin.Services.Cliente;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

public class ContaController : Controller
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public ContaController(UserManager<IdentityUser> _userManager, SignInManager<IdentityUser> _signInManager, RoleManager<IdentityRole> _roleManager)
    {
        userManager = _userManager;
        signInManager = _signInManager;
        roleManager = _roleManager;
    }

    public IActionResult Index()
    {
        var nickname = User.FindFirst(ClaimTypes.Name)?.Value;
        ViewBag.name = nickname;
        return View();
    }
    public IActionResult Cadastro()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Cadastro(string email, string password, string confirmPassword)
    {
        Console.WriteLine($"{email} - {password} - {confirmPassword}");
        if (!ModelState.IsValid)
        {
            TempData["Erro"] = "Dados Inválidos! Por favor, verifique os campos, e tente novamente.";
            return RedirectToAction("Cadastro");
        }
        if (email == null || password == null)
        {
            TempData["Erro"] = "Email ou senha inválidos!";
            return RedirectToAction("Cadastro");
        }
        
        //Validar Senhas
        if (password.Length < 6)
        {
            TempData["Erro"] = "A senha deve ter pelo menos 6 caracteres!";
            return RedirectToAction("Cadastro");
        }
        if (password.IsNullOrEmpty())
        {
            TempData["Erro"] = "Senha não podem ser vazio!";
            return RedirectToAction("Cadastro");
        }
        if (!password.Any(char.IsUpper))
        {
            TempData["Erro"] = "A senha deve conter pelo menos uma letra maiúscula!";
            return RedirectToAction("Cadastro");
        }
        if (!password.Any(char.IsLower))
        {
            TempData["Erro"] = "A senha deve conter pelo menos uma letra minúscula!";
            return RedirectToAction("Cadastro");
        }
        if (!password.Any(char.IsDigit))
        {
            TempData["Erro"] = "A senha deve conter pelo menos um numero!";
            return RedirectToAction("Cadastro");
        }
        if (!password.Any(c => "!@#$%^&*()_+-=[]{}|;':\",.<>?/".Contains(c)))
        {
            TempData["Erro"] = "A senha deve conter pelo menos um caractere especial!";
            return RedirectToAction("Cadastro");
        }


        //Validar Email
        if (email.IsNullOrEmpty())
        {
            TempData["Erro"] = "Email não podem ser vazio!";
            return RedirectToAction("Cadastro");
        }
        if (!email.Contains("@") || !email.Contains("."))
        {
            TempData["Erro"] = "Email inválido! Por favor, insira um email válido. Ex: abcde@gmail.com";
            return RedirectToAction("Cadastro");
        }
        if (await userManager.FindByEmailAsync(email) != null)
        {
            TempData["Erro"] = "Email já cadastrado! Por favor, insira um email diferente.";
            return RedirectToAction("Cadastro");
        }

        //Validar Senhas
        if (password != confirmPassword)
        {
            TempData["Erro"] = "As senhas não conferem! Por favor, verifique e tente novamente.";
            return RedirectToAction("Cadastro");
        }

        var user = new IdentityUser
        {
            Email = email,
            UserName = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            TempData["Sucesso"] = "Cadastro realizado com sucesso! Você já pode fazer login.";
            await userManager.AddToRoleAsync(user, "User");

            return RedirectToAction("Login");

        }
        return BadRequest(result.Errors);
    }
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Dados Invalido!!!");
        }
        if (email == null || password == null)
        {
            TempData["Erro"] = "Email ou senha inválidos!";
            return RedirectToAction("Login");
        }

        if (email.IsNullOrEmpty())
        {
            TempData["Erro"] = "Email não podem ser vazio!";
            return RedirectToAction("Login");
        }
        if (password.IsNullOrEmpty())
        {
            TempData["Erro"] = "Senha não podem ser vazio!";
            return RedirectToAction("Login");
        }
        if (!email.Contains("@") || !email.Contains("."))
        {
            TempData["Erro"] = "Email inválido! Por favor, insira um email válido. Ex: abcdefgh@gmail.com";
            return RedirectToAction("Login");

        }
        if (password.Length < 6)
        {
            TempData["Erro"] = "A senha deve ter pelo menos 6 caracteres!";
            return RedirectToAction("Login");
        }



        var result = await signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Conta");
        }
        else
        {
            TempData["Erro"] = $"Erro ao tentar logar - Email ou senha inválidos!";
            return RedirectToAction("Login");
        }
    }

    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index","Home");
    }
}
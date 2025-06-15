using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SistemaDeLogin.Models;
using SistemaDeLogin.Services.Cliente;
using System.Threading.Tasks;

[Authorize]
public class ClienteController : Controller
{
    private readonly IClienteService _clienteService;

    public ClienteController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    // ✅ Lista todos os clientes
    public async Task<IActionResult> Index(string? nome)
    {
        if (!string.IsNullOrEmpty(nome))
        {
            var clientesFiltrados = await _clienteService.GetByNameAsync(nome);
            return View(clientesFiltrados);
        }

        var todosClientes = await _clienteService.GetAllClientesAsync();
        var todosOrdenados = todosClientes.OrderBy(x => x.Nome).ToList();
        return View(todosOrdenados);
    }

    // ✅ Página para buscar cliente pelo ID (formulári

    // ✅ Página para adicionar cliente (formulário)
    //[Authorize(Roles = "Admin")]
    public IActionResult Adicionar()
    {
        return View();
    }

    // ✅ Post para adicionar cliente
    [HttpPost]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> Adicionar(Clientes cliente)
    {
        Console.WriteLine($"Email: {cliente.Email}, CPF: {cliente.Cpf}, Nome: {cliente.Nome}, Id: {cliente.IdCliente}");

        if (!ModelState.IsValid)
            return View(cliente); // ou "Adicionar" se for uma view específica

        try
        {
            await _clienteService.AddClienteAsync(cliente);
            TempData["Sucesso"] = "Cliente adicionado com sucesso!";
            return RedirectToAction("Index");
        }
        catch (ArgumentException ex)
        {
            if (ex.Message.Contains("ERRO AO ADICIONAR! JA EXISTE UM CLIENTE COM ESSE CPF"))
            {
                TempData["Erro"] = ex.Message;
                return RedirectToAction("Index"); // melhor que View("Index")
            }
            else if (ex.Message.Contains("ERRO AO ADICIONAR! JA EXISTE UM CLIENTE COM ESSE EMAIL"))
            {
                TempData["Erro"] = ex.Message;
                return RedirectToAction("Index");
            }

            throw; // se for outra exceção conhecida
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado: {ex.Message}");
            TempData["Erro"] = "Ocorreu um erro ao adicionar o cliente.";
            return RedirectToAction("Index");
        }
    }


    // ✅ Post para editar cliente
    [HttpPost]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> Editar(Clientes cliente)
    {
        if (!ModelState.IsValid)
        {
            TempData["Erro"] = "Dados inválidos. Por favor, verifique os campos.";
            return RedirectToAction("Index");
        }

        await _clienteService.UpdateClienteAsync(cliente);
        return RedirectToAction("Index");
    }

    // ✅ Post para deletar cliente
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Deletar(int IdCliente)
    {
        Console.WriteLine(IdCliente);
        if (!ModelState.IsValid || IdCliente <= 0)
        {
            TempData["Erro"] = "Id inválido. Por favor, verifique os campos e tente novamente.";
            return RedirectToAction("Index");
        }

        await _clienteService.DeleteClienteByIdAsync(IdCliente);
        return RedirectToAction("Index");
    }
}
/*
/Views/Cliente/Index.cshtml ➝ Lista todos os clientes

/Views/Cliente/BuscarPorId.cshtml ➝ Formulário pra digitar o ID

/Views/Cliente/Detalhes.cshtml ➝ Mostra os dados do cliente encontrado

/Views/Cliente/Adicionar.cshtml ➝ Formulário de cadastro

/Views/Cliente/Editar.cshtml ➝ Formulário de edição (se quiser implementar)

Obs: O botão de deletar geralmente fica no Index
*/
using Microsoft.AspNetCore.Mvc;
using SistemaDeLogin.Models;
using SistemaDeLogin.Services.Servico;
using SistemaDeLogin.Repositories.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SistemaDeLogin.Services.Cliente;

[Authorize]
public class ServicoController : Controller
{
    private readonly IServicoService servicoService;
    private readonly IClienteService clienteService;

    public ServicoController(IServicoService _servicoService, IClienteService _clienteService)
    {
        servicoService = _servicoService;
        clienteService = _clienteService;
    }

    public async Task<IActionResult> Index()
    {
        var allservicosAsync = await servicoService.GetAllServicoAsync();
        var clientesAync = await clienteService.GetAllClientesAsync();
        var normalClientes = clientesAync.ToList();
        var allservicos = allservicosAsync.ToList();
        var viewModel = allservicos.Join(normalClientes,
        s => s.IdCliente,
        c => c.IdCliente,
        (s, c) => new ServicoComClienteViewModel
        {
            Servico = s,
            NomeCliente = c.Nome
        }).ToList();

    return View(viewModel);

    }
    public async Task<IActionResult> Adicionar()
    {
        var clientes = await clienteService.GetAllClientesAsync();
        ViewBag.Clientes = clientes;
        return View(new Servico());
    }

    [HttpPost]
    public async Task<IActionResult> Adicionar(Servico servico)
    {
        Console.WriteLine($"ID: {servico.IdServico}, Nome: {servico.NomeAparelho}, Defeito: {servico.DefeitoInformado}, Diagnóstico: {servico.Diagnostico}, Valor: {servico.Valor}, Data de Entrada: {servico.DataDeEntrada}, Previsão de Entrega: {servico.PrevisaoDeEntrega}, Status: {servico.Status}");
        if (!ModelState.IsValid)
        {
            
            ViewBag.Clientes = await clienteService.GetAllClientesAsync();
            return View(servico);
        }

        if (servico != null)
        {
            try
            {
                await servicoService.AddServicoAsync(servico);
                TempData["Sucesso"] = "Serviço adicionado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ERRO AO ADICIONAR! ID do cliente inválido.") ||
                    ex.Message.Contains("ERRO AO ADICIONAR! Nome do aparelho não pode ser vazio.") ||
                    ex.Message.Contains("ERRO AO ADICIONAR! Valor não pode ser negativo.") ||
                    ex.Message.Contains("ERRO AO ADICIONAR! Data de entrada não pode ser futura.") ||
                    ex.Message.Contains("ERRO AO ADICIONAR! Data de entrada não pode ser maior que a previsão de entrega."))
                {
                    TempData["Erro"] = ex.Message;
                    return RedirectToAction("Index");
                }
                throw;
            }
        }
        
        return RedirectToAction("Index");
    }


    // Recebe o ID e mostra os detalhes
    [HttpPost]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> Editar(Servico servicoX)
    {
        Console.WriteLine($"ID: {servicoX.IdServico}, Nome: {servicoX.NomeAparelho}, Defeito: {servicoX.DefeitoInformado}, Diagnóstico: {servicoX.Diagnostico}, Valor: {servicoX.Valor}, Data de Entrada: {servicoX.DataDeEntrada}, Previsão de Entrega: {servicoX.PrevisaoDeEntrega}, Status: {servicoX.Status}");
        if (ModelState.IsValid)
        {

            try
            {
                await servicoService.UpdateServicoAsync(servicoX);
                TempData["Sucesso"] = "Serviço editado com sucesso!";
            }
            catch (ArgumentException ex)
            {
                if (ex.Message.Contains("ERRO AO EDITAR! ID do cliente inválido.") ||
                    ex.Message.Contains("ERRO AO EDITAR! Nome do aparelho não pode ser vazio.") ||
                    ex.Message.Contains("ERRO AO EDITAR! Valor não pode ser negativo.") ||
                    ex.Message.Contains("ERRO AO EDITAR! Data de entrada não pode ser futura.") ||
                    ex.Message.Contains("ERRO AO EDITAR! Data de entrada não pode ser maior que a previsão de entrega."))
                {
                    TempData["Erro"] = ex.Message;
                    return RedirectToAction("Index");
                }
                throw;
            }
        }
        else if (servicoService == null)
            return NotFound();
            
        return RedirectToAction("Index");
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteServicoById(int id)
    {
        if (id <= 0)
        {
            // Pode fazer um tratamento melhor, tipo redirecionar ou exibir uma view de erro
            ModelState.AddModelError(string.Empty, "ERRO! ID NÃO PODE SER MENOR OU IGUAL A 0");
            return View("DeleteServicoByIdInitial");
        }

        await servicoService.DeleteServicoByIdAsync(id);
        return RedirectToAction("Index");
    }
}

/*
/Views/Servico/Index.cshtml ➝ Lista todos os serviços

/Views/Servico/BuscarPorId.cshtml ➝ Formulário pra buscar por ID

/Views/Servico/Detalhes.cshtml ➝ Detalhes do serviço encontrado

/Views/Servico/Adicionar.cshtml ➝ Formulário pra adicionar

/Views/Servico/Editar.cshtml ➝ Formulário pra editar

/Views/Servico/Deletar.cshtml ➝ Formulário pra deletar (pode ser um campo pra digitar o ID ou botão direto no Index)
*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SistemaDeLogin.Models;
using SistemaDeLogin.Services.Pecas;

[Authorize]
public class PecasController : Controller
{
    private readonly IPecasService _pecasService;

    public PecasController(IPecasService pecasService)
    {
        _pecasService = pecasService;
    }

    // ✅ Lista todas as peças
    public async Task<IActionResult> Index()
    {
        var pecas = await _pecasService.GetAllPecasAsync();
        return View(pecas);
    }

    // ✅ Página para buscar peça pelo ID (formulário)
    public IActionResult BuscarPorId()
    {
        return View();
    }

    // ✅ Resultado da busca por ID (detalhes da peça)
    // ✅ Página para adicionar peça (formulário)
    //[Authorize(Roles = "Admin")]
    public IActionResult Adicionar()
    {
        return View();
    }

    // ✅ Post para adicionar peça
    [HttpPost]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> Adicionar(Pecas peca)
    {
        if (!ModelState.IsValid)
            return View(peca);

        try
        {
            await _pecasService.AddPecasAsync(peca);
            TempData["Sucesso"] = "Peça adicionada com sucesso!";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("ERRO AO ADICIONAR! O PREÇO DA PEÇA NÃO PODE SER MENOR OU IGUAL A ZERO") ||
                ex.Message.Contains("ERRO AO ADICIONAR! O NOME DA PEÇA NÃO PODE SER VAZIO"))
            {
                TempData["Erro"] = ex.Message;
                return RedirectToAction("Index");
            }
            throw; // se for outra exceção conhecida
        }
    }

    // ✅ Post para editar peça
    [HttpPost]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> Editar(Pecas peca)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Index");;

        try
        {
            await _pecasService.UpdatePecasAsync(peca);
            TempData["Sucesso"] = "Peça editada com sucesso!";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("ERRO AO EDITAR! O PREÇO DA PEÇA NÃO PODE SER MENOR OU IGUAL A ZERO") ||
            ex.Message.Contains("ERRO AO EDITAR! O NOME DA PEÇA NÃO PODE SER VAZIO"))
            {
                TempData["Erro"] = ex.Message;
                return RedirectToAction("Index");
            }
            throw; // se for outra exceção conhecida
        }
        
    }

    // ✅ Post para deletar peça
    [HttpPost]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> Deletar(int id)
    {
        await _pecasService.DeletePecasByIdAsync(id);
        return RedirectToAction("Index");
    }
}
/*🗂️ Views necessárias pra esse Controller funcionar:
/Views/Pecas/Index.cshtml ➝ Lista todas as peças

/Views/Pecas/BuscarPorId.cshtml ➝ Formulário pra digitar o ID

/Views/Pecas/Detalhes.cshtml ➝ Mostra os dados da peça encontrada

/Views/Pecas/Adicionar.cshtml ➝ Formulário de cadastro

/Views/Pecas/Editar.cshtml ➝ Formulário de edição (se quiser implementar)

Obs: O delete geralmente é um botão dentro do Index.*/
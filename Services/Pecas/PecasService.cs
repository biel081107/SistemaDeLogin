namespace SistemaDeLogin.Services.Pecas;
using SistemaDeLogin.Repositories.Pecas;
using SistemaDeLogin.Models;

public class PecasService : IPecasService
{
    private readonly IPecasRepository pecasRepository;

    public PecasService(IPecasRepository _pecasRepository)
    {
        pecasRepository = _pecasRepository;
    }

    public async Task<Pecas> GetByIdAsync(int id)
    {
        if (id <= 0)
            return new Pecas() { NomePeca = "Invalido", Descricao = "Invalido", Preco = 0 };
        return await pecasRepository.GetByIdAsync(id);
         
    }
    public async Task<List<Pecas>> GetAllPecasAsync()
    {
        return await pecasRepository.GetAllPecasAsync();
    }
    public async Task UpdatePecasAsync(Pecas peca)
    {
        if (peca == null)
            return;
        if (peca.Preco <= 0)
            throw new ArgumentException("ERRO AO EDITAR! O PREÇO DA PEÇA NÃO PODE SER MENOR OU IGUAL A ZERO");
        else if (string.IsNullOrEmpty(peca.NomePeca))
            throw new ArgumentException("ERRO AO EDITAR! O NOME DA PEÇA NÃO PODE SER VAZIO");


        await pecasRepository.UpdatePecasAsync(peca);
    }
    public async Task DeletePecasByIdAsync(int id)
    {
        if (id <= 0)
            return;
        await pecasRepository.DeletePecasByIdAsync(id);
    }
    public async Task AddPecasAsync(Pecas peca)
    {
        if (peca == null)
            return;
        if (peca.Preco <= 0)
            throw new ArgumentException("ERRO AO ADICIONAR! O PREÇO DA PEÇA NÃO PODE SER MENOR OU IGUAL A ZERO");
        else if (string.IsNullOrEmpty(peca.NomePeca))
            throw new ArgumentException("ERRO AO ADICIONAR! O NOME DA PEÇA NÃO PODE SER VAZIO");


        await pecasRepository.AddPecasAsync(peca);
    }
}
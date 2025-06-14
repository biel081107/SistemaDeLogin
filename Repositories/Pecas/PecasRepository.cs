namespace SistemaDeLogin.Repositories.Pecas;

using Microsoft.EntityFrameworkCore;
using SistemaDeLogin.Data;
using SistemaDeLogin.Models;

public class PecasRepository : IPecasRepository
{
    private readonly SistemaContext sistemaContext;
    public PecasRepository(SistemaContext _sistemaContext)
    {
        sistemaContext = _sistemaContext;
    }

    public async Task<Pecas> GetByIdAsync(int id)
    {
        var pecaX = await sistemaContext.Pecas.FindAsync(id);

        if (pecaX == null)
        {
            Pecas pecas = new Pecas() { NomePeca = "Invalido", Descricao = "Invalido", Preco = 0 , Status = 0};
            return pecas;
        }
        return pecaX;
    }
    public async Task<List<Pecas>> GetByNameAsync(string name)
    {
        var pecaX = await sistemaContext.Pecas.Where(x => x.NomePeca.Contains(name)).ToListAsync();
        if (pecaX == null)
        {
            var pecaY = new Pecas() { NomePeca = "Invalido", Descricao = "Invalido", Preco = 0, Status = 0 };
            return new List<Pecas>() { pecaY };
        }
        return pecaX;
    }
    public async Task<List<Pecas>> GetAllPecasAsync()
    {
        var pecas = await sistemaContext.Pecas.ToListAsync();
        return pecas;
    }
    public async Task UpdatePecasAsync(Pecas peca)
    {
        if (peca != null)
        {
            var pecaX = await sistemaContext.Pecas.FindAsync(peca.IdPeca);
            if (pecaX != null)
            {
    
                pecaX.NomePeca = peca.NomePeca;
                pecaX.Descricao = peca.Descricao;
                pecaX.Preco = peca.Preco;
                pecaX.Status = peca.Status;
                await sistemaContext.SaveChangesAsync();
            }
        }
    }
    public async Task DeletePecasByIdAsync(int id)
    {
        var produtoX = await sistemaContext.Pecas.FindAsync(id);
        if (produtoX != null)
        {
            sistemaContext.Remove(produtoX);
            await sistemaContext.SaveChangesAsync();
        }
    }
    public async Task AddPecasAsync(Pecas peca)
    {
        if (peca != null)
        {
            if (peca.Preco <= 0)
                throw new ArgumentException("ERRO AO ADICIONAR! O PREÇO DA PEÇA NÃO PODE SER MENOR OU IGUAL A ZERO");
            else if (string.IsNullOrEmpty(peca.NomePeca))
                throw new ArgumentException("ERRO AO ADICIONAR! O NOME DA PEÇA NÃO PODE SER VAZIO");

            await sistemaContext.Pecas.AddAsync(peca);
            await sistemaContext.SaveChangesAsync();
        }
    }
}
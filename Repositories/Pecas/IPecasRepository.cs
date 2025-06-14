namespace SistemaDeLogin.Repositories.Pecas;
using SistemaDeLogin.Data;
using SistemaDeLogin.Models;
public interface IPecasRepository
{
    Task<Pecas> GetByIdAsync(int id);
    Task<List<Pecas>> GetByNameAsync(string name);
    Task<List<Pecas>> GetAllPecasAsync();
    Task UpdatePecasAsync(Pecas peca);
    Task DeletePecasByIdAsync(int id);
    Task AddPecasAsync(Pecas peca);

}
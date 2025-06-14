namespace SistemaDeLogin.Services.Pecas;
using SistemaDeLogin.Repositories.Pecas;
using SistemaDeLogin.Models;
public interface IPecasService
{
    Task<Pecas> GetByIdAsync(int id);
    Task<List<Pecas>> GetAllPecasAsync();
    Task UpdatePecasAsync(Pecas peca);
    Task DeletePecasByIdAsync(int id);
    Task AddPecasAsync(Pecas peca);

}
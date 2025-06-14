using SistemaDeLogin.Data;
using SistemaDeLogin.Models;
namespace SistemaDeLogin.Repositories.Cliente;

public interface IClienteRepository
{
    Task<Clientes> GetByIdAsync(int id);
    Task<List<Clientes>> GetByNameAsync(string name);
    Task<List<Clientes>> GetAllClientesAsync();
    Task UpdateClienteAsync(Clientes cliente);
    Task DeleteClienteByIdAsync(int id);
    Task AddClienteAsync(Clientes cliente);

}
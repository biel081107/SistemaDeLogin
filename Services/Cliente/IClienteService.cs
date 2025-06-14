using SistemaDeLogin.Repositories.Cliente;
using SistemaDeLogin.Models;

namespace SistemaDeLogin.Services.Cliente;
public interface IClienteService
{
    Task<Clientes> GetByIdAsync(int id);
    Task<List<Clientes>> GetAllClientesAsync();
    Task<List<Clientes>> GetByNameAsync(string name);
    Task UpdateClienteAsync(Clientes cliente);
    Task DeleteClienteByIdAsync(int id);
    Task AddClienteAsync(Clientes cliente);
}
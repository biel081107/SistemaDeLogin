using SistemaDeLogin.Repositories.Cliente;
using SistemaDeLogin.Models;
namespace SistemaDeLogin.Services.Cliente;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository clienteRepository;

    public ClienteService(IClienteRepository _clienteRepository)
    {
        clienteRepository = _clienteRepository;
    }
    public async Task<Clientes> GetByIdAsync(int id)
    {
        var clienteX = await clienteRepository.GetByIdAsync(id);
        if (id <= 0 || clienteX == null)
        {
            Clientes clienteInvalido = new Clientes() { Nome = "Não Encontrado", Cpf = "Invalido", Email = "Invalido", Telefone = "Invalido" };
            return clienteInvalido;
        }
        return await clienteRepository.GetByIdAsync(id);
    }
    public async Task<List<Clientes>> GetByNameAsync(string name)
    {
        if (name == null)
        {
            Clientes clienteInvalido = new Clientes() { Nome = "Não Encontrado", Cpf = "Invalido", Email = "Invalido", Telefone = "Invalido" };
            return new List<Clientes>(){clienteInvalido};
        }
        else
        {
            var clienteX = await clienteRepository.GetByNameAsync(name);
            return clienteX;
        }
    }
    public async Task<List<Clientes>> GetAllClientesAsync()
    {
        return await clienteRepository.GetAllClientesAsync();
    }
    public async Task UpdateClienteAsync(Clientes cliente)
    {
        if (cliente == null)
            throw new ArgumentException($"ERRO! CLIENTE NÃO ENCONTRADO");

        if (cliente.Nome == null)
            throw new ArgumentException($"ERRO! NOME DO CLIENTE NÃO PODE SER NULL");
        if (cliente.Cpf == null)
            throw new ArgumentException($"ERRO! CPF DO CLIENTE NÃO PODE SER NULL");
        if (cliente.Email == null)
            throw new ArgumentException($"ERRO! EMAIL DO CLIENTE NÃO PODE SER NULL");
        
        
        await clienteRepository.UpdateClienteAsync(cliente);
    }
    public async Task DeleteClienteByIdAsync(int id)
    {
        if (id > 0)
        {
            await clienteRepository.DeleteClienteByIdAsync(id);
        }
    }
    public async Task AddClienteAsync(Clientes cliente)
    {
        if (cliente != null)
        {
            await clienteRepository.AddClienteAsync(cliente);
        }
    }

}
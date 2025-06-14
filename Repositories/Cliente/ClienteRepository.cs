using Microsoft.EntityFrameworkCore;
using SistemaDeLogin.Data;
using SistemaDeLogin.Models;
namespace SistemaDeLogin.Repositories.Cliente;

public class ClienteRepository : IClienteRepository
{
    private readonly SistemaContext sistemaContext;

    public ClienteRepository(SistemaContext _sistemaContext)
    {
        sistemaContext = _sistemaContext;
    }

    public async Task<Clientes> GetByIdAsync(int id)
    {
        var clienteX = await sistemaContext.Clientes.FindAsync(id);
        if (clienteX == null)
        {
            Clientes clienteInvalido = new Clientes() { Nome = "Não Encontrado", Cpf = "Invalido", Email = "Invalido", Telefone = "Invalido" };
            return clienteInvalido;
        }
        return clienteX;
    }
    public async Task<List<Clientes>> GetByNameAsync(string name)
    {
        var clienteX = await sistemaContext.Clientes.Where(x => x.Nome.Contains(name)).ToListAsync();
        if (clienteX == null)
        {
            Clientes clienteInvalido = new Clientes() { Nome = "Não Encontrado", Cpf = "Invalido", Email = "Invalido", Telefone = "Invalido" };
            return new List<Clientes>(){clienteInvalido};
        }
        return clienteX;
    }
    public async Task<List<Clientes>> GetAllClientesAsync()
    {
        var clientes = await sistemaContext.Clientes.Include(c => c.Servicos).ToListAsync();
        return clientes;
    }
    public async Task UpdateClienteAsync(Clientes cliente)
    {
        var clienteX = await sistemaContext.Clientes.FindAsync(cliente.IdCliente);
        if (clienteX != null)
        {
            clienteX.Nome = cliente.Nome ?? clienteX.Nome;
            clienteX.Email = cliente.Email ?? clienteX.Email;
            clienteX.Telefone = cliente.Telefone ?? clienteX.Telefone;
            clienteX.Cpf = cliente.Cpf ?? clienteX.Cpf;

            await sistemaContext.SaveChangesAsync();
        }
    }

    public async Task DeleteClienteByIdAsync(int id)
    {
        var clienteX = await sistemaContext.Clientes.FindAsync(id);
        if (clienteX != null)
        {
            sistemaContext.Remove(clienteX);
            await sistemaContext.SaveChangesAsync();
        }
    }
    public async Task AddClienteAsync(Clientes cliente)
    {
        if (cliente != null)
        {
            var clienteWithTheSameCpf = await sistemaContext.Clientes.FirstOrDefaultAsync(c => c.Cpf == cliente.Cpf);
            var clienteWithTheSameEmail = await sistemaContext.Clientes.FirstOrDefaultAsync(c => c.Email == cliente.Email);
            if (clienteWithTheSameCpf == null && clienteWithTheSameEmail == null)
            {
                await sistemaContext.Clientes.AddAsync(cliente);
                await sistemaContext.SaveChangesAsync();
            }
            else
            {
                if (clienteWithTheSameCpf != null)
                {
                    throw new ArgumentException("ERRO AO ADICIONAR! JA EXISTE UM CLIENTE COM ESSE CPF");
                }
                else if (clienteWithTheSameEmail != null)
                {
                    throw new ArgumentException("ERRO AO ADICIONAR! JA EXISTE UM CLIENTE COM ESSE EMAIL");
                }
            }
                

            
        }    
        
    }
}
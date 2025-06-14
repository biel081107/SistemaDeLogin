
namespace SistemaDeLogin.Services.Servico;

using SistemaDeLogin.Models;
public interface IServicoService
{
    Task<Servico> GetByIdAsync(int id);
    Task<Servico> GetByNameAsync(string name);
    Task<List<Servico>> GetAllServicoAsync();
    Task UpdateServicoAsync(Servico servico);
    Task DeleteServicoByIdAsync(int id);
    Task AddServicoAsync(Servico servicoX);
}
using SistemaDeLogin.Models;


namespace SistemaDeLogin.Repositories.Servicos;
public interface IServicoRepository
{
    Task<Servico> GetByIdAsync(int id);
    Task<Servico> GetByNameAsync(string name);
    Task<List<Servico>> GetAllServicoAsync();
    Task UpdateServicoAsync(Servico servico);
    Task DeleteServicoByIdAsync(int id);
    Task AddServicoAsync(Servico servicoX);
}
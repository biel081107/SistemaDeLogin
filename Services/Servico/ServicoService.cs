namespace SistemaDeLogin.Services.Servico;
using SistemaDeLogin.Repositories.Servicos;
using SistemaDeLogin.Models;

public class ServicoService : IServicoService
{
    private readonly IServicoRepository _servicoRepository;

    public ServicoService(IServicoRepository servicoRepository)
    {
        _servicoRepository = servicoRepository;
    }

    public async Task<Servico> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            return new Servico
            {
                NomeAparelho = "ID inválido",
                DefeitoInformado = "O ID deve ser maior que zero"
            };
        }

        return await _servicoRepository.GetByIdAsync(id);
    }

    public async Task<Servico> GetByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return new Servico
            {
                NomeAparelho = "Nome inválido",
                DefeitoInformado = "Informe um nome para buscar"
            };
        }

        return await _servicoRepository.GetByNameAsync(name);
    }

    public async Task<List<Servico>> GetAllServicoAsync()
    {
        return await _servicoRepository.GetAllServicoAsync();
    }

    public async Task AddServicoAsync(Servico servico)
    {
        if (servico == null)
        {
            return;
        }

        await _servicoRepository.AddServicoAsync(servico);
    }

    public async Task UpdateServicoAsync(Servico servico)
    {
        if (servico == null)
        {
            return;
        }

        await _servicoRepository.UpdateServicoAsync(servico);
    }

    public async Task DeleteServicoByIdAsync(int id)
    {
        if (id <= 0)
            return;

        await _servicoRepository.DeleteServicoByIdAsync(id);
    }
}


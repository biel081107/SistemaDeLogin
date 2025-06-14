using Microsoft.EntityFrameworkCore;
using SistemaDeLogin.Data;
using SistemaDeLogin.Models;
using SistemaDeLogin.Repositories.Servicos;

namespace SistemaDeLogin.Repositories.Servicos
{
    public class ServicoRepository : IServicoRepository
    {
        private readonly SistemaContext _context;

        public ServicoRepository(SistemaContext context)
        {
            _context = context;
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

            var servico = await _context.Servicos.Include(s => s.Clientes).FirstOrDefaultAsync(s => s.IdServico == id);

            if (servico == null)
            {
                return new Servico
                {
                    NomeAparelho = "Não Encontrado",
                    DefeitoInformado = "Serviço não localizado"
                };
            }

            return servico;
        }

        public async Task<Servico> GetByNameAsync(string name)
        {
            var servico = await _context.Servicos
                .Include(s => s.Clientes)
                .FirstOrDefaultAsync(s => s.NomeAparelho.Contains(name));

            if (servico == null)
            {
                return new Servico
                {
                    NomeAparelho = "Não Encontrado",
                    DefeitoInformado = "Nome não corresponde a nenhum serviço"
                };
            }

            return servico;
        }

        public async Task<List<Servico>> GetAllServicoAsync()
        {
            return await _context.Servicos.Include(s => s.Clientes).ToListAsync();
        }

        public async Task AddServicoAsync(Servico servico)
        {
            if (servico != null &&
                servico.IdCliente > 0 &&
                !string.IsNullOrWhiteSpace(servico.NomeAparelho) &&
                servico.Valor >= 0 &&
                servico.DataDeEntrada <= DateTime.Now &&
                servico.DataDeEntrada <= servico.PrevisaoDeEntrega &&
                servico.PrevisaoDeEntrega >= DateTime.Now)
            {
                try
                {
                    await _context.Servicos.AddAsync(servico);
                    await _context.SaveChangesAsync();
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }

            }
            else
            {
                if (servico != null)
                {
                    if (servico.IdCliente <= 0)
                        throw new ArgumentException("ERRO AO ADICIONAR! ID do cliente inválido.");
                    if (string.IsNullOrWhiteSpace(servico.NomeAparelho))
                        throw new ArgumentException("ERRO AO ADICIONAR! Nome do aparelho não pode ser vazio.");
                    if (servico.Valor < 0)
                        throw new ArgumentException("ERRO AO ADICIONAR! Valor não pode ser negativo.");
                    if (servico.DataDeEntrada > DateTime.Now)
                        throw new ArgumentException("ERRO AO ADICIONAR! Data de entrada não pode ser futura.");
                    if (servico.DataDeEntrada > servico.PrevisaoDeEntrega)
                        throw new ArgumentException("ERRO AO ADICIONAR! Data de entrada não pode ser maior que a previsão de entrega.");
                    if (servico.PrevisaoDeEntrega < DateTime.Now)
                        throw new ArgumentException("ERRO AO ADICIONAR! Previsão de entrega não pode ser no passado.");
                }
            }
        }

        public async Task UpdateServicoAsync(Servico servico)
        {
            var servicoExistente = await _context.Servicos.FindAsync(servico.IdServico);
            Console.WriteLine(servico.DataDeEntrada);
            if (servicoExistente != null &&
                servico.IdCliente > 0 &&
                !string.IsNullOrWhiteSpace(servico.NomeAparelho) &&
                servico.Valor >= 0 &&
                servico.DataDeEntrada <= DateTime.Now &&
                servico.DataDeEntrada <= servico.PrevisaoDeEntrega &&
                servico.PrevisaoDeEntrega >= DateTime.Now)
            {
                servicoExistente.IdCliente = servico.IdCliente;
                servicoExistente.NomeAparelho = servico.NomeAparelho;
                servicoExistente.DefeitoInformado = servico.DefeitoInformado;
                servicoExistente.Diagnostico = servico.Diagnostico;
                servicoExistente.Status = servico.Status;
                servicoExistente.Valor = servico.Valor;
                servicoExistente.DataDeEntrada = servico.DataDeEntrada;
                servicoExistente.PrevisaoDeEntrega = servico.PrevisaoDeEntrega;

                await _context.SaveChangesAsync();
            }
            else
            {
                if (servico != null)
                {
                    if (servico.IdCliente <= 0)
                        throw new ArgumentException("ERRO AO EDITAR! ID do cliente inválido.");
                    if (string.IsNullOrWhiteSpace(servico.NomeAparelho))
                        throw new ArgumentException("ERRO AO EDITAR! Nome do aparelho não pode ser vazio.");
                    if (servico.Valor < 0)
                        throw new ArgumentException("ERRO AO EDITAR! Valor não pode ser negativo.");
                    if (servico.DataDeEntrada > DateTime.Now)
                        throw new ArgumentException("ERRO AO EDITAR! Data de entrada não pode ser futura.");
                    if (servico.DataDeEntrada > servico.PrevisaoDeEntrega)
                        throw new ArgumentException("ERRO AO EDITAR! Data de entrada não pode ser maior que a previsão de entrega.");
                }
            }
        }

        public async Task DeleteServicoByIdAsync(int id)
        {
            var servico = await _context.Servicos.FindAsync(id);
            if (servico != null)
            {
                _context.Servicos.Remove(servico);
                await _context.SaveChangesAsync();
            }
        }
    }
}

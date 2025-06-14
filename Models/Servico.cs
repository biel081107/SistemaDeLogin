using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SistemaDeLogin.Models;

public class Servico
{
    public int IdServico { get; set; }

    [Required]
    public int IdCliente { get; set; }

    [Required(ErrorMessage = "O campo NomeAparelho é obrigatório.")]
    public string NomeAparelho { get; set; } = string.Empty;

    public string DefeitoInformado { get; set; } = string.Empty;

    public string Diagnostico { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo Status é obrigatório.")]
    public int Status { get; set; } //0 - PENDENTE / 1 - COMPLETO / 2 - CANCELADO
    [Required(ErrorMessage = "O Valor do serviço é obrigatório.")]
    public decimal Valor { get; set; }

    [Required]
    public DateTime DataDeEntrada { get; set; }
    [Required]
    public DateTime PrevisaoDeEntrega { get; set; }

    [JsonIgnore]
    public Clientes? Clientes { get; set; }
}
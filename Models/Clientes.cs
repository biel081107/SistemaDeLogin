using System.ComponentModel.DataAnnotations;

namespace SistemaDeLogin.Models;

public class Clientes
{
    public int IdCliente { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo Cpf é obrigatório.")]
    public string Cpf { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    public string Email { get; set; } = string.Empty;

    public List<Servico>? Servicos { get; set; }
}
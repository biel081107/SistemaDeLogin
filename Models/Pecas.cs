using System.ComponentModel.DataAnnotations;

namespace SistemaDeLogin.Models;

public class Pecas
{
    public int IdPeca { get; set; }

    [Required]
    public string NomePeca { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int Status { get; set; } = 0; // 1 = Comprado, 0 = Pendente, 

    [Required]
    public decimal Preco { get; set; }
}
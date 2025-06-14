using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SistemaDeLogin.Models;

namespace SistemaDeLogin.Data;

public class SistemaContext : IdentityDbContext
{
    public SistemaContext(DbContextOptions<SistemaContext> options) : base(options) { }

    public DbSet<Clientes> Clientes { get; set; }
    public DbSet<Servico> Servicos { get; set; }
    public DbSet<Pecas> Pecas { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        //TABELA CLIENTE
        builder.Entity<Clientes>(
            entity =>
            {
                entity.HasKey(e => e.IdCliente);
                entity.Property(e => e.Nome).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Cpf).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Telefone).HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            }
        );

        //TABELA SERVICOS
        builder.Entity<Servico>(
        entity =>
        {
            entity.HasKey(e => e.IdServico);
            entity.Property(e => e.NomeAparelho).HasMaxLength(100).IsRequired();
            entity.Property(e => e.DefeitoInformado).HasMaxLength(500); // Remove .IsRequired()
            entity.Property(e => e.Diagnostico).HasMaxLength(500); // Remove .IsRequired()
            entity.Property(e => e.Status).IsRequired().HasDefaultValue(0);
            entity.Property(e => e.Valor).HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.DataDeEntrada).HasColumnType("date").IsRequired();
            entity.Property(e => e.PrevisaoDeEntrega).HasColumnType("date").IsRequired();
        }
        );

        //TABELA PEÇAS
        builder.Entity<Pecas>(
            entity =>
            {
                entity.HasKey(e => e.IdPeca);
                entity.Property(e => e.NomePeca).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Descricao).HasMaxLength(500);
                entity.Property(e => e.Preco).HasColumnType("decimal(10,2)").IsRequired();
            }
        );

        //RELAÇÃO
        builder.Entity<Servico>()
            .HasOne(e => e.Clientes)
            .WithMany(e => e.Servicos)
            .HasForeignKey(e => e.IdCliente);
    }
}
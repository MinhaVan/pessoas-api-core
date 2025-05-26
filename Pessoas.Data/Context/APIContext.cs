using Microsoft.EntityFrameworkCore;
using Pessoas.Core.Domain.Models;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Threading;
using Pessoas.Core.Data.Configurations;

namespace Pessoas.Core.Data.Context;

public class APIContext : DbContext
{
    public APIContext(DbContextOptions<APIContext> options) : base(options)
    { }

    public override int SaveChanges()
    {
        AtualizarDatas();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AtualizarDatas();
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AlunoConfiguration());
        modelBuilder.ApplyConfiguration(new AjusteAlunoRotaConfiguration());
        modelBuilder.ApplyConfiguration(new MotoristaConfiguration());

        base.OnModelCreating(modelBuilder);
    }
    private void AtualizarDatas()
    {
        var now = DateTime.UtcNow;
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Entity &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                ((Entity)entry.Entity).DataCriacao = now;
                ((Entity)entry.Entity).Status = Domain.Enums.StatusEntityEnum.Ativo;
            }
            ((Entity)entry.Entity).DataAlteracao = now;
        }
    }

    [DbFunction("unaccent", IsBuiltIn = true)]
    public static string Unaccent(string input) => throw new NotSupportedException();
    public DbSet<AjusteAlunoRota> AjusteAlunoRotas { get; set; }
    public DbSet<Aluno> Alunos { get; set; }
}
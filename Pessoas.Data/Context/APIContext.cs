using Microsoft.EntityFrameworkCore;
using Aluno.Core.Domain.Models;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Threading;
using Aluno.Core.Data.Configurations;

namespace Aluno.Core.Data.Context;

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
        modelBuilder.ApplyConfiguration(new AlunoRotaConfiguration());
        modelBuilder.ApplyConfiguration(new AlunoConfiguration());
        modelBuilder.ApplyConfiguration(new AjusteAlunoRotaConfiguration());

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

    public DbSet<AjusteAlunoRota> AjusteAlunoRotas { get; set; }
    public DbSet<Domain.Models.Aluno> Alunos { get; set; }
    public DbSet<AlunoRota> AlunoRotas { get; set; }
}
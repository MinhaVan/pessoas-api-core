using Aluno.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aluno.Core.Data.Configurations;

public class AlunoRotaConfiguration : IEntityTypeConfiguration<AlunoRota>
{
    public void Configure(EntityTypeBuilder<AlunoRota> modelBuilder)
    {
        modelBuilder.HasKey(x => new { x.AlunoId, x.RotaId });
        modelBuilder.ToTable("alunoRota");

        modelBuilder.HasOne(x => x.Aluno)
            .WithMany(y => y.AlunoRotas)
            .HasForeignKey(x => x.AlunoId);
    }
}
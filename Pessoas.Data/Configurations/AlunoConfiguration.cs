using Aluno.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aluno.Core.Data.Configurations;

public class AlunoConfiguration : IEntityTypeConfiguration<Domain.Models.Aluno>
{
    public void Configure(EntityTypeBuilder<Domain.Models.Aluno> modelBuilder)
    {
        modelBuilder.ConfigureBaseEntity();
        modelBuilder.ToTable("alunos");
    }
}

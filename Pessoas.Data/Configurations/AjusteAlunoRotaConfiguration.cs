using Pessoas.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pessoas.Core.Data.Configurations;

public class AjusteAlunoRotaConfiguration : IEntityTypeConfiguration<AjusteAlunoRota>
{
    public void Configure(EntityTypeBuilder<AjusteAlunoRota> modelBuilder)
    {
        modelBuilder.ConfigureBaseEntity();
        modelBuilder.ToTable("ajusteAlunoRota");
    }
}

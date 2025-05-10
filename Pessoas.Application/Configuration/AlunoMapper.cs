using AutoMapper;
using Pessoas.Core.Domain.ViewModels;
using Pessoas.Core.Domain.Models;
using Pessoas.Core.Domain.ViewModels.Aluno;

namespace Pessoas.Core.Service.Configurations;

public class AlunoMapper : Profile
{
    public AlunoMapper()
    {
        CreateMap<AlunoViewModel, Domain.Models.Aluno>().ReverseMap();
        CreateMap<AlunoAdicionarViewModel, Domain.Models.Aluno>().ReverseMap();
        CreateMap<AlunoDetalheViewModel, Domain.Models.Aluno>().ReverseMap();
    }
}
using AutoMapper;
using Aluno.Core.Domain.ViewModels;
using Aluno.Core.Domain.Models;
using Aluno.Core.Domain.ViewModels.Aluno;

namespace Aluno.Core.Service.Configurations;

public class AlunoMapper : Profile
{
    public AlunoMapper()
    {
        CreateMap<AlunoViewModel, Domain.Models.Aluno>().ReverseMap();
        CreateMap<AlunoAdicionarViewModel, Domain.Models.Aluno>().ReverseMap();
        CreateMap<AlunoDetalheViewModel, Domain.Models.Aluno>().ReverseMap();
    }
}
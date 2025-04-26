using AutoMapper;
using Aluno.Core.Domain.ViewModels;
using Aluno.Core.Domain.Models;

namespace Aluno.Core.Service.Configurations;

public class AlunoMapper : Profile
{
    public AlunoMapper()
    {
        CreateMap<AlunoViewModel, Aluno>().ReverseMap();
        CreateMap<AlunoAdicionarViewModel, Aluno>().ReverseMap();
        CreateMap<AlunoRotaViewModel, AlunoRota>().ReverseMap();
        CreateMap<AlunoDetalheViewModel, Aluno>().ReverseMap();
    }
}
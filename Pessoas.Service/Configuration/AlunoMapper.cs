using AutoMapper;
using Pessoas.Core.Domain.ViewModels;
using Pessoas.Core.Domain.Models;

namespace Pessoas.Core.Application.Configurations;

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
using AutoMapper;
using Pessoas.Core.Domain.ViewModels;
using Pessoas.Core.Domain.Models;

namespace Pessoas.Core.Application.Configurations;

public class MotoristaMapper : Profile
{
    public MotoristaMapper()
    {
        CreateMap<Motorista, MotoristaViewModel>().ReverseMap();
        CreateMap<Motorista, MotoristaAtualizarViewModel>().ReverseMap();
        CreateMap<Motorista, MotoristaRotaViewModel>().ReverseMap();
        CreateMap<Motorista, MotoristaNovoViewModel>().ReverseMap();
    }
}
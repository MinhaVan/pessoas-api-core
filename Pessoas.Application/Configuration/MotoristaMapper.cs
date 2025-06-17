using AutoMapper;
using Pessoas.Core.Domain.Models;
using Pessoas.Core.Domain.ViewModels;
using Pessoas.Core.Domain.ViewModels.Motorista;
using Pessoas.Domain.ViewModels.Motorista;

namespace Pessoas.Application.Configuration;

public class MotoristaMapper : Profile
{
    public MotoristaMapper()
    {
        CreateMap<Motorista, UsuarioNovoViewModel>().ReverseMap();
        CreateMap<Motorista, MotoristaViewModel>().ReverseMap();
        CreateMap<Motorista, MotoristaAtualizarViewModel>().ReverseMap();
        CreateMap<Motorista, MotoristaRotaViewModel>().ReverseMap();
        CreateMap<Motorista, MotoristaNovoViewModel>().ReverseMap();
    }
}
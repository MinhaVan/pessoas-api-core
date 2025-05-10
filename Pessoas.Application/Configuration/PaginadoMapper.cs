using AutoMapper;
using Pessoas.Core.Domain.ViewModels;
using Pessoas.Core.Domain.Models;

namespace Pessoas.Core.Service.Configurations;

public class PaginadoMapper : Profile
{
    public PaginadoMapper()
    {
        #region Paginado
        CreateMap(typeof(Paginado<>), typeof(PaginadoViewModel<>)).ReverseMap();
        #endregion
    }
}
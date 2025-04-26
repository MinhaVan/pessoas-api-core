using AutoMapper;
using Aluno.Core.Domain.ViewModels;
using Aluno.Core.Domain.Models;

namespace Aluno.Core.Service.Configurations;

public class PaginadoMapper : Profile
{
    public PaginadoMapper()
    {
        #region Paginado
        CreateMap(typeof(Paginado<>), typeof(PaginadoViewModel<>)).ReverseMap();
        #endregion
    }
}
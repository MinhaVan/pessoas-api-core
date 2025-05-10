using AutoMapper;
using Pessoas.Core.Domain.Models;
using Pessoas.Core.Domain.ViewModels.Rota;

namespace Pessoas.Core.Application.Configurations;

public class AjusteAlunoRotaMapper : Profile
{
    public AjusteAlunoRotaMapper()
    {
        #region RotaAjusteEndereco
        // CreateMap<AjusteAlunoRota, AjusteAlunoRota>().ReverseMap();
        #endregion
    }
}
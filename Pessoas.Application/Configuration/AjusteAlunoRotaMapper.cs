using AutoMapper;
using Pessoas.Core.Domain.Models;
using Pessoas.Core.Domain.ViewModels.Rota;

namespace Pessoas.Core.Service.Configurations;

public class AjusteAlunoRotaMapper : Profile
{
    public AjusteAlunoRotaMapper()
    {
        #region RotaAjusteEndereco
        // CreateMap<RotaAjusteEnderecoViewModel, AjusteAlunoRota>().ReverseMap();
        #endregion
    }
}
using AutoMapper;
using Aluno.Core.Domain.Models;
using Aluno.Core.Domain.ViewModels.Rota;

namespace Aluno.Core.Service.Configurations;

public class AjusteAlunoRotaMapper : Profile
{
    public AjusteAlunoRotaMapper()
    {
        #region RotaAjusteEndereco
        CreateMap<RotaAjusteEnderecoViewModel, AjusteAlunoRota>().ReverseMap();
        #endregion
    }
}
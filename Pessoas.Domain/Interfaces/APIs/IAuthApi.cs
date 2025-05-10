using System.Threading.Tasks;
using Pessoas.Core.Domain.ViewModels;
using Pessoas.Core.Domain.ViewModels.Motorista;
using Refit;

namespace Pessoas.Core.Domain.Interfaces.APIs;

public interface IAuthApi
{
    [Post("/v1/usuario")] Task<BaseResponse<UsuarioViewModel>> RegistrarAsync(UsuarioNovoViewModel user);
    [Put("/v1/usuario")] Task<BaseResponse<object>> AtualizarAsync(UsuarioAtualizarViewModel user);
}
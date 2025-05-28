using System.Threading.Tasks;
using Pessoas.Core.Domain.ViewModels;
using Pessoas.Core.Domain.ViewModels.Motorista;
using Refit;

namespace Pessoas.Core.Domain.Interfaces.APIs;

public interface IAuthApi
{
    [Post("/v1/Usuario")] Task<BaseResponse<UsuarioViewModel>> RegistrarAsync(UsuarioNovoViewModel user);
    [Put("/v1/Usuario")] Task<BaseResponse<object>> AtualizarAsync(UsuarioAtualizarViewModel user);
    [Get("/v1/Usuario/{usuarioId}")] Task<BaseResponse<UsuarioViewModel>> ObterUsuarioPorIdAsync([Header("Authorization")] string authorization, int usuarioId);
}
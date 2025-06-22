using System.Threading.Tasks;
using Pessoas.Core.Domain.ViewModels;
using Pessoas.Core.Domain.ViewModels.Motorista;
using Refit;

namespace Pessoas.Core.Domain.Interfaces.APIs;

public interface IAuthApi
{
    [Post("/Auth/v1/Usuario/Motorista")] Task<BaseResponse<UsuarioViewModel>> RegistrarAsync(UsuarioMotoristaNovoViewModel user);
    [Put("/Auth/v1/Usuario")] Task<BaseResponse<object>> AtualizarAsync([Header("Authorization")] string authorization, UsuarioAtualizarViewModel user);
    [Get("/Auth/v1/Usuario/{userId}")] Task<BaseResponse<UsuarioViewModel>> ObterUsuarioPorIdAsync([Header("Authorization")] string authorization, int userId, [Query] bool obterDadosMotorista = false, [Query] bool obterDadosEndereco = false);
}
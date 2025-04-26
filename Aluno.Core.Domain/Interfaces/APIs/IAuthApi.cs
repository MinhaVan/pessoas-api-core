using System.Threading.Tasks;
using Aluno.Core.Domain.ViewModels;
using Aluno.Core.Domain.ViewModels.Motorista;
using Refit;

namespace Aluno.Core.Domain.Interfaces.APIs;

public interface IAuthApi
{
    [Post("v1/usuario")] Task<BaseResponse<UsuarioViewModel>> RegistrarAsync(UsuarioNovoViewModel user);
    [Put("v1/usuario")] Task<BaseResponse<object>> AtualizarAsync(UsuarioAtualizarViewModel user);
}
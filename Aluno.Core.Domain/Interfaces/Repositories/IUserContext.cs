namespace Aluno.Core.Domain.Interfaces.Repository;

public interface IUserContext
{
    string Token { get; }
    int UserId { get; }
    int Empresa { get; }
}
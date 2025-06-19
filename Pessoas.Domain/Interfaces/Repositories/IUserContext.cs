namespace Pessoas.Core.Domain.Interfaces.Repository;

public interface IUserContext
{
    string Token { get; }
    int UserId { get; }
}
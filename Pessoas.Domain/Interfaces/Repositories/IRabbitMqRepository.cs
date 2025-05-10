using System.Threading.Tasks;

namespace Pessoas.Core.Domain.Interfaces.Repositories;

public interface IRabbitMqRepository
{
    Task PublishAsync<T>(string queue, T message);
}
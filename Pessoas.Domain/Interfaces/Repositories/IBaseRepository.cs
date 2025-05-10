using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Pessoas.Core.Domain.Models;

namespace Pessoas.Core.Domain.Interfaces.Repository;

public interface IBaseRepository<T> where T : Entity
{
    Task<T> ObterPorIdAsync(int id);
    Task<IEnumerable<T>> ObterTodosAsync();
    Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    Task<Paginado<T>> BuscarPaginadoAsync(int pagina, int tamanho, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    Task<T> BuscarUmAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    Task AdicionarAsync(T entity);
    Task AdicionarAsync(IEnumerable<T> entities);
    Task AtualizarAsync(T entity);
    Task AtualizarAsync(IEnumerable<T> entities);
    Task RemoverAsync(T entity);
    Task RemoverAsync(int id);
    IQueryable<T> AsQueryable();
}
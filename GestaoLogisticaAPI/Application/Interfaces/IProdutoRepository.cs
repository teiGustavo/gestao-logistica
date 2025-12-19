using GestaoLogisticaAPI.Domain.Entities;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IProdutoRepository {
    Task<IEnumerable<Produto>> GetAllAsync();
    Task<Produto?> GetAsync(int id);
    Task AddAsync(Produto entity);
    Task UpdateAsync(Produto entity);
    Task DeleteAsync(int id);
}
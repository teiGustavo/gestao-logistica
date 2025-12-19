using GestaoLogisticaAPI.Domain.Entities;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IEntregaProdutoRepository {
    Task<IEnumerable<EntregaProduto>> GetAllAsync();
    Task<EntregaProduto?> GetAsync(int id);
    Task AddAsync(EntregaProduto entity);
    Task UpdateAsync(EntregaProduto entity);
    Task DeleteAsync(int id);

}
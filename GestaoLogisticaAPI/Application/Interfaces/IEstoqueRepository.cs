using GestaoLogisticaAPI.Domain.Entities;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IEstoqueRepository {
    Task<IEnumerable<Estoque>> GetAllAsync();
    Task<Estoque?> GetAsync(int id);
    Task AddAsync(Estoque entity);
    Task UpdateAsync(Estoque entity);
    Task DeleteAsync(int id);
}
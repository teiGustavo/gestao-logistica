using GestaoLogisticaAPI.Domain.Entities;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IRastreamentoRepository {
    Task<IEnumerable<Rastreamento>> GetAllAsync();
    Task<Rastreamento?> GetAsync(int id);
    Task AddAsync(Rastreamento entity);
    Task UpdateAsync(Rastreamento entity);
    Task DeleteAsync(int id);

}
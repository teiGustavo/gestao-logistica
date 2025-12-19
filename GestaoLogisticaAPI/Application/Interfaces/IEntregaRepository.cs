using GestaoLogisticaAPI.Domain.Entities;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IEntregaRepository {
    Task<IEnumerable<Entrega>> GetAllAsync();
    Task<Entrega?> GetAsync(int id);
    Task AddAsync(Entrega entity);
    Task UpdateAsync(Entrega entity);
    Task DeleteAsync(int id);

}
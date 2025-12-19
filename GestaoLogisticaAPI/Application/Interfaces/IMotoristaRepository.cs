using GestaoLogisticaAPI.Domain.Entities;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IMotoristaRepository {
    Task<IEnumerable<Motorista>> GetAllAsync();
    Task<Motorista?> GetAsync(int id);
    Task AddAsync(Motorista entity);
    Task UpdateAsync(Motorista entity);
    Task DeleteAsync(int id);

}
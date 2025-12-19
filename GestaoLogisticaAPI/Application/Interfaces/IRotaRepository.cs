using GestaoLogisticaAPI.Domain.Entities;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IRotaRepository {
    Task<IEnumerable<Rota>> GetAllAsync();
    Task<Rota?> GetAsync(int id);
    Task AddAsync(Rota entity);
    Task UpdateAsync(Rota entity);
    Task DeleteAsync(int id);

}
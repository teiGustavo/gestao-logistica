using GestaoLogisticaAPI.Domain.Entities;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface ITransportadoraRepository {
    Task<IEnumerable<Transportadora>> GetAllAsync();
    Task<Transportadora?> GetAsync(int id);
    Task AddAsync(Transportadora entity);
    Task UpdateAsync(Transportadora entity);
    Task DeleteAsync(int id);

}
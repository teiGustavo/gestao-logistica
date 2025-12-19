using GestaoLogisticaAPI.Domain.Entities;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IClienteRepository {
    Task<IEnumerable<Cliente>> GetAllAsync();
    Task<Cliente?> GetAsync(int id);
    Task AddAsync(Cliente entity);
    Task UpdateAsync(Cliente entity);
    Task DeleteAsync(int id);

}
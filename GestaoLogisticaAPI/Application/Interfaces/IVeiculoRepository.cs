using GestaoLogisticaAPI.Domain.Entities;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IVeiculoRepository {
    Task<IEnumerable<Veiculo>> GetAllAsync();
    Task<Veiculo?> GetAsync(int id);
    Task AddAsync(Veiculo entity);
    Task UpdateAsync(Veiculo entity);
    Task DeleteAsync(int id);

}
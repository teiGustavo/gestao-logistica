using GestaoLogisticaAPI.Domain.Entities;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IArmazemRepository {
    Task<IEnumerable<Armazem>> GetAllAsync();
    Task<Armazem?> GetAsync(int id);
    Task AddAsync(Armazem entity);
    Task UpdateAsync(Armazem entity);
    Task DeleteAsync(int id);

}
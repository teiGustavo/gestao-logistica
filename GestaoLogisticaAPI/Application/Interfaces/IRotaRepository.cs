using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Application.DTOs;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IRotaRepository {
    Task<IEnumerable<RotaDto>> GetAllAsync();
    Task<RotaDto?> GetAsync(int id);
    Task AddAsync(Rota entity);
    Task UpdateAsync(Rota entity);
    Task DeleteAsync(int id);

}
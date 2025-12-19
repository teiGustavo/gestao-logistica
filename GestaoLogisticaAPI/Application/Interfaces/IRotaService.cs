using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IRotaService {
    Task<Result<IEnumerable<RotaDto>>> GetAllAsync();
    Task<Result<RotaDto>> GetAsync(int id);
    Task<Result<RotaDto>> CreateAsync(RotaDto dto);
    Task<Result> UpdateAsync(RotaDto dto);
    Task<Result> DeleteAsync(int id);

}
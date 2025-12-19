using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IMotoristaService {
    Task<Result<IEnumerable<MotoristaDto>>> GetAllAsync();
    Task<Result<MotoristaDto>> GetAsync(int id);
    Task<Result<MotoristaDto>> CreateAsync(MotoristaDto dto);
    Task<Result> UpdateAsync(MotoristaDto dto);
    Task<Result> DeleteAsync(int id);

}
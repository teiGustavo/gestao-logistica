using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IArmazemService {
    Task<Result<IEnumerable<ArmazemDto>>> GetAllAsync();
    Task<Result<ArmazemDto>> GetAsync(int id);
    Task<Result<ArmazemDto>> CreateAsync(ArmazemDto dto);
    Task<Result> UpdateAsync(ArmazemDto dto);
    Task<Result> DeleteAsync(int id);

}
using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IVeiculoService {
    Task<Result<IEnumerable<VeiculoDto>>> GetAllAsync();
    Task<Result<VeiculoDto>> GetAsync(int id);
    Task<Result<VeiculoDto>> CreateAsync(VeiculoDto dto);
    Task<Result> UpdateAsync(VeiculoDto dto);
    Task<Result> DeleteAsync(int id);

}
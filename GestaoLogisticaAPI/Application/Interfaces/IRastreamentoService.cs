using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IRastreamentoService {
    Task<Result<IEnumerable<RastreamentoDto>>> GetAllAsync();
    Task<Result<RastreamentoDto>> GetAsync(int id);
    Task<Result<RastreamentoDto>> CreateAsync(RastreamentoDto dto);
    Task<Result> UpdateAsync(RastreamentoDto dto);
    Task<Result> DeleteAsync(int id);

}
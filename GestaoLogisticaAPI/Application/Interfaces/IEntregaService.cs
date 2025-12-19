using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IEntregaService {
    Task<Result<IEnumerable<EntregaDto>>> GetAllAsync();
    Task<Result<EntregaDto>> GetAsync(int id);
    Task<Result<EntregaDto>> CreateAsync(EntregaDto dto);
    Task<Result> UpdateAsync(EntregaDto dto);
    Task<Result> DeleteAsync(int id);

}
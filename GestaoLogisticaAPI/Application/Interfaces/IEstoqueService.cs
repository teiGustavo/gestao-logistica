using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IEstoqueService {
    Task<Result<IEnumerable<EstoqueDto>>> GetAllAsync();
    Task<Result<EstoqueDto>> GetAsync(int id);
    Task<Result<EstoqueDto>> CreateAsync(EstoqueDto dto);
    Task<Result> UpdateAsync(EstoqueDto dto);
    Task<Result> DeleteAsync(int id);

}
using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IEntregaProdutoService {
    Task<Result<IEnumerable<EntregaProdutoDto>>> GetAllAsync();
    Task<Result<EntregaProdutoDto>> GetAsync(int id);
    Task<Result<EntregaProdutoDto>> CreateAsync(EntregaProdutoDto dto);
    Task<Result> UpdateAsync(EntregaProdutoDto dto);
    Task<Result> DeleteAsync(int id);

}
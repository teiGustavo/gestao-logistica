using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IProdutoService {
    Task<Result<IEnumerable<ProdutoDto>>> GetAllAsync();
    Task<Result<ProdutoDto>> GetAsync(int id);
    Task<Result<ProdutoDto>> CreateAsync(ProdutoDto dto);
    Task<Result> UpdateAsync(ProdutoDto dto);
    Task<Result> DeleteAsync(int id);

}
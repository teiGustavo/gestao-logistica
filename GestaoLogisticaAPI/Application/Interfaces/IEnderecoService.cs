using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IEnderecoService {
    Task<Result<IEnumerable<EnderecoDto>>> GetAllAsync();
    Task<Result<EnderecoDto>> GetAsync(int id);
    Task<Result<EnderecoDto>> CreateAsync(EnderecoDto dto);
    Task<Result> UpdateAsync(EnderecoDto dto);
    Task<Result> DeleteAsync(int id);

}
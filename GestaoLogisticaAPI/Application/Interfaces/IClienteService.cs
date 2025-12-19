using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IClienteService {
    Task<Result<IEnumerable<ClienteDto>>> GetAllAsync();
    Task<Result<ClienteDto>> GetAsync(int id);
    Task<Result<ClienteDto>> CreateAsync(ClienteDto dto);
    Task<Result> UpdateAsync(ClienteDto dto);
    Task<Result> DeleteAsync(int id);

}
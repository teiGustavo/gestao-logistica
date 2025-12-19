using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface ITransportadoraService {
    Task<Result<IEnumerable<TransportadoraDto>>> GetAllAsync();
    Task<Result<TransportadoraDto>> GetAsync(int id);
    Task<Result<TransportadoraDto>> CreateAsync(TransportadoraDto dto);
    Task<Result> UpdateAsync(TransportadoraDto dto);
    Task<Result> DeleteAsync(int id);

}
using AutoMapper;
using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;
using GestaoLogisticaAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Application.Services;

public class TransportadoraService(ITransportadoraRepository repo, IMapper mapper) : ITransportadoraService
{
    public async Task<Result<IEnumerable<TransportadoraDto>>> GetAllAsync()
    {
        return Result<IEnumerable<TransportadoraDto>>.Success(mapper.Map<IEnumerable<TransportadoraDto>>(await repo.GetAllAsync()));
    }

    public async Task<Result<TransportadoraDto>> GetAsync(int id)
    {
        var entity = await repo.GetAsync(id);
        if (entity == null)
            return Result<TransportadoraDto>.Failure("Transportadora não encontrada.");

        var dto = mapper.Map<TransportadoraDto>(entity);
        return Result<TransportadoraDto>.Success(dto);
    }

    public async Task<Result<TransportadoraDto>> CreateAsync(TransportadoraDto dto)
    {
        try
        {
            var entity = mapper.Map<Transportadora>(dto);
            entity.CriadoEm ??= DateTime.UtcNow;

            await repo.AddAsync(entity);

            var createdDto = mapper.Map<TransportadoraDto>(entity);
            return Result<TransportadoraDto>.Success(createdDto);
        }
        catch (DbUpdateException ex)
        {
            return Result<TransportadoraDto>.Failure("Erro ao salvar Transportadora: " + (ex.InnerException?.Message ?? ex.Message));
        }
        catch (Exception ex)
        {
            return Result<TransportadoraDto>.Failure("Erro inesperado: " + ex.Message);
        }
    }

    public async Task<Result> UpdateAsync(TransportadoraDto dto)
    {
        var existing = await repo.GetAsync(dto.CodTransportadora);
        if (existing == null)
            return Result.Failure("Transportadora não encontrada.");

        mapper.Map(dto, existing);

        await repo.UpdateAsync(existing);
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var existing = await repo.GetAsync(id);
        if (existing == null)
            return Result.Failure("Transportadora não encontrada.");

        await repo.DeleteAsync(id);
        return Result.Ok();
    }

}
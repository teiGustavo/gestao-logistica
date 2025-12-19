using AutoMapper;
using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;
using GestaoLogisticaAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Application.Services;

public class RastreamentoService(IRastreamentoRepository repo, IMapper mapper) : IRastreamentoService
{
    public async Task<Result<IEnumerable<RastreamentoDto>>> GetAllAsync()
    {
        return Result<IEnumerable<RastreamentoDto>>.Success(mapper.Map<IEnumerable<RastreamentoDto>>(await repo.GetAllAsync()));
    }

    public async Task<Result<RastreamentoDto>> GetAsync(int id)
    {
        var entity = await repo.GetAsync(id);
        if (entity == null)
            return Result<RastreamentoDto>.Failure("Rastreamento não encontrado.");

        var dto = mapper.Map<RastreamentoDto>(entity);
        return Result<RastreamentoDto>.Success(dto);
    }

    public async Task<Result<RastreamentoDto>> CreateAsync(RastreamentoDto dto)
    {
        try
        {
            var entity = mapper.Map<Rastreamento>(dto);

            await repo.AddAsync(entity);

            var createdDto = mapper.Map<RastreamentoDto>(entity);
            return Result<RastreamentoDto>.Success(createdDto);
        }
        catch (DbUpdateException ex)
        {
            return Result<RastreamentoDto>.Failure("Erro ao salvar Rastreamento: " + (ex.InnerException?.Message ?? ex.Message));
        }
        catch (Exception ex)
        {
            return Result<RastreamentoDto>.Failure("Erro inesperado: " + ex.Message);
        }
    }

    public async Task<Result> UpdateAsync(RastreamentoDto dto)
    {
        var existing = await repo.GetAsync(dto.CodRastreamento);
        if (existing == null)
            return Result.Failure("Rastreamento não encontrado.");

        mapper.Map(dto, existing);

        await repo.UpdateAsync(existing);
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var existing = await repo.GetAsync(id);
        if (existing == null)
            return Result.Failure("Rastreamento não encontrado.");

        await repo.DeleteAsync(id);
        return Result.Ok();
    }

}
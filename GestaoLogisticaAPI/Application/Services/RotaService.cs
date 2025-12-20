using AutoMapper;
using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;
using GestaoLogisticaAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Application.Services;

public class RotaService(IRotaRepository repo, IMapper mapper) : IRotaService
{
    public async Task<Result<IEnumerable<RotaDto>>> GetAllAsync()
    {
        return Result<IEnumerable<RotaDto>>.Success(await repo.GetAllAsync());
    }

    public async Task<Result<RotaDto>> GetAsync(int id)
    {
        var dto = await repo.GetAsync(id);
        if (dto == null)
            return Result<RotaDto>.Failure("Rota não encontrada.");

        return Result<RotaDto>.Success(dto);
    }

    public async Task<Result<RotaDto>> CreateAsync(RotaDto dto)
    {
        try
        {
            var entity = mapper.Map<Rota>(dto);
            entity.CriadoEm ??= DateTime.UtcNow;

            await repo.AddAsync(entity);

            var createdDto = mapper.Map<RotaDto>(entity);
            return Result<RotaDto>.Success(createdDto);
        }
        catch (DbUpdateException ex)
        {
            return Result<RotaDto>.Failure("Erro ao salvar Rota: " + (ex.InnerException?.Message ?? ex.Message));
        }
        catch (Exception ex)
        {
            return Result<RotaDto>.Failure("Erro inesperado: " + ex.Message);
        }
    }

    public async Task<Result> UpdateAsync(RotaDto dto)
    {
        var existingEntity = await repo.GetAsync(dto.CodRota);
        if (existingEntity == null)
            return Result.Failure("Rota não encontrada.");

        var existing = mapper.Map<Rota>(existingEntity);

        mapper.Map(dto, existing);

        await repo.UpdateAsync(existing);
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var existing = await repo.GetAsync(id);
        if (existing == null)
            return Result.Failure("Rota não encontrada.");

        await repo.DeleteAsync(id);
        return Result.Ok();
    }

}
using AutoMapper;
using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;
using GestaoLogisticaAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Application.Services;

public class EntregaService(IEntregaRepository repo, IMapper mapper) : IEntregaService
{
    public async Task<Result<IEnumerable<EntregaDto>>> GetAllAsync()
    {
        return Result<IEnumerable<EntregaDto>>.Success(mapper.Map<IEnumerable<EntregaDto>>(await repo.GetAllAsync()));
    }

    public async Task<Result<EntregaDto>> GetAsync(int id)
    {
        var entity = await repo.GetAsync(id);
        if (entity == null)
            return Result<EntregaDto>.Failure("Entrega não encontrada.");

        var dto = mapper.Map<EntregaDto>(entity);
        return Result<EntregaDto>.Success(dto);
    }

    public async Task<Result<EntregaDto>> CreateAsync(EntregaDto dto)
    {
        try
        {
            var entity = mapper.Map<Entrega>(dto);
            entity.CriadoEm ??= DateTime.UtcNow;

            await repo.AddAsync(entity);

            var createdDto = mapper.Map<EntregaDto>(entity);
            return Result<EntregaDto>.Success(createdDto);
        }
        catch (DbUpdateException ex)
        {
            return Result<EntregaDto>.Failure("Erro ao salvar Entrega: " + (ex.InnerException?.Message ?? ex.Message));
        }
        catch (Exception ex)
        {
            return Result<EntregaDto>.Failure("Erro inesperado: " + ex.Message);
        }
    }

    public async Task<Result> UpdateAsync(EntregaDto dto)
    {
        var existing = await repo.GetAsync(dto.CodEntrega);
        if (existing == null)
            return Result.Failure("Entrega não encontrada.");

        mapper.Map(dto, existing);

        await repo.UpdateAsync(existing);
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var existing = await repo.GetAsync(id);
        if (existing == null)
            return Result.Failure("Entrega não encontrada.");

        await repo.DeleteAsync(id);
        return Result.Ok();
    }

}
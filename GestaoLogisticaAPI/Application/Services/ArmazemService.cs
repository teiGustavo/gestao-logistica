using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Domain.Common;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace GestaoLogisticaAPI.Application.Services;

public class ArmazemService(IArmazemRepository repo, IMapper mapper) : IArmazemService
{
    public async Task<Result<IEnumerable<ArmazemDto>>> GetAllAsync()
    {
        return Result<IEnumerable<ArmazemDto>>.Success(mapper.Map<IEnumerable<ArmazemDto>>(await repo.GetAllAsync()));
    }

    public async Task<Result<ArmazemDto>> GetAsync(int id)
    {
        var entity = await repo.GetAsync(id);
        if (entity == null)
            return Result<ArmazemDto>.Failure("Armazém não encontrado.");

        var dto = mapper.Map<ArmazemDto>(entity);
        return Result<ArmazemDto>.Success(dto);
    }

    public async Task<Result<ArmazemDto>> CreateAsync(ArmazemDto dto)
    {
        try
        {
            var entity = mapper.Map<Armazem>(dto);
            entity.CriadoEm ??= DateTime.UtcNow;

            await repo.AddAsync(entity);

            var createdDto = mapper.Map<ArmazemDto>(entity);
            return Result<ArmazemDto>.Success(createdDto);
        }
        catch (DbUpdateException ex)
        {
            return Result<ArmazemDto>.Failure("Erro ao salvar Armazém: " +
                                              (ex.InnerException?.Message ?? ex.Message));
        }
        catch (Exception ex)
        {
            return Result<ArmazemDto>.Failure("Erro inesperado: " + ex.Message);
        }
    }

    public async Task<Result> UpdateAsync(ArmazemDto dto)
    {
        var existing = await repo.GetAsync(dto.CodArmazem);
        if (existing == null)
            return Result.Failure("Armazém não encontrado.");

        mapper.Map(dto, existing);

        await repo.UpdateAsync(existing);
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var existing = await repo.GetAsync(id);
        if (existing == null)
            return Result.Failure("Armazém não encontrado.");

        await repo.DeleteAsync(id);
        return Result.Ok();
    }

}
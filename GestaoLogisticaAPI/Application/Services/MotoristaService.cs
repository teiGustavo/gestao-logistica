using AutoMapper;
using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;
using GestaoLogisticaAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Application.Services;

public class MotoristaService(IMotoristaRepository repo, IMapper mapper) : IMotoristaService
{
    public async Task<Result<IEnumerable<MotoristaDto>>> GetAllAsync()
    {
        return Result<IEnumerable<MotoristaDto>>.Success(mapper.Map<IEnumerable<MotoristaDto>>(await repo.GetAllAsync()));
    }

    public async Task<Result<MotoristaDto>> GetAsync(int id)
    {
        var entity = await repo.GetAsync(id);
        if (entity == null)
            return Result<MotoristaDto>.Failure("Motorista não encontrado.");

        var dto = mapper.Map<MotoristaDto>(entity);
        return Result<MotoristaDto>.Success(dto);
    }

    public async Task<Result<MotoristaDto>> CreateAsync(MotoristaDto dto)
    {
        try
        {
            var entity = mapper.Map<Motorista>(dto);
            entity.CriadoEm ??= DateTime.UtcNow;

            await repo.AddAsync(entity);

            var createdDto = mapper.Map<MotoristaDto>(entity);
            return Result<MotoristaDto>.Success(createdDto);
        }
        catch (DbUpdateException ex)
        {
            return Result<MotoristaDto>.Failure("Erro ao salvar Motorista: " + (ex.InnerException?.Message ?? ex.Message));
        }
        catch (Exception ex)
        {
            return Result<MotoristaDto>.Failure("Erro inesperado: " + ex.Message);
        }
    }

    public async Task<Result> UpdateAsync(MotoristaDto dto)
    {
        var existing = await repo.GetAsync(dto.CodMotorista);
        if (existing == null)
            return Result.Failure("Motorista não encontrado.");

        mapper.Map(dto, existing);

        await repo.UpdateAsync(existing);
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var existing = await repo.GetAsync(id);
        if (existing == null)
            return Result.Failure("Motorista não encontrado.");

        await repo.DeleteAsync(id);
        return Result.Ok();
    }

}
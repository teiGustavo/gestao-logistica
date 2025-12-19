using AutoMapper;
using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;
using GestaoLogisticaAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Application.Services;

public class VeiculoService(IVeiculoRepository repo, IMapper mapper) : IVeiculoService
{
    public async Task<Result<IEnumerable<VeiculoDto>>> GetAllAsync()
    {
        return Result<IEnumerable<VeiculoDto>>.Success(mapper.Map<IEnumerable<VeiculoDto>>(await repo.GetAllAsync()));
    }

    public async Task<Result<VeiculoDto>> GetAsync(int id)
    {
        var entity = await repo.GetAsync(id);
        if (entity == null)
            return Result<VeiculoDto>.Failure("Veículo não encontrado.");

        var dto = mapper.Map<VeiculoDto>(entity);
        return Result<VeiculoDto>.Success(dto);
    }

    public async Task<Result<VeiculoDto>> CreateAsync(VeiculoDto dto)
    {
        try
        {
            var entity = mapper.Map<Veiculo>(dto);
            entity.CriadoEm ??= DateTime.UtcNow;

            await repo.AddAsync(entity);

            var createdDto = mapper.Map<VeiculoDto>(entity);
            return Result<VeiculoDto>.Success(createdDto);
        }
        catch (DbUpdateException ex)
        {
            return Result<VeiculoDto>.Failure("Erro ao salvar Veículo: " + (ex.InnerException?.Message ?? ex.Message));
        }
        catch (Exception ex)
        {
            return Result<VeiculoDto>.Failure("Erro inesperado: " + ex.Message);
        }
    }

    public async Task<Result> UpdateAsync(VeiculoDto dto)
    {
        var existing = await repo.GetAsync(dto.CodVeiculo);
        if (existing == null)
            return Result.Failure("Veículo não encontrado.");

        mapper.Map(dto, existing);

        await repo.UpdateAsync(existing);
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var existing = await repo.GetAsync(id);
        if (existing == null)
            return Result.Failure("Veículo não encontrado.");

        await repo.DeleteAsync(id);
        return Result.Ok();
    }

}
using AutoMapper;
using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;
using GestaoLogisticaAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Application.Services;

public class ClienteService(IClienteRepository repo, IMapper mapper) : IClienteService
{
    public async Task<Result<IEnumerable<ClienteDto>>> GetAllAsync()
    {
        return Result<IEnumerable<ClienteDto>>.Success(mapper.Map<IEnumerable<ClienteDto>>(await repo.GetAllAsync()));
    }

    public async Task<Result<ClienteDto>> GetAsync(int id)
    {
        var entity = await repo.GetAsync(id);
        if (entity == null)
            return Result<ClienteDto>.Failure("Cliente não encontrado.");

        var dto = mapper.Map<ClienteDto>(entity);
        return Result<ClienteDto>.Success(dto);
    }

    public async Task<Result<ClienteDto>> CreateAsync(ClienteDto dto)
    {
        try
        {
            var entity = mapper.Map<Cliente>(dto);
            entity.CriadoEm ??= DateTime.UtcNow;

            await repo.AddAsync(entity);

            var createdDto = mapper.Map<ClienteDto>(entity);
            return Result<ClienteDto>.Success(createdDto);
        }
        catch (DbUpdateException ex)
        {
            return Result<ClienteDto>.Failure("Erro ao salvar Cliente: " + (ex.InnerException?.Message ?? ex.Message));
        }
        catch (Exception ex)
        {
            return Result<ClienteDto>.Failure("Erro inesperado: " + ex.Message);
        }
    }

    public async Task<Result> UpdateAsync(ClienteDto dto)
    {
        var existing = await repo.GetAsync(dto.CodCliente);
        if (existing == null)
            return Result.Failure("Cliente não encontrado.");

        mapper.Map(dto, existing);

        await repo.UpdateAsync(existing);
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var existing = await repo.GetAsync(id);
        if (existing == null)
            return Result.Failure("Cliente não encontrado.");

        await repo.DeleteAsync(id);
        return Result.Ok();
    }

}
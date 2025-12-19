using AutoMapper;
using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;
using GestaoLogisticaAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Application.Services;

public class EstoqueService(IEstoqueRepository repo, IMapper mapper) : IEstoqueService
{
    public async Task<Result<IEnumerable<EstoqueDto>>> GetAllAsync()
    {
        return Result<IEnumerable<EstoqueDto>>.Success(mapper.Map<IEnumerable<EstoqueDto>>(await repo.GetAllAsync()));
    }

    public async Task<Result<EstoqueDto>> GetAsync(int id)
    {
        var entity = await repo.GetAsync(id);
        if (entity == null)
            return Result<EstoqueDto>.Failure("Estoque não encontrado.");

        var dto = mapper.Map<EstoqueDto>(entity);
        return Result<EstoqueDto>.Success(dto);
    }

    public async Task<Result<EstoqueDto>> CreateAsync(EstoqueDto dto)
    {
        try
        {
            var entity = mapper.Map<Estoque>(dto);

            await repo.AddAsync(entity);

            var createdDto = mapper.Map<EstoqueDto>(entity);
            return Result<EstoqueDto>.Success(createdDto);
        }
        catch (DbUpdateException ex)
        {
            return Result<EstoqueDto>.Failure("Erro ao salvar Estoque: " + (ex.InnerException?.Message ?? ex.Message));
        }
        catch (Exception ex)
        {
            return Result<EstoqueDto>.Failure("Erro inesperado: " + ex.Message);
        }
    }

    public async Task<Result> UpdateAsync(EstoqueDto dto)
    {
        var existing = await repo.GetAsync(dto.CodEstoque);
        if (existing == null)
            return Result.Failure("Estoque não encontrado.");

        mapper.Map(dto, existing);

        await repo.UpdateAsync(existing);
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var existing = await repo.GetAsync(id);
        if (existing == null)
            return Result.Failure("Estoque não encontrado.");

        await repo.DeleteAsync(id);
        return Result.Ok();
    }

}
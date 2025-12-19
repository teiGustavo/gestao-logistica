using AutoMapper;
using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;
using GestaoLogisticaAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Application.Services;

public class EntregaProdutoService(IEntregaProdutoRepository repo, IMapper mapper) : IEntregaProdutoService
{
    public async Task<Result<IEnumerable<EntregaProdutoDto>>> GetAllAsync()
    {
        return Result<IEnumerable<EntregaProdutoDto>>.Success(mapper.Map<IEnumerable<EntregaProdutoDto>>(await repo.GetAllAsync()));
    }

    public async Task<Result<EntregaProdutoDto>> GetAsync(int id)
    {
        var entity = await repo.GetAsync(id);
        if (entity == null)
            return Result<EntregaProdutoDto>.Failure("EntregaProduto não encontrado.");

        var dto = mapper.Map<EntregaProdutoDto>(entity);
        return Result<EntregaProdutoDto>.Success(dto);
    }

    public async Task<Result<EntregaProdutoDto>> CreateAsync(EntregaProdutoDto dto)
    {
        try
        {
            var entity = mapper.Map<EntregaProduto>(dto);
            entity.CriadoEm ??= DateTime.UtcNow;

            await repo.AddAsync(entity);

            var createdDto = mapper.Map<EntregaProdutoDto>(entity);
            return Result<EntregaProdutoDto>.Success(createdDto);
        }
        catch (DbUpdateException ex)
        {
            return Result<EntregaProdutoDto>.Failure("Erro ao salvar EntregaProduto: " + (ex.InnerException?.Message ?? ex.Message));
        }
        catch (Exception ex)
        {
            return Result<EntregaProdutoDto>.Failure("Erro inesperado: " + ex.Message);
        }
    }

    public async Task<Result> UpdateAsync(EntregaProdutoDto dto)
    {
        var existing = await repo.GetAsync(dto.CodEntregaProduto);
        if (existing == null)
            return Result.Failure("EntregaProduto não encontrado.");

        mapper.Map(dto, existing);

        await repo.UpdateAsync(existing);
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var existing = await repo.GetAsync(id);
        if (existing == null)
            return Result.Failure("EntregaProduto não encontrado.");

        await repo.DeleteAsync(id);
        return Result.Ok();
    }

}
using AutoMapper;
using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;
using GestaoLogisticaAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Application.Services;

public class ProdutoService(IProdutoRepository repo, IMapper mapper) : IProdutoService
{
    public async Task<Result<IEnumerable<ProdutoDto>>> GetAllAsync()
    {
        return Result<IEnumerable<ProdutoDto>>.Success(mapper.Map<IEnumerable<ProdutoDto>>(await repo.GetAllAsync()));
    }

    public async Task<Result<ProdutoDto>> GetAsync(int id)
    {
        var entity = await repo.GetAsync(id);
        if (entity == null)
            return Result<ProdutoDto>.Failure("Produto não encontrado.");

        var dto = mapper.Map<ProdutoDto>(entity);
        return Result<ProdutoDto>.Success(dto);
    }

    public async Task<Result<ProdutoDto>> CreateAsync(ProdutoDto dto)
    {
        try
        {
            var entity = mapper.Map<Produto>(dto);
            entity.CriadoEm ??= DateTime.UtcNow;

            await repo.AddAsync(entity);

            var createdDto = mapper.Map<ProdutoDto>(entity);
            return Result<ProdutoDto>.Success(createdDto);
        }
        catch (DbUpdateException ex)
        {
            return Result<ProdutoDto>.Failure("Erro ao salvar Produto: " + (ex.InnerException?.Message ?? ex.Message));
        }
        catch (Exception ex)
        {
            return Result<ProdutoDto>.Failure("Erro inesperado: " + ex.Message);
        }
    }

    public async Task<Result> UpdateAsync(ProdutoDto dto)
    {
        var existing = await repo.GetAsync(dto.CodProduto);
        if (existing == null)
            return Result.Failure("Produto não encontrado.");

        mapper.Map(dto, existing);

        await repo.UpdateAsync(existing);
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var existing = await repo.GetAsync(id);
        if (existing == null)
            return Result.Failure("Produto não encontrado.");

        await repo.DeleteAsync(id);
        return Result.Ok();
    }

}
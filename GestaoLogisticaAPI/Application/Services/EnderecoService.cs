using AutoMapper;
using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Common;
using GestaoLogisticaAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Application.Services;

public class EnderecoService(IEnderecoRepository repo, IMapper mapper) : IEnderecoService
{
    public async Task<Result<IEnumerable<EnderecoDto>>> GetAllAsync()
    {
        return Result<IEnumerable<EnderecoDto>>.Success(mapper.Map<IEnumerable<EnderecoDto>>(await repo.GetAllAsync()));
    }
    
    public async Task<Result<EnderecoDto>> GetAsync(int id)
    {
        var entity = await repo.GetAsync(id);
        if (entity == null)
            return Result<EnderecoDto>.Failure("Endereço não encontrado.");

        var dto = mapper.Map<EnderecoDto>(entity);
        return Result<EnderecoDto>.Success(dto);
    }

    public async Task<Result<EnderecoDto>> CreateAsync(EnderecoDto dto)
    {
        try
        {
            var entity = mapper.Map<Endereco>(dto);
            entity.CriadoEm ??= DateTime.UtcNow;

            await repo.AddAsync(entity);

            var createdDto = mapper.Map<EnderecoDto>(entity);
            return Result<EnderecoDto>.Success(createdDto);
        }
        catch (DbUpdateException ex)
        {
            return Result<EnderecoDto>.Failure("Erro ao salvar Endereço: " +
                                              (ex.InnerException?.Message ?? ex.Message));
        }
        catch (Exception ex)
        {
            return Result<EnderecoDto>.Failure("Erro inesperado: " + ex.Message);
        }
    }

    public async Task<Result> UpdateAsync(EnderecoDto dto)
    {
        var existing = await repo.GetAsync(dto.CodEndereco);
        if (existing == null)
            return Result.Failure("Endereço não encontrado.");

        mapper.Map(dto, existing);

        await repo.UpdateAsync(existing);
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var existing = await repo.GetAsync(id);
        if (existing == null)
            return Result.Failure("Endereço não encontrado.");

        await repo.DeleteAsync(id);
        return Result.Ok();
    }

}
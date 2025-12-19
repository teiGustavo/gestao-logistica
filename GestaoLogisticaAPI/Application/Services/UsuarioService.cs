using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Application.DTOs;
using AutoMapper;
using GestaoLogisticaAPI.Domain.Common;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Domain.Enums;

namespace GestaoLogisticaAPI.Application.Services;

public class UsuarioService(IUsuarioRepository repo, IMapper mapper) : IUsuarioService
{
    public async Task<Result<IEnumerable<UsuarioDto>>> GetAllAsync()
    {
        var usuarios = await repo.GetAllAsync();
        var mapped = usuarios.Select(mapper.Map<UsuarioDto>).ToList();
        return Result<IEnumerable<UsuarioDto>>.Success(mapped);
    }

    public async Task<Result<UsuarioDto>> GetAsync(int id)
    {
        var usuario = await repo.GetAsync(id);
        return usuario == null 
            ? Result<UsuarioDto>.Failure("Usuário não encontrado.") 
            : Result<UsuarioDto>.Success(mapper.Map<UsuarioDto>(usuario));
    }

    public async Task<Result<UsuarioDto>> CreateAsync(CadastroUsuarioDto dto)
    {
        if (string.IsNullOrEmpty(dto.Apelido) || string.IsNullOrEmpty(dto.Senha))
            return Result<UsuarioDto>.Failure("Apelido e senha são obrigatórios.");
            
        var exists = await repo.GetByApelidoAsync(dto.Apelido);
            
        if (exists != null)
            return Result<UsuarioDto>.Failure("Apelido já existe.");
            
        dto.Role ??= RoleUsuarioEnum.Operador.ToString();
        dto.Ativo ??= true;
        dto.CriadoEm ??= DateTime.UtcNow;
        
        var entity = mapper.Map<Usuario>(dto);
        var created = await repo.CreateAsync(entity);
        return Result<UsuarioDto>.Success(mapper.Map<UsuarioDto>(created));
    }
        
    public async Task<Result> UpdateAsync(UsuarioDto dto)
    {
        var existing = await repo.GetAsync(dto.CodUsuario);
            
        if (existing == null)
            return Result.Failure("Usuário não encontrado.");
            
        mapper.Map(dto, existing);
        await repo.UpdateAsync(existing);
        return Result.Ok();
    }
        
    public async Task<Result> DeleteAsync(int id)
    {
        var existing = await repo.GetAsync(id);
        if (existing == null)
            return Result.Failure("Usuário não encontrado.");

        await repo.DeleteAsync(id);
        return Result.Ok();
    }

    public async Task<Result<Usuario?>> LoginAsync(string apelido, string senha)
    {
        var usuario = await repo.LoginAsync(apelido, senha);
            
        return usuario == null
            ? Result<Usuario?>.Failure("Credenciais inválidas.") 
            : Result<Usuario?>.Success(usuario);
    }
}
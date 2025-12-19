using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Domain.Common;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IUsuarioService {
    Task<Result<IEnumerable<UsuarioDto>>> GetAllAsync();
        
    Task<Result<UsuarioDto>> GetAsync(int id);
        
    Task<Result<UsuarioDto>> CreateAsync(CadastroUsuarioDto dto);
        
    Task<Result> UpdateAsync(UsuarioDto dto);
        
    Task<Result> DeleteAsync(int id);

    Task<Result<Usuario?>> LoginAsync(string apelido, string senha);
}
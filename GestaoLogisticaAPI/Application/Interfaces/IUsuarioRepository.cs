using GestaoLogisticaAPI.Domain.Entities;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<IEnumerable<Usuario>> GetAllAsync();
        
    Task<Usuario?> GetAsync(int id);
        
    Task UpdateAsync(Usuario entity);
        
    Task DeleteAsync(int id);

    Task<Usuario?> GetByApelidoAsync(string apelido);
        
    Task<Usuario> CreateAsync(Usuario user);
        
    Task<Usuario?> LoginAsync(string apelido, string senha);


}
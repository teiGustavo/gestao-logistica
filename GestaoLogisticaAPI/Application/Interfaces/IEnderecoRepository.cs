using GestaoLogisticaAPI.Domain.Entities;

namespace GestaoLogisticaAPI.Application.Interfaces;

public interface IEnderecoRepository
{
    Task<IEnumerable<Endereco>> GetAllAsync();
    Task<Endereco?> GetAsync(int id);
    Task AddAsync(Endereco entity);
    Task UpdateAsync(Endereco entity);
    Task DeleteAsync(int id);

}
using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Infrastructure.Repositories;

public class EnderecoRepository(AppDbContext context) : IEnderecoRepository
{
    public async Task<IEnumerable<Endereco>> GetAllAsync()
    {
        return await context.Endereco
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Endereco?> GetAsync(int id)
    {
        return await context.Endereco
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.CodEndereco == id);
    }

    public async Task AddAsync(Endereco entity)
    {
        await context.Endereco.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Endereco entity)
    {
        context.Endereco.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var e = await context.Endereco.FindAsync(id);
        if (e != null)
        {
            context.Endereco.Remove(e);
            await context.SaveChangesAsync();
        }
    }
}
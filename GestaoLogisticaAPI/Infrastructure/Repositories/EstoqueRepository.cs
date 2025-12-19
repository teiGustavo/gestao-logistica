using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Infrastructure.Repositories;

public class EstoqueRepository(AppDbContext context) : IEstoqueRepository
{
    public async Task<IEnumerable<Estoque>> GetAllAsync()
    {
        return await context.Estoque
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Estoque?> GetAsync(int id)
    {
        return await context.Estoque
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.CodEstoque == id);
    }

    public async Task AddAsync(Estoque entity)
    {
        await context.Estoque.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Estoque entity)
    {
        context.Estoque.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var e = await context.Estoque.FindAsync(id);
        if (e != null)
        {
            context.Estoque.Remove(e);
            await context.SaveChangesAsync();
        }
    }
}
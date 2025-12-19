using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Infrastructure.Repositories;

public class EntregaProdutoRepository(AppDbContext context) : IEntregaProdutoRepository
{
    public async Task<IEnumerable<EntregaProduto>> GetAllAsync()
    {
        return await context.EntregaProduto
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<EntregaProduto?> GetAsync(int id)
    {
        return await context.EntregaProduto
            .AsNoTracking()
            .FirstOrDefaultAsync(ep => ep.CodEntregaProduto == id);
    }

    public async Task AddAsync(EntregaProduto entity)
    {
        await context.EntregaProduto.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(EntregaProduto entity)
    {
        context.EntregaProduto.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var e = await context.EntregaProduto.FindAsync(id);
        if (e != null)
        {
            context.EntregaProduto.Remove(e);
            await context.SaveChangesAsync();
        }
    }
}
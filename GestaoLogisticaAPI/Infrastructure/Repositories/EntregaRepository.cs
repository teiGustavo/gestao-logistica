using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Infrastructure.Repositories;

public class EntregaRepository(AppDbContext context) : IEntregaRepository
{
    public async Task<IEnumerable<Entrega>> GetAllAsync()
    {
        return await context.Entrega
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Entrega?> GetAsync(int id)
    {
        return await context.Entrega
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.CodEntrega == id);
    }

    public async Task AddAsync(Entrega entity)
    {
        await context.Entrega.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Entrega entity)
    {
        context.Entrega.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var e = await context.Entrega.FindAsync(id);
        if (e != null)
        {
            context.Entrega.Remove(e);
            await context.SaveChangesAsync();
        }
    }
}
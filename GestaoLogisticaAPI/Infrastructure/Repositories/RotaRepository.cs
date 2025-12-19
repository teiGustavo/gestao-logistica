using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Infrastructure.Repositories;

public class RotaRepository(AppDbContext context) : IRotaRepository
{
    public async Task<IEnumerable<Rota>> GetAllAsync()
    {
        return await context.Rota
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Rota?> GetAsync(int id)
    {
        return await context.Rota
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.CodRota == id);
    }

    public async Task AddAsync(Rota entity)
    {
        await context.Rota.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Rota entity)
    {
        context.Rota.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var e = await context.Rota.FindAsync(id);
        if (e != null)
        {
            context.Rota.Remove(e);
            await context.SaveChangesAsync();
        }
    }
}
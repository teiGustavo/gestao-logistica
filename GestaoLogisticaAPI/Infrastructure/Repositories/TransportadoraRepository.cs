using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Infrastructure.Repositories;

public class TransportadoraRepository(AppDbContext context) : ITransportadoraRepository
{
    public async Task<IEnumerable<Transportadora>> GetAllAsync()
    {
        return await context.Transportadora
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Transportadora?> GetAsync(int id)
    {
        return await context.Transportadora
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.CodTransportadora == id);
    }

    public async Task AddAsync(Transportadora entity)
    {
        await context.Transportadora.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Transportadora entity)
    {
        context.Transportadora.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var e = await context.Transportadora.FindAsync(id);
        if (e != null)
        {
            context.Transportadora.Remove(e);
            await context.SaveChangesAsync();
        }
    }
}
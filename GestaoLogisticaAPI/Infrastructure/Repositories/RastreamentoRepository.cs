using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Infrastructure.Repositories;

public class RastreamentoRepository(AppDbContext context) : IRastreamentoRepository
{
    public async Task<IEnumerable<Rastreamento>> GetAllAsync()
    {
        return await context.Rastreamento
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Rastreamento?> GetAsync(int id)
    {
        return await context.Rastreamento
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.CodRastreamento == id);
    }

    public async Task AddAsync(Rastreamento entity)
    {
        await context.Rastreamento.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Rastreamento entity)
    {
        context.Rastreamento.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var e = await context.Rastreamento.FindAsync(id);
        if (e != null)
        {
            context.Rastreamento.Remove(e);
            await context.SaveChangesAsync();
        }
    }
}
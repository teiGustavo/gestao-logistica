using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Infrastructure.Repositories;

public class MotoristaRepository(AppDbContext context) : IMotoristaRepository
{
    public async Task<IEnumerable<Motorista>> GetAllAsync()
    {
        return await context.Motorista
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Motorista?> GetAsync(int id)
    {
        return await context.Motorista
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.CodMotorista == id);
    }

    public async Task AddAsync(Motorista entity)
    {
        await context.Motorista.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Motorista entity)
    {
        context.Motorista.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var e = await context.Motorista.FindAsync(id);
        if (e != null)
        {
            context.Motorista.Remove(e);
            await context.SaveChangesAsync();
        }
    }
}
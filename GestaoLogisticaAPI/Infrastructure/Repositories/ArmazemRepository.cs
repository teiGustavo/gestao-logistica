using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Infrastructure.Repositories;

public class ArmazemRepository(AppDbContext context) : IArmazemRepository
{
    public async Task<IEnumerable<Armazem>> GetAllAsync()
    {
        return await context.Armazem
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Armazem?> GetAsync(int id)
    {
        return await context.Armazem
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.CodArmazem == id);
    }

    public async Task AddAsync(Armazem entity)
    {
        await context.Armazem.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Armazem entity)
    {
        context.Armazem.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var e = await context.Armazem.FindAsync(id);
        if (e != null)
        {
            context.Armazem.Remove(e);
            await context.SaveChangesAsync();
        }
    }
}
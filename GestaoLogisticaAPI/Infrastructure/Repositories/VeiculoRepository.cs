using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Infrastructure.Repositories;

public class VeiculoRepository(AppDbContext context) : IVeiculoRepository
{
    public async Task<IEnumerable<Veiculo>> GetAllAsync()
    {
        return await context.Veiculo
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Veiculo?> GetAsync(int id)
    {
        return await context.Veiculo
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.CodVeiculo == id);
    }

    public async Task AddAsync(Veiculo entity)
    {
        await context.Veiculo.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Veiculo entity)
    {
        context.Veiculo.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var e = await context.Veiculo.FindAsync(id);
        if (e != null)
        {
            context.Veiculo.Remove(e);
            await context.SaveChangesAsync();
        }
    }
}
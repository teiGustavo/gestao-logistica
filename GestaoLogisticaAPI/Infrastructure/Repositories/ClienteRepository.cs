using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Infrastructure.Repositories;

public class ClienteRepository(AppDbContext context) : IClienteRepository
{
    public async Task<IEnumerable<Cliente>> GetAllAsync()
    {
        return await context.Cliente
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Cliente?> GetAsync(int id)
    {
        return await context.Cliente
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CodCliente == id);
    }

    public async Task AddAsync(Cliente entity)
    {
        await context.Cliente.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Cliente entity)
    {
        context.Cliente.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var e = await context.Cliente.FindAsync(id);
        if (e != null)
        {
            context.Cliente.Remove(e);
            await context.SaveChangesAsync();
        }
    }
}
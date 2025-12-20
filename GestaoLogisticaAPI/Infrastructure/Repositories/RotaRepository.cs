using AutoMapper;
using AutoMapper.QueryableExtensions;
using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Infrastructure.Repositories;

public class RotaRepository(AppDbContext context, IMapper mapper) : IRotaRepository
{
    public async Task<IEnumerable<RotaDto>> GetAllAsync()
    {
        return await context.Rota
            .AsNoTracking()
            .ProjectTo<RotaDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<RotaDto?> GetAsync(int id)
    {
        return await context.Rota
            .AsNoTracking()
            .Where(r => r.CodRota == id)
            .ProjectTo<RotaDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
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
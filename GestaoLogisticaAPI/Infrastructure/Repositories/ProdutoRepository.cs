using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Infrastructure.Repositories;

public class ProdutoRepository(AppDbContext context) : IProdutoRepository
{
    public async Task<IEnumerable<Produto>> GetAllAsync()
    {
        return await context.Produto
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Produto?> GetAsync(int id)
    {
        return await context.Produto
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.CodProduto == id);
    }

    public async Task AddAsync(Produto entity)
    {
        await context.Produto.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Produto entity)
    {
        context.Produto.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var e = await context.Produto.FindAsync(id);
        if (e != null)
        {
            context.Produto.Remove(e);
            await context.SaveChangesAsync();
        }
    }
}
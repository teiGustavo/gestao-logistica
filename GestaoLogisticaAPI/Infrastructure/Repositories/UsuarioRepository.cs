using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoLogisticaAPI.Infrastructure.Repositories;

public class UsuarioRepository(AppDbContext context) : IUsuarioRepository
{
    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        return await context.Usuario.AsNoTracking().ToListAsync();
    }

    public async Task<Usuario?> GetAsync(int id)
    {
        return await context.Usuario.FindAsync(id);
    }

    public async Task<Usuario> CreateAsync(Usuario user)
    {
        if (!user.Ativo) user.Ativo = true;

        context.Usuario.Add(user);
        await context.SaveChangesAsync();

        // Recarrega para garantir que o Id e outros valores gerados pelo DB estejam presentes
        await context.Entry(user).ReloadAsync();
        return user;
    }

    public async Task UpdateAsync(Usuario entity)
    {
        context.Usuario.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var e = await context.Usuario.FindAsync(id);

        if (e != null)
        {
            context.Usuario.Remove(e);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Usuario?> GetByApelidoAsync(string apelido)
    {
        if (string.IsNullOrWhiteSpace(apelido)) return null;
        var normalized = apelido.Trim().ToLowerInvariant();

        return await context.Usuario
            .AsNoTracking()
            .FirstOrDefaultAsync(u => 
                u.Apelido.Equals(normalized, StringComparison.CurrentCultureIgnoreCase)
            );
    }

    public async Task<Usuario?> LoginAsync(string apelido, string senha)
    {
        if (string.IsNullOrWhiteSpace(apelido) || string.IsNullOrWhiteSpace(senha))
            return null;

        var normalized = apelido.Trim().ToLowerInvariant();

        var user = await context.Usuario
            .FirstOrDefaultAsync(u => 
                u.Apelido.Equals(normalized, StringComparison.CurrentCultureIgnoreCase) && u.Ativo
            );

        if (user == null) return null;

        // comparar hashes se usar hashing
        return user.Senha == senha ? user : null;
    }
}
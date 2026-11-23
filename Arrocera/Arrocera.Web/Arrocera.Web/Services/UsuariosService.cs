using Aplicada1.Core;
using Arrocera.Web.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Arrocera.Web.Services;

public class UsuariosService(IDbContextFactory<ApplicationDbContext> contextFactory)
{
    public async Task<Result<ApplicationUser?>> Buscar(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return Result.Success<ApplicationUser?>(null);

        await using var contexto = await contextFactory.CreateDbContextAsync();
        var usuario = await contexto.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

        return Result.Success<ApplicationUser?>(usuario);
    }

    public async Task<Result<List<ApplicationUser>>> GetList(Expression<Func<ApplicationUser, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var lista = await contexto.Users
            .Where(criterio)
            .AsNoTracking()
            .OrderByDescending(u => u.FechaCreacion)
            .ToListAsync();

        return Result.Success(lista);
    }

    public async Task<Result<bool>> Modificar(ApplicationUser usuario)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(usuario);
        var modifico = await contexto.SaveChangesAsync() > 0;
        return Result.Success(modifico);
    }

    public async Task<Result<bool>> Eliminar(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return Result.Success(false);

        await using var contexto = await contextFactory.CreateDbContextAsync();
        var eliminado = await contexto.Users
            .Where(u => u.Id == id)
            .ExecuteDeleteAsync() > 0;

        return Result.Success(eliminado);
    }
}
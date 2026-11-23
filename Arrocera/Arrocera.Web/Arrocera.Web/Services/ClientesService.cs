using Aplicada1.Core;
using Arrocera.Web.Data;
using Arrocera.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Arrocera.Web.Services;

public class ClientesService(IDbContextFactory<ApplicationDbContext> contextFactory) : IServiceResult<Clientes, int>
{
    public async Task<Result> Guardar(Clientes cliente)
    {
        cliente.Nombre = cliente.Nombre.Trim();
        cliente.Rnc = cliente.Rnc.Trim();
        cliente.Telefono = string.IsNullOrWhiteSpace(cliente.Telefono) ? null : cliente.Telefono.Trim();

        if (!await Existe(cliente.ClienteId))
        {
            if (await ExisteRnc(cliente.Rnc))
                return Result.Failure(new Error("RNC Duplicado", "Ya existe un cliente registrado con ese RNC"));

            var inserto = await Insertar(cliente);
            return Result.Success(inserto);
        }
        else
        {
            if (await ExisteRnc(cliente.Rnc, cliente.ClienteId))
                return Result.Failure(new Error("RNC Duplicado", "Ya existe otro cliente registrado con ese RNC"));

            var modifico = await Modificar(cliente);
            return Result.Success(modifico);
        }
    }

    private async Task<bool> Existe(int? clienteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Clientes
            .AnyAsync(c => c.ClienteId == clienteId);
    }

    private async Task<bool> ExisteRnc(string rnc, int? clienteIdExcluir = null)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Clientes
            .AnyAsync(c => c.Rnc == rnc && (!clienteIdExcluir.HasValue || c.ClienteId != clienteIdExcluir.Value));
    }

    private async Task<bool> Insertar(Clientes cliente)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        cliente.CreatedAt = DateTime.UtcNow;
        cliente.UpdatedAt = DateTime.UtcNow;

        contexto.Clientes.Add(cliente);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Clientes cliente)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        cliente.UpdatedAt = DateTime.UtcNow;

        contexto.Update(cliente);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<Clientes?>> Buscar(int clienteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Clientes
            .FirstOrDefaultAsync(c => c.ClienteId == clienteId);
    }

    public async Task<Result<List<Clientes>>> GetList(Expression<Func<Clientes, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Clientes
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Result> Eliminar(int clienteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var eliminado = await contexto.Clientes
            .Where(c => c.ClienteId == clienteId)
            .ExecuteDeleteAsync() > 0;
        return Result.Success(eliminado);
    }
}

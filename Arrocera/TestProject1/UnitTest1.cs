using Arrocera.Web.Data;
using Arrocera.Web.Models;
using Arrocera.Web.Services;
using Microsoft.EntityFrameworkCore;

namespace TestProject1;

public class UnitTest1
{
    private sealed class SimpleDbContextFactory(DbContextOptions<ApplicationDbContext> options)
        : IDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext() => new(options);
    }

    [Fact]
    public async Task GuardarCliente_InsertaYBuscaCorrectamente()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var service = new ClientesService(new SimpleDbContextFactory(options));

        var cliente = new Clientes
        {
            Nombre = "Arrocera Duarte",
            Rnc = "130123456",
            Telefono = "809-555-1234"
        };

        var guardado = await service.Guardar(cliente);
        var encontrado = await service.Buscar(cliente.ClienteId);

        Assert.True(guardado.IsSuccess);
        Assert.NotNull(encontrado.Value);
        Assert.Equal("Arrocera Duarte", encontrado.Value!.Nombre);
    }
}
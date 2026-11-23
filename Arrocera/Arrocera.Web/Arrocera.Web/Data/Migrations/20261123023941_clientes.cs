using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arrocera.Web.Migrations;

/// <inheritdoc />
public partial class _20261123023941_clientes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Clientes",
            columns: table => new
            {
                ClienteId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Rnc = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Activo = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Clientes", x => x.ClienteId);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Clientes_Rnc",
            table: "Clientes",
            column: "Rnc",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Clientes");
    }
}

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Arrocera.Web.Models;

[Index(nameof(Rnc), IsUnique = true)]
public class Clientes
{
    [Key]
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Rnc { get; set; } = null!;

    [Phone(ErrorMessage = "Ingrese un número de teléfono válido")]
    public string? Telefono { get; set; }

    public bool Activo { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
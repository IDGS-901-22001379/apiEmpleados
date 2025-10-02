namespace Empleados.Api.Models;

public class EmpleadoDto
{
    public string Nombre { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Puesto { get; set; } = default!;
    public decimal Salario { get; set; }
}

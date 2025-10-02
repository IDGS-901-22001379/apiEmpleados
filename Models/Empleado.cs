namespace Empleados.Api.Models;

public class Empleado
{
    public int Id_Empleado { get; set; }
    public string Nombre { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Puesto { get; set; } = default!;
    public decimal Salario { get; set; }
    public string Estatus { get; set; } = default!;
}

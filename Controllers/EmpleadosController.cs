using Empleados.Api.Data;
using Empleados.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Empleados.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmpleadosController : ControllerBase
{
    private readonly EmpleadoData _data;
    public EmpleadosController(EmpleadoData data) => _data = data;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Empleado>>> Listar([FromQuery] int soloActivos = 1)
        => Ok(await _data.ListarAsync(soloActivos == 1));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Empleado>> Obtener(int id)
    {
        var emp = await _data.ObtenerAsync(id);
        return emp is null ? NotFound() : Ok(emp);
    }

    [HttpPost]
    public async Task<ActionResult> Crear([FromBody] EmpleadoDto dto)
    {
        var id = await _data.CrearAsync(dto);
        return CreatedAtAction(nameof(Obtener), new { id }, new { id_empleado = id });
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Actualizar(int id, [FromBody] EmpleadoDto dto)
    {
        var filas = await _data.ActualizarAsync(id, dto);
        return filas > 0 ? NoContent() : NotFound();
    }

    [HttpPatch("{id:int}/estatus")]
    public async Task<ActionResult> CambiarEstatus(int id, [FromQuery] string estatus)
    {
        estatus = estatus?.ToUpperInvariant() ?? "";
        if (estatus != "ACTIVO" && estatus != "INACTIVO")
            return BadRequest("Estatus inválido. Usa ACTIVO o INACTIVO.");

        var filas = await _data.CambiarEstatusAsync(id, estatus);
        return filas > 0 ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public ActionResult EliminarNoPermitido()
        => StatusCode(StatusCodes.Status405MethodNotAllowed, "Eliminación física no permitida");
}

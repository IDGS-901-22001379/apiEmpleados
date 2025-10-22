using System.Data;
using Dapper;
using Empleados.Api.Models;
using MySqlConnector;

namespace Empleados.Api.Data;

public class EmpleadoData
{
    private readonly string _connString;
    public EmpleadoData(IConfiguration config) => _connString = config.GetConnectionString("MySql")!;
    private IDbConnection Open() => new MySqlConnection(_connString);

    public async Task<IEnumerable<Empleado>> ListarAsync(bool soloActivos = true)
    {
        using var db = Open();
        return await db.QueryAsync<Empleado>(
            "sp_empleado_listar",
            new { p_solo_activos = soloActivos ? 1 : 0 },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<Empleado?> ObtenerAsync(int id)
    {
        using var db = Open();
        return await db.QueryFirstOrDefaultAsync<Empleado>(
            "sp_empleado_obtener",
            new { p_id = id },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<int> CrearAsync(EmpleadoDto dto)
    {
        using var db = Open();
        return await db.ExecuteScalarAsync<int>(
            "sp_empleado_insertar",
            new { p_nombre = dto.Nombre, p_email = dto.Email, p_puesto = dto.Puesto, p_salario = dto.Salario },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<int> ActualizarAsync(int id, EmpleadoDto dto)
    {
        using var db = Open();
        return await db.ExecuteScalarAsync<int>(
            "sp_empleado_actualizar",
            new { p_id = id, p_nombre = dto.Nombre, p_email = dto.Email, p_puesto = dto.Puesto, p_salario = dto.Salario },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<int> CambiarEstatusAsync(int id, string estatus)
    {
        using var db = Open();
        return await db.ExecuteScalarAsync<int>(
            "sp_empleado_cambiar_estatus",
            new { p_id = id, p_estatus = estatus },
            commandType: CommandType.StoredProcedure
        );
    }
}
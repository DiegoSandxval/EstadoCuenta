using Microsoft.AspNetCore.Mvc;
using EstadoCuentaAPI.Models;
using EstadoCuentaAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EstadoCuentaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimientosController : ControllerBase
    {
        private readonly IMovimientoRepository _movimientoRepository;
        private readonly ITarjetaRepository _tarjetaRepository;

        // Inyección de dependencias
        public MovimientosController(IMovimientoRepository movimientoRepository, ITarjetaRepository tarjetaRepository)
        {
            _movimientoRepository = movimientoRepository;
            _tarjetaRepository = tarjetaRepository;
        }

        [HttpPost("compra")]
        public async Task<IActionResult> RegistrarCompra([FromBody] Movimiento movimiento)
        {
            // Validación inicial del objeto
            if (movimiento == null)
            {
                return BadRequest("El cuerpo de la solicitud no puede ser nulo.");
            }

            // Validar que IdTarjeta, FechaMovimiento y Monto sean válidos
            if (movimiento.IdTarjeta <= 0 || movimiento.Monto <= 0 || movimiento.FechaMovimiento == DateTime.MinValue)
            {
                return BadRequest("IdTarjeta, Fecha de Compra y Monto son obligatorios.");
            }

            try
            {
                // Configurar campos automáticos si no vienen desde el cliente
                movimiento.TipoMovimiento = "Cargo"; // Siempre será compra

                // Guardar el movimiento
                _movimientoRepository.Add(movimiento);
                await _movimientoRepository.SaveChangesAsync();

                // Respuesta exitosa
                return Ok(new
                {
                    Mensaje = "Compra registrada correctamente.",
                    MovimientoId = movimiento.IdMovimiento,
                    IdTarjeta = movimiento.IdTarjeta,
                    FechaMovimiento = movimiento.FechaMovimiento,
                    Descripcion = movimiento.Descripcion,
                    Monto = movimiento.Monto
                });
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, $"Error al guardar en la base de datos: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error inesperado: {ex.Message}");
            }
        }


        [HttpGet("Tarjeta/{id}")]
        public async Task<IActionResult> GetMovimientosPorTarjeta(int id)
        {
            try
            {
                var movimientos = await _movimientoRepository.GetByTarjetaIdAsync(id);
                if (movimientos == null || !movimientos.Any())
                {
                    return NotFound("No se encontraron movimientos para esta tarjeta.");
                }

                return Ok(movimientos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los movimientos: {ex.Message}");
            }
        }

        [HttpPost("pago")]
        public async Task<IActionResult> RegistrarPago([FromBody] Movimiento movimiento)
        {
            if (movimiento == null || movimiento.IdTarjeta <= 0 || movimiento.Monto <= 0)
            {
                return BadRequest("Datos inválidos. Verifica los campos enviados.");
            }

            try
            {
                var tarjeta = await _tarjetaRepository.GetByIdAsync(movimiento.IdTarjeta);
                if (tarjeta == null)
                {
                    return NotFound("Tarjeta no encontrada.");
                }

                movimiento.TipoMovimiento = "Abono";
                _movimientoRepository.Add(movimiento);

                tarjeta.SaldoUtilizado -= movimiento.Monto;
                tarjeta.SaldoDisponible += movimiento.Monto;

                await _movimientoRepository.SaveChangesAsync();
                return Ok("Pago registrado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al guardar en la base de datos: {ex.Message}");
            }

        }

    }
}

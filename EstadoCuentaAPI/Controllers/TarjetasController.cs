using EstadoCuentaAPI.Data;
using EstadoCuentaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstadoCuentaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarjetasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TarjetasController(AppDbContext context)
        {
            _context = context;
        }

        // Obtiene todas las tarjetas o filtra por búsqueda
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarjeta>>> GetTarjetas([FromQuery] string? search)
        {
            var query = _context.Tarjetas.AsQueryable();

            // Filtra las tarjetas si se proporciona un término de búsqueda
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t => t.NumeroTarjeta.Contains(search) || t.NombreTitular.Contains(search));
            }

            var tarjetas = await query.ToListAsync();

            // Si no hay resultados, retorna NoContent
            if (!tarjetas.Any())
                return NoContent();

            return Ok(tarjetas);
        }

        // Obtiene una tarjeta específica por su ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Tarjeta>> GetTarjetaById(int id)
        {
            var tarjeta = await _context.Tarjetas.FindAsync(id);
            if (tarjeta == null)
                return NotFound();

            return Ok(tarjeta);
        }

        // Obtiene los movimientos de una tarjeta específica
        [HttpGet("{id}/movimientos")]
        public async Task<ActionResult<IEnumerable<Movimiento>>> GetMovimientosByTarjeta(int id)
        {
            var movimientos = await _context.Movimientos
                .Where(m => m.IdTarjeta == id)
                .ToListAsync();

            return Ok(movimientos);
        }
    }
}

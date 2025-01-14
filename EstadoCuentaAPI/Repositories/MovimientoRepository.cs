using EstadoCuentaAPI.Data;
using EstadoCuentaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EstadoCuentaAPI.Repositories
{
    public class MovimientoRepository : IMovimientoRepository
    {
        private readonly AppDbContext _context;

        public MovimientoRepository(AppDbContext context)
        {
            _context = context;
        }
   
        public async Task<Movimiento> GetByIdAsync(int id)
        {
            return await _context.Movimientos
                .Include(m => m.Tarjeta) // Incluye la tarjeta asociada si es necesario
                .FirstOrDefaultAsync(m => m.IdMovimiento == id);
        }

        public async Task<IEnumerable<Movimiento>> GetAllAsync()
        {
            return await _context.Movimientos
                .Include(m => m.Tarjeta) // Incluye las tarjetas si es necesario
                .ToListAsync();
        }

        public async Task<IEnumerable<Movimiento>> GetByTarjetaIdAsync(int tarjetaId)
        {
            return await _context.Movimientos
                .Where(m => m.IdTarjeta == tarjetaId)
                .ToListAsync();
        }

        public void Add(Movimiento movimiento)
        {
            _context.Movimientos.Add(movimiento);
        }

        public void Remove(Movimiento movimiento)
        {
            _context.Movimientos.Remove(movimiento);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}


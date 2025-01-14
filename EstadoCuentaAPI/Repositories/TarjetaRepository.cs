using EstadoCuentaAPI.Data;
using EstadoCuentaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EstadoCuentaAPI.Repositories
{
    public class TarjetaRepository : ITarjetaRepository
    {
        private readonly AppDbContext _context;

        public TarjetaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tarjeta>> GetAllAsync()
        {
            return await _context.Tarjetas.ToListAsync();
        }

        public async Task<Tarjeta> GetByIdAsync(int id)
        {
            return await _context.Tarjetas
                .Include(t => t.Movimientos) // Incluye movimientos si es necesario
                .FirstOrDefaultAsync(t => t.IdTarjeta == id);
        }

        public async Task AddAsync(Tarjeta tarjeta)
        {
            await _context.Tarjetas.AddAsync(tarjeta);
        }

        public void Update(Tarjeta tarjeta)
        {
            _context.Tarjetas.Update(tarjeta);
        }

        public void Remove(Tarjeta tarjeta)
        {
            _context.Tarjetas.Remove(tarjeta);
        }
    }
}

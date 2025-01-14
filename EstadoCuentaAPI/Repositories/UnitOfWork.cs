using EstadoCuentaAPI.Data;
using EstadoCuentaAPI.Repositories;

namespace EstadoCuentaAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public ITarjetaRepository Tarjetas { get; }
        public IMovimientoRepository Movimientos { get; }

        public UnitOfWork(AppDbContext context, ITarjetaRepository tarjetaRepository, IMovimientoRepository movimientoRepository)
        {
            _context = context;
            Tarjetas = tarjetaRepository;
            Movimientos = movimientoRepository;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

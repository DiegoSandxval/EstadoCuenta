using EstadoCuentaAPI.Models;

namespace EstadoCuentaAPI.Repositories
{
    public interface IMovimientoRepository
    {
        Task<Movimiento> GetByIdAsync(int id); // Obtener un movimiento por ID.
        Task<IEnumerable<Movimiento>> GetAllAsync(); // Obtener todos los movimientos.
        Task<IEnumerable<Movimiento>> GetByTarjetaIdAsync(int tarjetaId); // Obtener movimientos de una tarjeta.
        void Add(Movimiento movimiento); // Agregar un movimiento.
        void Remove(Movimiento movimiento); // Eliminar un movimiento.
        Task SaveChangesAsync();
    }
}
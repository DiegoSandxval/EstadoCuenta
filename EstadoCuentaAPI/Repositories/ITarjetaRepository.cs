using EstadoCuentaAPI.Models;

namespace EstadoCuentaAPI.Repositories
{
    public interface ITarjetaRepository
    {
        Task<IEnumerable<Tarjeta>> GetAllAsync();
        Task<Tarjeta> GetByIdAsync(int id);
        Task AddAsync(Tarjeta tarjeta);
        void Update(Tarjeta tarjeta);
        void Remove(Tarjeta tarjeta);
    }
}

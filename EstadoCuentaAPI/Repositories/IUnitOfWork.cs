using EstadoCuentaAPI.Repositories;

namespace EstadoCuentaAPI.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        ITarjetaRepository Tarjetas { get; }
        IMovimientoRepository Movimientos { get; }
        Task<int> CompleteAsync();
    }

}

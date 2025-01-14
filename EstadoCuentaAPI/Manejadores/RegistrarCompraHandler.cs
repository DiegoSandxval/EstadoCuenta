using EstadoCuentaAPI.Models;
using EstadoCuentaAPI.Repository;
using MediatR;

namespace EstadoCuentaAPI.Manejadores
{
    public class RegistrarCompraHandler : IRequestHandler<RegistrarCompraCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarCompraHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(RegistrarCompraCommand request, CancellationToken cancellationToken)
        {
            var tarjeta = await _unitOfWork.Tarjetas.GetByIdAsync(request.IdTarjeta);
            if (tarjeta == null) return false;

            tarjeta.SaldoUtilizado += request.Monto;
            tarjeta.SaldoDisponible -= request.Monto;

            _unitOfWork.Movimientos.Add(new Movimiento
            {
                IdTarjeta = request.IdTarjeta,
                Monto = request.Monto,
                Descripcion = request.Descripcion,
                FechaMovimiento = DateTime.Now,
                TipoMovimiento = "Cargo"
            });

            await _unitOfWork.CompleteAsync();
            return true;
        }
    }

}

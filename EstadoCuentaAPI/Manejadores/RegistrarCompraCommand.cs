

using MediatR;

namespace EstadoCuentaAPI.Manejadores
{
    public record RegistrarCompraCommand(int IdTarjeta, decimal Monto, string Descripcion) : IRequest<bool>;

}

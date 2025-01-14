namespace EstadoCuentaAPI.Validators
{
    using FluentValidation;
    using EstadoCuentaAPI.Models;
    public class MovimientoValidator : AbstractValidator<Movimiento>
    {
        public MovimientoValidator()
        {
            RuleFor(x => x.Monto).GreaterThan(0).WithMessage("El monto debe ser mayor a 0.");
            RuleFor(x => x.Descripcion).NotEmpty().WithMessage("La descripción es obligatoria.");
        }
    }
}

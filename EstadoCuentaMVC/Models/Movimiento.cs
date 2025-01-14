using System.ComponentModel.DataAnnotations;

namespace EstadoCuentaMVC.Models
{
    public class Movimiento
    {
        public int IdMovimiento { get; set; } // Será generado por la base de datos, no necesitas enviarlo desde el frontend

        [Required(ErrorMessage = "El ID de la tarjeta es obligatorio.")]
        public int IdTarjeta { get; set; }

        [Required(ErrorMessage = "La fecha del movimiento es obligatoria.")]
        public DateTime FechaMovimiento { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
        public decimal Monto { get; set; }

        public string? TipoMovimiento { get; set; } // Será configurado como "Compra" desde el controlador

        [StringLength(255, ErrorMessage = "La descripción no puede superar los 255 caracteres.")]
        public string? Descripcion { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EstadoCuentaAPI.Models
{
    public class Movimiento
    {
        [Key]
        public int IdMovimiento { get; set; } // Clave primaria

        [Required]
        public int IdTarjeta { get; set; } // Clave foránea hacia Tarjeta

        [Required]
        public DateTime FechaMovimiento { get; set; }

        [Required]
        public decimal Monto { get; set; }

        [StringLength(50)]
        public string? TipoMovimiento { get; set; } // "Compra" o "Abono"

        [StringLength(255)]
        public string? Descripcion { get; set; }

        // Relación con Tarjeta
        [ForeignKey("IdTarjeta")]

        [JsonIgnore]
        public Tarjeta? Tarjeta { get; set; }

    }
}

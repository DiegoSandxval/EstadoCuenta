using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EstadoCuentaAPI.Models
{
    public class Tarjeta
    {
        [Key]
        public int IdTarjeta { get; set; } // Clave primaria

        [Required]
        [StringLength(16)]
        public string NumeroTarjeta { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreTitular { get; set; }

        public decimal SaldoDisponible { get; set; }
        public decimal SaldoUtilizado { get; set; }
        public decimal LimiteCredito { get; set; }

        public DateTime FechaCreacion { get; set; }

        // Relación con Movimientos
        public ICollection<Movimiento> Movimientos { get; set; }

    }
}

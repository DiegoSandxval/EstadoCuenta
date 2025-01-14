namespace EstadoCuentaMVC.Models
{
    public class Tarjeta
    {
        public int IdTarjeta { get; set; }
        public string NumeroTarjeta { get; set; }
        public string NombreTitular { get; set; }
        public decimal SaldoDisponible { get; set; }
        public decimal SaldoUtilizado { get; set; }
        public decimal LimiteCredito { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Propiedades calculadas para facilitar su uso
        public string Nombre => NombreTitular?.Split(' ')[0] ?? string.Empty;
        public string Apellido => NombreTitular?.Split(' ').Last() ?? string.Empty;
    }
}

namespace EstadoCuentaAPI.Dtos
{
    public class TarjetaDTO
    {
        public int IdTarjeta { get; set; }
        public string NumeroTarjeta { get; set; }
        public string NombreTitular { get; set; }
        public decimal SaldoDisponible { get; set; }
        public decimal SaldoUtilizado { get; set; }

    }
}

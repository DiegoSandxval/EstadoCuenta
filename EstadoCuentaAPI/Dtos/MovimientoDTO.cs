namespace EstadoCuentaAPI.Dtos
{
    public class MovimientoDTO
    {
        public int IdMovimiento { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public decimal Monto { get; set; }
        public string TipoMovimiento { get; set; }
        public string Descripcion { get; set; }
    }
}

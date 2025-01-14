using EstadoCuentaMVC.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EstadoCuentaMVC.Pages.Tarjetas
{
    public class EstadoCuentaModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public EstadoCuentaModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        // Propiedades del Modelo
        public Tarjeta Tarjeta { get; set; }
        public List<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
        public decimal TotalComprasMesActual { get; set; }
        public decimal TotalComprasMesAnterior { get; set; }
        public decimal InteresBonificable { get; set; }
        public decimal CuotaMinimaPagar { get; set; }
        public decimal MontoTotalPagar { get; set; }
        public decimal MontoTotalConIntereses { get; set; }

        public async Task OnGetAsync(int id)
        {
            try
            {
                var apiUrl = _configuration["ApiBaseUrl"];
                var client = _httpClientFactory.CreateClient();

                // Obtiene la tarjeta por ID
                Tarjeta = await client.GetFromJsonAsync<Tarjeta>($"{apiUrl}/Tarjetas/{id}");

                // Obtiene los movimientos asociados a la tarjeta
                Movimientos = await client.GetFromJsonAsync<List<Movimiento>>($"{apiUrl}/Tarjetas/{id}/movimientos");

                if (Tarjeta != null && Movimientos != null)
                {
                    // Calcular totales de compras del mes actual y anterior
                    TotalComprasMesActual = Math.Max(0, Movimientos
                        .Where(m => m.FechaMovimiento.Month == DateTime.Now.Month)
                        .Sum(m => m.Monto));

                    TotalComprasMesAnterior = Math.Max(0, Movimientos
                        .Where(m => m.FechaMovimiento.Month == DateTime.Now.AddMonths(-1).Month)
                        .Sum(m => m.Monto));

                    // Si no viene directamente de la API, calcula el saldo actual
                    if (Tarjeta.SaldoUtilizado == 0)
                    {
                        Tarjeta.SaldoUtilizado = Math.Max(0, Movimientos.Sum(m => m.Monto));
                    }

                    // Calcular Interés Bonificable
                    InteresBonificable = Math.Max(0, Tarjeta.SaldoUtilizado * 0.25m);

                    // Calcular Cuota Mínima a Pagar
                    CuotaMinimaPagar = Math.Max(0, Tarjeta.SaldoUtilizado * 0.05m);

                    // Calcular Monto Total a Pagar
                    MontoTotalPagar = Math.Max(0, Tarjeta.SaldoUtilizado);

                    // Calcular Pago de Contado con Intereses
                    MontoTotalConIntereses = Math.Max(0, MontoTotalPagar + InteresBonificable);
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}

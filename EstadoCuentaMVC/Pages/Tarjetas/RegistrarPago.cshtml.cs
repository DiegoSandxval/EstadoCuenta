using EstadoCuentaMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace EstadoCuentaMVC.Pages.Tarjetas
{
    public class RegistrarPagoModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public RegistrarPagoModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public Tarjeta Tarjeta { get; set; }

        [BindProperty]
        public Movimiento Movimiento { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var apiUrl = _configuration["ApiBaseUrl"];
            var client = _httpClientFactory.CreateClient();

            try
            {
                var response = await client.GetAsync($"{apiUrl}/Tarjetas/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "No se pudo obtener la tarjeta.";
                    return RedirectToPage("./Index");
                }

                Tarjeta = await response.Content.ReadFromJsonAsync<Tarjeta>();

                if (Tarjeta == null)
                {
                    TempData["ErrorMessage"] = "Tarjeta no encontrada.";
                    return RedirectToPage("./Index");
                }

                // Inicializar el movimiento
                Movimiento = new Movimiento
                {
                    IdTarjeta = id,
                    FechaMovimiento = DateTime.Now
                };

                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar la tarjeta: {ex.Message}";
                return RedirectToPage("./Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var apiUrl = _configuration["ApiBaseUrl"];
            var client = _httpClientFactory.CreateClient();

            if (!ModelState.IsValid)
            {
                // Si hay errores en el modelo, recargar la tarjeta
                Tarjeta = await client.GetFromJsonAsync<Tarjeta>($"{apiUrl}/Tarjetas/{Movimiento.IdTarjeta}");
                return Page();
            }

            try
            {
                // Configurar el tipo de movimiento como "Abono"
                Movimiento.TipoMovimiento = "Abono";

                // Enviar el movimiento a la API
                var response = await client.PostAsJsonAsync($"{apiUrl}/Movimientos/pago", Movimiento);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Error al registrar el pago: {errorContent}";
                    return Page();
                }

                TempData["SuccessMessage"] = "Pago registrado exitosamente.";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ocurrió un error: {ex.Message}";
                return Page();
            }
        }
    }
}

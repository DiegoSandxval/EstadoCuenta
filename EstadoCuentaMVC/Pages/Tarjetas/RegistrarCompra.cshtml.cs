using EstadoCuentaMVC.Models; // Modelo de la tarjeta y movimiento
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;


namespace EstadoCuentaMVC.Pages.Tarjetas
{
    public class RegistrarCompraModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public RegistrarCompraModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
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

            // Verificar si la tarjeta existe
            var response = await client.GetAsync($"{apiUrl}/Tarjetas/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "No se encontró la tarjeta especificada.";
                return RedirectToPage("./Index");
            }

            Tarjeta = await response.Content.ReadFromJsonAsync<Tarjeta>();

            if (Tarjeta == null)
            {
                TempData["ErrorMessage"] = "No se encontraron datos válidos para esta tarjeta.";
                return RedirectToPage("./Index");
            }

            // Inicializar el movimiento con el IdTarjeta
            Movimiento = new Movimiento
            {
                IdTarjeta = id, // Aquí asignamos el ID de la tarjeta seleccionada
                FechaMovimiento = DateTime.Now
            };
            Debug.WriteLine($"JSON Enviado: {JsonSerializer.Serialize(Movimiento)}");
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var apiUrl = _configuration["ApiBaseUrl"];
            var client = _httpClientFactory.CreateClient();

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Datos inválidos. Por favor, verifica los campos.";
                return Page();
            }

            // Configurar el tipo de movimiento como "Compra"
            Movimiento.TipoMovimiento = "Compra";

            // Imprimir los valores de Movimiento antes de enviarlo
            Console.WriteLine($"JSON Enviado: {JsonSerializer.Serialize(Movimiento)}");

            var response = await client.PostAsJsonAsync($"{apiUrl}/Movimientos/compra", Movimiento);
            Debug.WriteLine($"JSON Enviado: {JsonSerializer.Serialize(Movimiento)}");

            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"JSON Enviado: {JsonSerializer.Serialize(Movimiento)}");
                var errorDetails = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Error al registrar la compra: {errorDetails}";
                return Page();
            }

            TempData["SuccessMessage"] = "Compra registrada exitosamente.";
            return RedirectToPage("./Index");
        }


    }

}
 
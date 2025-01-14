using EstadoCuentaMVC.Models; // Modelo de la tarjeta
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace EstadoCuentaMVC.Pages.Tarjetas
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public IndexModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public List<Tarjeta> Tarjetas { get; set; } = new List<Tarjeta>();

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10; // Resultados por página

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1; // Página actual

        public int TotalResults { get; set; } = 30; /// Total de resultados
        public int TotalPages => (int)Math.Ceiling((double)TotalResults / PageSize);// Total de páginas

        public async Task OnGetAsync()
        {
            var apiUrl = _configuration["ApiBaseUrl"];
            var client = _httpClientFactory.CreateClient();

            // Construir el endpoint
            var endpoint = $"/Tarjetas?search={SearchQuery}&page={CurrentPage}&pageSize={PageSize}";
            CurrentPage = CurrentPage < 1 ? 1 : CurrentPage;
            // Llamar a la API
            var response = await client.GetAsync($"{apiUrl}{endpoint}");
            if (response.IsSuccessStatusCode)
            {
                // Leer las tarjetas desde el cuerpo de la respuesta
                Tarjetas = await response.Content.ReadFromJsonAsync<List<Tarjeta>>();

                // Leer el total de resultados desde un encabezado (o ajusta según tu API)
                if (response.Headers.TryGetValues("X-Total-Results", out var totalResults))
                {
                    TotalResults = int.Parse(totalResults.First());
                }

            
            }
            else
            {
                throw new Exception($"Error en la API: {response.StatusCode}");
            }
        }
    }
}

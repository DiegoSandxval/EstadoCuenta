using EstadoCuentaMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using System.Net.Http.Json;

namespace EstadoCuentaMVC.Pages.Tarjetas
{
    public class TransaccionesModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public TransaccionesModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public Tarjeta Tarjeta { get; set; }
        public List<Movimiento> Transacciones { get; set; } = new List<Movimiento>();
        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var apiUrl = _configuration["ApiBaseUrl"];
                var client = _httpClientFactory.CreateClient();

                // Obtener la tarjeta por ID
                Tarjeta = await client.GetFromJsonAsync<Tarjeta>($"{apiUrl}/Tarjetas/{id}");
                if (Tarjeta == null)
                {
                    TempData["ErrorMessage"] = "La tarjeta no existe.";
                    return RedirectToPage("./Index");
                }

                // Obtener los movimientos asociados a la tarjeta
                Transacciones = await client.GetFromJsonAsync<List<Movimiento>>($"{apiUrl}/Movimientos/Tarjeta/{id}");
                if (Transacciones == null || Transacciones.Count == 0)
                {
                    TempData["InfoMessage"] = "No se encontraron transacciones para esta tarjeta.";
                }

                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar las transacciones: {ex.Message}";
                return RedirectToPage("./Index");
            }
        }
        public async Task<IActionResult> OnPostExportToExcelAsync(int id)
        {
            try
            {
                var apiUrl = _configuration["ApiBaseUrl"];
                var client = _httpClientFactory.CreateClient();

                // Obtener los movimientos asociados a la tarjeta
                var transacciones = await client.GetFromJsonAsync<List<Movimiento>>($"{apiUrl}/Movimientos/Tarjeta/{id}");
                if (transacciones == null || transacciones.Count == 0)
                {
                    TempData["InfoMessage"] = "No se encontraron transacciones para esta tarjeta.";
                    return RedirectToPage("./Index");
                }

                // Configurar el contexto de licencia para EPPlus
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                // Crear el archivo Excel
                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add("Transacciones");

                // Encabezados
                worksheet.Cells[1, 1].Value = "ID Movimiento";
                worksheet.Cells[1, 2].Value = "Fecha";
                worksheet.Cells[1, 3].Value = "Monto";
                worksheet.Cells[1, 4].Value = "Tipo";
                worksheet.Cells[1, 5].Value = "Descripción";

                // Contenido
                for (int i = 0; i < transacciones.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = transacciones[i].IdMovimiento;
                    worksheet.Cells[i + 2, 2].Value = transacciones[i].FechaMovimiento;
                    worksheet.Cells[i + 2, 3].Value = transacciones[i].Monto;
                    worksheet.Cells[i + 2, 4].Value = transacciones[i].TipoMovimiento;
                    worksheet.Cells[i + 2, 5].Value = transacciones[i].Descripcion;
                }

                worksheet.Cells.AutoFitColumns();

                var fileName = $"Transacciones_Tarjeta_{id}.xlsx";
                var fileContent = package.GetAsByteArray();

                // Retornar el archivo como respuesta
                return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al exportar las transacciones: {ex.Message}";
                return RedirectToPage("./Index");
            }
        }


    }
}

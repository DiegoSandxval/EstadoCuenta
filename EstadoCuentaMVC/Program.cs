using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

// Habilitar Razor Pages
builder.Services.AddRazorPages();

// Configurar IHttpClientFactory
builder.Services.AddHttpClient();

// Configurar appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var app = builder.Build();
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.MapGet("/", () => Results.Redirect("/Tarjetas"));

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();

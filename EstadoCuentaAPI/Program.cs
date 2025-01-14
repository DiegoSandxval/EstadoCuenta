using EstadoCuentaAPI.Data;
using EstadoCuentaAPI.Middleware;
using EstadoCuentaAPI.Repositories;
using EstadoCuentaAPI.Repository;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configuraci�n de CORS
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(corsBuilder =>
            {
                corsBuilder.WithOrigins("https://localhost:7114") // URL del frontend MVC
                           .AllowAnyHeader()
                           .AllowAnyMethod();
            });
        });

        // Configuraci�n de servicios
        builder.Services.AddControllers(); // Habilitar controladores
        builder.Services.AddEndpointsApiExplorer(); // Explorador de endpoints
        builder.Services.AddSwaggerGen(); // Swagger para la documentaci�n

        // Configuraci�n de AutoMapper
        builder.Services.AddAutoMapper(typeof(Program));

        // Configuraci�n del DbContext y repositorios
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<ITarjetaRepository, TarjetaRepository>();
        builder.Services.AddScoped<IMovimientoRepository, MovimientoRepository>();

        var app = builder.Build();

        // Middleware CORS
        app.UseCors();

        // Configuraci�n de Swagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Middleware para manejo global de excepciones
        app.UseMiddleware<GlobalExceptionMiddleware>();

        // Middleware de redirecci�n a HTTPS
        app.UseHttpsRedirection();

        // Middleware de autorizaci�n
        app.UseAuthorization();

        // Mapeo de controladores
        app.MapControllers();

        // Ejecutar la aplicaci�n
        app.Run();
    }
}

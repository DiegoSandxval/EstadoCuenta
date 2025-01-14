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

        // Configuración de CORS
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(corsBuilder =>
            {
                corsBuilder.WithOrigins("https://localhost:7114") // URL del frontend MVC
                           .AllowAnyHeader()
                           .AllowAnyMethod();
            });
        });

        // Configuración de servicios
        builder.Services.AddControllers(); // Habilitar controladores
        builder.Services.AddEndpointsApiExplorer(); // Explorador de endpoints
        builder.Services.AddSwaggerGen(); // Swagger para la documentación

        // Configuración de AutoMapper
        builder.Services.AddAutoMapper(typeof(Program));

        // Configuración del DbContext y repositorios
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<ITarjetaRepository, TarjetaRepository>();
        builder.Services.AddScoped<IMovimientoRepository, MovimientoRepository>();

        var app = builder.Build();

        // Middleware CORS
        app.UseCors();

        // Configuración de Swagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Middleware para manejo global de excepciones
        app.UseMiddleware<GlobalExceptionMiddleware>();

        // Middleware de redirección a HTTPS
        app.UseHttpsRedirection();

        // Middleware de autorización
        app.UseAuthorization();

        // Mapeo de controladores
        app.MapControllers();

        // Ejecutar la aplicación
        app.Run();
    }
}

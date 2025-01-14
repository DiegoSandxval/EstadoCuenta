using EstadoCuentaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EstadoCuentaAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets
        public DbSet<Tarjeta> Tarjetas { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de Tarjeta
            modelBuilder.Entity<Tarjeta>(entity =>
            {
                entity.HasKey(t => t.IdTarjeta); // Clave primaria
                entity.Property(t => t.NumeroTarjeta)
                    .IsRequired()
                    .HasMaxLength(16);
                entity.Property(t => t.NombreTitular)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            // Configuración de Movimiento
            modelBuilder.Entity<Movimiento>(entity =>
            {
                entity.HasKey(m => m.IdMovimiento); // Clave primaria
                entity.Property(m => m.Descripcion)
                    .HasMaxLength(255);
                entity.Property(m => m.TipoMovimiento)
                    .HasMaxLength(50);

                // Relación uno a muchos entre Tarjeta y Movimiento
                entity.HasOne(m => m.Tarjeta)
                      .WithMany(t => t.Movimientos)
                      .HasForeignKey(m => m.IdTarjeta)
                      .OnDelete(DeleteBehavior.Cascade); // Propagar eliminaciones si es necesario
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

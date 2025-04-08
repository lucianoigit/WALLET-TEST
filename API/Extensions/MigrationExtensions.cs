using INFRAESTRUCTURE;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigration(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using ApplicationDbContext _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Verificar si hay migraciones pendientes antes de aplicarlas
            var pendingMigrations = _context.Database.GetPendingMigrations();

            if (pendingMigrations.Any())
            {
                Console.WriteLine("Applying pending migrations...");
                _context.Database.Migrate();
            }
            else
            {
                Console.WriteLine("No pending migrations.");
            }
        }
    }
}

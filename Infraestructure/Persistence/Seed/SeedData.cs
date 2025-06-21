using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Seed
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

            if (context.Zombies.Any()) return;

            context.Zombies.AddRange(
                new Zombie
                {
                    Tipo = "Rápido",
                    Puntaje = 10,
                    NivelAmenaza = 2,
                    TiempoDisparo = 3,
                    BalasNecesarias = 2
                },
                new Zombie
                {
                    Tipo = "Lento",
                    Puntaje = 5,
                    NivelAmenaza = 1,
                    TiempoDisparo = 5,
                    BalasNecesarias = 1
                }
            );

            context.SaveChanges();
        }
    }
}
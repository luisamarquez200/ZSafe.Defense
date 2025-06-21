using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases
{
    public class DefenseStrategyService
    {
        private readonly IZombieRepository _repo;
        private readonly AppDbContext _context;

        public DefenseStrategyService(IZombieRepository repo, AppDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<List<Zombie>> GetOptimalStrategy(int bullets, int secondsAvailable)
        {
            var zombies = await _repo.GetAllAsync();
            var result = new List<Zombie>();
            int maxScore = 0;

            void Backtrack(List<Zombie> current, int usedBullets, int usedTime, int score, int index)
            {
                if (usedBullets > bullets || usedTime > secondsAvailable)
                    return;

                if (score > maxScore)
                {
                    maxScore = score;
                    result = new List<Zombie>(current);
                }

                for (int i = index; i < zombies.Count; i++)
                {
                    var zom = zombies[i];
                    current.Add(zom);
                    Backtrack(current, usedBullets + zom.BalasNecesarias, usedTime + zom.TiempoDisparo, score + zom.Puntaje, i + 1);
                    current.RemoveAt(current.Count - 1);
                }
            }

            Backtrack(new List<Zombie>(), 0, 0, 0, 0);
            return result.OrderByDescending(z => z.NivelAmenaza).ToList();
        }

        /// <summary>
        /// Guarda en base de datos la simulación con los zombies eliminados.
        /// </summary>
        public async Task<Simulation> SaveSimulation(int bullets, int secondsAvailable)
        {
            var zombies = await GetOptimalStrategy(bullets, secondsAvailable);

            var simulation = new Simulation
            {
                Fecha = DateTime.Now,
                BalasDisponibles = bullets,
                TiempoDisponible = secondsAvailable,
                Eliminados = zombies.Select(zom => new Eliminated
                {
                    ZombieId = zom.Id,
                    PuntosObtenidos = zom.Puntaje
                }).ToList()
            };

            _context.Simulations.Add(simulation);
            await _context.SaveChangesAsync();

            return await _context.Simulations
                .Include(s => s.Eliminados)
                    .ThenInclude(e => e.Zombie)
                .FirstOrDefaultAsync(s => s.Id == simulation.Id);
        }

        /// <summary>
        /// Guarda en base de datos los registros
        /// </summary>
        public async Task<Simulation> RegisterRealStrategy(int bullets, int secondsAvailable, List<int> zombieIds)
        {
            var zombies = await _context.Zombies
                .Where(z => zombieIds.Contains(z.Id))
                .ToListAsync();

            var simulation = new Simulation
            {
                Fecha = DateTime.Now,
                BalasDisponibles = bullets,
                TiempoDisponible = secondsAvailable,
                Eliminados = zombies.Select(z => new Eliminated
                {
                    ZombieId = z.Id,
                    PuntosObtenidos = z.Puntaje
                }).ToList()
            };

            _context.Simulations.Add(simulation);
            await _context.SaveChangesAsync();

            return await _context.Simulations
                .Include(s => s.Eliminados)
                .ThenInclude(e => e.Zombie)
                .FirstOrDefaultAsync(s => s.Id == simulation.Id);
        }

        public async Task<List<Simulation>> GetAllSimulations()
        {
            return await _context.Simulations
                .Include(s => s.Eliminados)
                .ThenInclude(e => e.Zombie)
                .OrderByDescending(s => s.Fecha)
                .ToListAsync();
        }



    }

}
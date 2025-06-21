using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ZombieRepository : IZombieRepository
    {
        private readonly AppDbContext _context;

        public ZombieRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<Zombie>> GetAllAsync()
        {
            return _context.Zombies.ToListAsync();
        }
    }
}
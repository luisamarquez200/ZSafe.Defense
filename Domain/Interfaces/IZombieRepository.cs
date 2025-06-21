using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IZombieRepository
    {
        Task<List<Zombie>> GetAllAsync();
    }
}

using System;

namespace Domain.Entities
{
    public class Eliminated
    {
        public int Id { get; set; }

        public int ZombieId { get; set; }                
        public Zombie Zombie { get; set; } = null!;      

        public int SimulationId { get; set; }           
        public Simulation Simulation { get; set; } = null!;  

        public int PuntosObtenidos { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now; 
    }
}
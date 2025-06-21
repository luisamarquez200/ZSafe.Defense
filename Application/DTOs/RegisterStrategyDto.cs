namespace Application.DTOs
{
    public class RegisterStrategyDto
    {
        public int BalasDisponibles { get; set; }
        public int TiempoDisponible { get; set; }
        public List<int> ZombieIds { get; set; } = new();
    }
}

namespace Domain.Entities;
public class Simulation
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public int TiempoDisponible { get; set; }
    public int BalasDisponibles { get; set; }
    public List<Eliminated> Eliminados { get; set; } = new();

}
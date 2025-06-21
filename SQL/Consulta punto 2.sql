SELECT 
s.Id AS SimulacionId, 
z.Tipo AS TipoZombie,
COUNT(e.Id) AS CantidadEliminados FROM Eliminados e 
JOIN Simulaciones s  ON s.Id = e.SimulationId 
JOIN Zombies z  ON z.Id = e.ZombieId
GROUP  BY s.Id, z.Tipo 
ORDER  BY s.Id, z.Tipo 

CREATE TRIGGER tgrAuditInsertSimulation
ON Simulaciones
AFTER INSERT 
AS 
BEGIN 
	SET NOCOUNT ON;

	INSERT INTO SimulacionesLog(SimulationID, Accion, Usuario, Detalles)
	SELECT
		i.Id,
		'INSERT',
		SYSTEM_USER,
		'Balas: ' + CAST(i.BalasDisponibles AS NVARCHAR) + 
		' Tiempo: ' + CAST(i.TiempoDisponible AS NVARCHAR)
	FROM inserted i;
END;



CREATE TRIGGER tgrAuditUpdateSimulation
ON Simulaciones
AFTER UPDATE 
AS 
BEGIN 
	SET NOCOUNT ON;

	INSERT INTO SimulacionesLog(SimulationID, Accion, Usuario, Detalles)
	SELECT
		i.Id,
		'UPDATE',
		SYSTEM_USER,
		'ANTES: Balas: ' + CAST(d.BalasDisponibles AS NVARCHAR) + 
		' DESPUÉS: Balas: ' + CAST(i.BalasDisponibles AS NVARCHAR)
	FROM inserted i
	JOIN deleted d ON i.Id = d.Id;
END;

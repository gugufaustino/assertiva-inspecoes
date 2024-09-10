





CREATE FUNCTION [dbo].[FunctionTable]
(
	@param1 float,
	@param2 float 
)
RETURNS @returntable TABLE
(
	Id int,
	NomeOperador varchar(300),
	Distancia float
) 
AS 
BEGIN
	INSERT @returntable
		--SELECT @param1, @param2 
		SELECT OPERADOR.ID as Id, NomeOperador,  dbo.fnCalcDistancia(@param1, @param2, ENDERECO.Latitude, ENDERECO.Longitude) as DISTANCIA
		    FROM OPERADOR
		INNER JOIN ENDERECO ON OPERADOR.IDENDERECO = ENDERECO.ID
	RETURN
END
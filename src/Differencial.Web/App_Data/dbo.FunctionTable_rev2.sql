USE [bdDifferencial]
GO

/****** Object: Table Valued Function [dbo].[FunctionTable] Script Date: 06/06/2017 22:57:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





ALTER FUNCTION [dbo].[FunctionTable]
(
	@param1 float,
	@param2 float 
)
RETURNS @returntable TABLE
(
	Id int,
	NomeOperador varchar(300),
	DistanciaRaio float null,
	DistanciaRota float null
) 
AS 
BEGIN

DECLARE @Top AS INT

--DECLARE @param1 AS FLOAT, @param2 AS FLOAT
--SET @param1 = -30.008597 
--SET @param2 = -51.191220


	INSERT @returntable

		SELECT TOP 10
				Operador.Id,
				Operador.NomeOperador,
				--DISTANCIA = CASE WHEN ENDERECO.Latitude is NULL THEN NULL ELSE dbo.fnCalcDistancia(@param1, @param2, ENDERECO.Latitude, ENDERECO.Longitude) END
				DISTANCIA =  dbo.fnCalcDistancia(@param1, @param2, Endereco.Latitude, Endereco.Longitude),
				NULL
		FROM Operador		
		INNER JOIN Vistoriador ON Operador.Id = Vistoriador.Id
		INNER JOIN Endereco ON Vistoriador.IdEnderecoBase = Endereco.Id
		WHERE Endereco.Latitude IS NOT NULL
		ORDER BY 3


	RETURN
END

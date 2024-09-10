using Microsoft.EntityFrameworkCore.Migrations;

namespace Differencial.Repository.Migrations
{
    public partial class init_function_sp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
							GO
							SET ANSI_NULLS ON
							GO
							SET QUOTED_IDENTIFIER ON
							GO
							CREATE FUNCTION [dbo].[fnCalcDistancia] 
							(
							@latIni float, @lonIni float, @latFim float, @lonFim float  
							)
							RETURNS float
							AS
							BEGIN

							DECLARE @Result as float, @arcoA as float, @arcoB  float, @arcoC as float, @auxPI as FLOAT;

							SET @auxPi = Pi() / 180;
							SET @arcoA = (@lonFim - @lonIni) * @auxPi;
							SET @arcoB = (90 - @latFim) * @auxPi;
							SET @arcoC = (90 - @latIni) * @auxPi;
							SET @Result = Cos(@arcoB) * Cos(@arcoC) + Sin(@arcoB) * Sin(@arcoC) * Cos(@arcoA);
							SET @Result = (40030 * ((180 / Pi()) * Acos(@Result))) /360

							RETURN Round(@Result,4)
							END
							GO

							CREATE FUNCTION [dbo].[FunctionTable]
							(
								@param1 float,
								@param2 float,
								@param3 int 
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

							/*
							DECLARE @param1 AS FLOAT, @param2 AS FLOAT
							SET @param1 = -30.008597 
							SET @param2 = -51.191220 
							*/


								INSERT @returntable

									SELECT TOP 10
											Operador.Id,
											Operador.NomeOperador,
											/* DISTANCIA = CASE WHEN ENDERECO.Latitude is NULL THEN NULL ELSE dbo.fnCalcDistancia(@param1, @param2, ENDERECO.Latitude, ENDERECO.Longitude) END */
											DISTANCIA =  dbo.fnCalcDistancia(@param1, @param2, Endereco.Latitude, Endereco.Longitude),
											NULL
									FROM Operador		
									INNER JOIN Vistoriador ON Operador.Id = Vistoriador.Id
									INNER JOIN VistoriadorProduto ON VistoriadorProduto.IdVistoriador = Vistoriador.Id
									INNER JOIN Endereco ON Vistoriador.IdEnderecoBase = Endereco.Id
									WHERE Endereco.Latitude IS NOT NULL 
									AND VistoriadorProduto.IndAtivo = 1 
									AND VistoriadorProduto.IdProduto = @param3
									ORDER BY 3


								RETURN
							END

							GO
							CREATE PROCEDURE [dbo].[sp_setusercontext] 
								@cod_usuario VARCHAR(10)
							AS
							BEGIN
								SET NOCOUNT ON;

								DECLARE @context VARBINARY(10)
								SET @context = CONVERT(BINARY(10), @cod_usuario)

								SET CONTEXT_INFO @context
							END

					");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

using Dapper;
using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Util;
using Differencial.Domain.DTO;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.Queries;
using Differencial.Queries;
using System.Collections.Generic;
using System.Linq;

namespace Differencial.Queries.Queries
{
    public class VistoriadorQueries : QueriesBase<Vistoriador, VistoriadorFilter>, IVistoriadorQueries
    {
        public VistoriadorQueries(IUsuarioService usuario,
                                        IConfiguracaoAplicativo configuracaoAplicativo)
            : base(usuario, configuracaoAplicativo)
        {
        }

        public List<OperadorDistancia> ListarOperadorDistancia(double latitude, double longitude, int idProduto, int? IdContratoLancamentoValor)
        {
            //(float)-30.008597, (float)-51.191220
            var sql = "SELECT TOP 10" +
                       " Operador.Id, Operador.NomeOperador," +
                       " DistanciaRaio = dbo.fnCalcDistancia(@latitude, @longitude, Endereco.Latitude, Endereco.Longitude)" +
                       " FROM Operador" +
                       " INNER JOIN Vistoriador ON Operador.Id = Vistoriador.Id                               " +
                       " INNER JOIN VistoriadorProduto ON VistoriadorProduto.IdVistoriador = Vistoriador.Id   " +
                       " INNER JOIN Endereco ON Vistoriador.IdEnderecoBase = Endereco.Id                      " +
                       " WHERE Endereco.Latitude IS NOT NULL                                                " +
                       " AND VistoriadorProduto.IndAtivo = 1  AND VistoriadorProduto.IdProduto = @IdProduto" +
                       "   AND VistoriadorProduto.IdContratoLancamentoValor = @IdContratoLancamentoValor" +
                       " ORDER BY 3";

            return _conn.Query<OperadorDistancia>(sql, new { latitude, longitude, idProduto, IdContratoLancamentoValor }).ToList();
        }

    }
}
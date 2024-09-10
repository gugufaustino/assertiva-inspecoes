using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.DTO;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Queries
{
    public interface IVistoriadorQueries : IQueryBase<Vistoriador, VistoriadorFilter>
    {
        List<OperadorDistancia> ListarOperadorDistancia(double latitude, double longitude, int idProduto, int? IdContratoLancamentoValor);
    }
}
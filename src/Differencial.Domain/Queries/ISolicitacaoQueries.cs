using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Filters;
using Differencial.Domain.Queries.Dao;
using System.Collections.Generic;

namespace Differencial.Domain.Queries
{
    public interface ISolicitacaoQueries  
    {
        List<SolicitacaoCobrancaVistoriaDao> SolicitacoesCobrancaVistoria();
    }
}

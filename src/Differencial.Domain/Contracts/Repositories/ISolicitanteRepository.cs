using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Repositories
{
    public interface ISolicitanteRepository : IRepository<Solicitante>
    {
        IEnumerable<Solicitante> ddlSolicitante(int idSeguradora);
    }
}
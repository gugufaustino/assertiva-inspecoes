using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Repositories
{
    public interface INotificacaoOperadorRepository : IRepository<NotificacaoOperador>
    {
        IEnumerable<NotificacaoOperador> MinhasNotificacoes(int idOperador);
    }
}
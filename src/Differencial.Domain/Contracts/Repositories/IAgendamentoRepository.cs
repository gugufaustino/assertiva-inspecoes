using System;
using System.Collections.Generic;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;

namespace Differencial.Domain.Contracts.Repositories
{
    public interface IAgendamentoRepository : IRepository<Agendamento>
    {
        IEnumerable<Agendamento> ListarAgendamentosVistoriadorDiaVigentes(int idVistoriador, DateTime dataAgenda);

        IEnumerable<Agendamento> ListarAgendamentosVistoriadorDiaVigentesCancelada(int idVistoriador, DateTime dataAgenda, int IdSolicCancelada);
    }
}
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;
namespace Differencial.Domain.Contracts.Services
{
    public interface IDashboardsService : IBaseService<Solicitacao, SolicitacaoFilter>
    {      
        IEnumerable<Solicitacao> ListarSolicitacoesVistoriador();
        IEnumerable<Solicitacao> ListarSolicitacoesGerencia();
        IEnumerable<Solicitacao> ListarSolicitacoesAnalista();
        IEnumerable<Solicitacao> ListarSolicitacoesAnalistaMinhas();
        IEnumerable<Solicitacao> ListarSolicitacoesSolicitante();
        List<Solicitacao> ListarSolicitacoesGerenciaAgendamento();
        IEnumerable<Solicitacao> ListarSolicitacoesFinanceiro();
    }
  
    }
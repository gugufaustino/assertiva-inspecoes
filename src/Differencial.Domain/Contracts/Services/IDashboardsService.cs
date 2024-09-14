using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Differencial.Domain.Contracts.Services
{
    public interface IDashboardsService : IBaseService<Solicitacao, SolicitacaoFilter>
    {      
        IEnumerable<Solicitacao> ListarSolicitacoesVistoriador();
        Task<List<Solicitacao>> ListarSolicitacoesGerencia();
        Task<List<Solicitacao>> ListarSolicitacoesAnalista();
        Task<List<Solicitacao>> ListarSolicitacoesAnalistaMinhas();
        IEnumerable<Solicitacao> ListarSolicitacoesSolicitante();
        Task<List<Solicitacao>> ListarSolicitacoesGerenciaAgendamento();
        IEnumerable<Solicitacao> ListarSolicitacoesFinanceiro();
    }
  
    }
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Differencial.Domain.Contracts.Repositories
{
    public interface ISolicitacaoRepository : IRepository<Solicitacao>
    {
        Solicitacao BuscarUI(int id);
        List<Solicitacao> ListarSolicitacoesGerenciaAgendamento();
        IEnumerable<Solicitacao> ListarSolicitacoesFinanceiro();
        IEnumerable<Solicitacao> ListarSolicitacoesVistoriador();
        IEnumerable<Solicitacao> ListarSolicitacoesAnalistaMinhas();
        IEnumerable<Solicitacao> ListarSolicitacoesAnalista();
        IEnumerable<Solicitacao> ListarTodasAgendas();
        IEnumerable<Solicitacao> ListarTodasSolicitacoes();
        IEnumerable<Solicitacao> ListarSolicitacoesGerencia();

        Task<Solicitacao> BuscarParaEnviar(int id);
        Task<Solicitacao> BuscarComContrato(int id);
        Task<Solicitacao> BuscarParaExcluir(int id);
    }
}
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Differencial.Domain.Contracts.Repositories
{
    public interface ISolicitacaoRepository : IRepository<Solicitacao>
    {
        Task<Solicitacao> BuscarUI(int id);

        Task<List<Solicitacao>> ListarSolicitacoesGerenciaAgendamento();
        IEnumerable<Solicitacao> ListarSolicitacoesFinanceiro();
        IEnumerable<Solicitacao> ListarSolicitacoesVistoriador();
        Task<List<Solicitacao>> ListarSolicitacoesAnalistaMinhas();
        Task<List<Solicitacao>> ListarSolicitacoesAnalista();
        IEnumerable<Solicitacao> ListarTodasAgendas();
        IEnumerable<Solicitacao> ListarTodasSolicitacoes();
        Task<List<Solicitacao>> ListarSolicitacoesGerencia();

        Task<Solicitacao> BuscarParaEnviar(int id);
        Task<Solicitacao> BuscarComContrato(int id);
        Task<Solicitacao> BuscarParaExcluir(int id);
    }
}
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.Queries.Dao;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
namespace Differencial.Domain.Contracts.Services
{

	public interface ISolicitacaoService //: IBaseService<Solicitacao, SolicitacaoFilter>
    {
        void SalvarAtividadeDefinirVistoriador(int id, int idVistoriador, string txtJustificativaVistoriadorDefinido);
        void SalvarAtividadeCroqui(int id, IFormFile arquivocroquie, TipoArquivoAnexoEnum tipoArquivoAnexo);
        void PreAcordoCustoDeslocamento(int id, decimal custoDeslocamentoAcordado);
        void PreAcordoVlrPagamentoVistoriaAcordado(int id, decimal vlrPagamentoVistoriaAcordado);
        void SalvarAtividadeAgendar(Solicitacao solicitacao, TipoAgendamentoEnum tipoAgendamentoEnum, DateTime? dateTime = null, string motivoCancelamentoReagendamento = null, string contatoAgendamento = null);
        string RotaAnteriorUrlMapa(Solicitacao solicitacao);
        string RotaDeVoltaUrlMapa(Solicitacao entidade);
        Solicitacao RotaAnteriorSolicitacao(Solicitacao solicitacao);
        void SalvarAtividadeInformarRotaRealizada(int id, TipoOpcaoInformarRota rbtTipoIntinerario, string txtJustificativaDeslocamentoRealizado, decimal? deslocamentoRealizado, DateTime? dataAgenda);
        void SalvarAtividadeInformarAgendamento(int id, TipoNotificacaoEnum rbtTipoNotificacao, Comunicacao comunicacaoAssuntoTexto);
        void SalvarAtividadeDefinirAnalista(int id, int idAnalista);
        void ValorizarFinanceiro(int id);
        void RegistrarLancamentoFinanceiro(int IdSolicitacao, LancamentoFinanceiro lancamento);
        void SalvarAtividadeCheckList(int id, decimal? areaConstruida, int? blocoConstruido);
        void SalvarAtividadeLaudoAnalista(int id, IFormFile arquivolaudoanalista, decimal? areaConstruida, int? blocoConstruido, int? casaConstruida, int? qtdEquipamento, bool? indRelatorioExigenciaMelhoria);
        void SalvarAtividadeRealizarVistoria(int id);
        void RegistrarComunicacao(Comunicacao comunicacao, bool indEnviarEmail);
        Agendamento AgendamentoVigenteSolicitacao(Solicitacao solicitacao);
        void RN_ValidarExiteAgendamento(Solicitacao solicitacao);
        void RN_ValidarExiteInspecaoRealizada(Solicitacao solicitacao);
        void RN_ValidarExiteVistoriadorDefinido(Solicitacao solicitacao);
        void RN_ValidarExiteLaudoConcluido(Solicitacao solicitacao);
        Solicitacao Reinspecao(int id);
        Solicitacao BuscarUI(int id);
        Solicitacao BuscarParaAgendar(int id);
        void EnviarEmailCobrancaVistoria(SolicitacaoCobrancaVistoriaDao solicitacao, int usuarioServiceId);
        void CobrarVistoria( int usuarioServiceId);
        Solicitacao BuscarSolicitacaoEndereco(int id);
        Solicitacao BuscarComMovimento(int id);

		void SalvarSolicitacao(Solicitacao entidade);
		Solicitacao Buscar(int id);

		IEnumerable<Solicitacao> Listar(SolicitacaoFilter filtro); 

		void Excluir(int id);

		void Excluir(int[] ids);

	}
}
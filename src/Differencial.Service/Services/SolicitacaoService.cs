using Differencial.Domain;
using Differencial.Domain.Annotation;
using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Contracts.Util;
using Differencial.Domain.DTO;
using Differencial.Domain.Entities;
using Differencial.Domain.Exceptions;
using Differencial.Domain.Filters;
using Differencial.Domain.Queries;
using Differencial.Domain.Queries.Dao;
using Differencial.Domain.Resources;
using Differencial.Domain.UOW;
using Differencial.Domain.Util;
using Differencial.Domain.Util.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Differencial.Service.Services
{
	public class SolicitacaoService : Service, ISolicitacaoService, IDashboardsService, IConsultasService, IWorkFlowSolicitacaoService
	{
		ISolicitacaoRepository _solicitacaoRepositorio;
		IEnderecoService _enderecoService;
		IClienteService _clienteService;
		IProdutoService _produtoService;
		IOperadorService _operadorService;
		IConfiguracaoAplicativo _configuracaoAplicativo;
		IEmailService _emailService;
		IArquivoAnexoService _arquivoAnexoService;
		IUsuarioService _usuarioService;
		ICoberturaService _coberturaService;
		IWorkFlowService _workflow;
		IMovimentacaoProcessoService _movimentacaoProcessoService;
		IAgendamentoService _agendamentoService;
		ILancamentoFinanceiroService _lancamentoFinanceiroService;
		IContratoService _contratoService;
		ILancamentoFinanceiroTotalService _lancamentoFinanceiroTotalService;
		IComunicacaoService _comunicacaoService;
		IAtividadeProcessoService _atividadeService;
		INotificacaoService _notificacaoService;
		ISolicitacaoQueries _solicitacaoQueries;
		private readonly ILaudoFotoService _laudoFotoService;

		public SolicitacaoService(IUnitOfWork uow, ISolicitacaoRepository solicitacaoRepositorio,
			IEnderecoService enderecoService,
			IClienteService clienteService,
			IProdutoService produtoService,
			IOperadorService operadorService,
			IConfiguracaoAplicativo configuracaoAplicativo,
			IEmailService emailService,
			IArquivoAnexoService arquivoAnexoService,
			ICoberturaService coberturaService,
			IWorkFlowService workflow,
			IMovimentacaoProcessoService movimentacaoProcessoService,
			IUsuarioService usuarioService,
			IAgendamentoService agendamentoService,
			ILancamentoFinanceiroService lancamentoFinanceiroService,
			ILancamentoFinanceiroTotalService lancamentoFinanceiroTotalService,
			IContratoService contratoService,
			IComunicacaoService comunicacaoService,
			IAtividadeProcessoService atividadeService,
			INotificacaoService notificacaoService,
			ISolicitacaoQueries solicitacaoQueries,
			ILaudoFotoService laudoFotoService)
			: base(uow)
		{
			_solicitacaoRepositorio = solicitacaoRepositorio;
			_enderecoService = enderecoService;
			_clienteService = clienteService;
			_produtoService = produtoService;
			_operadorService = operadorService;
			_configuracaoAplicativo = configuracaoAplicativo;
			_emailService = emailService;
			_arquivoAnexoService = arquivoAnexoService;
			_coberturaService = coberturaService;
			_usuarioService = usuarioService;
			_workflow = workflow;
			_movimentacaoProcessoService = movimentacaoProcessoService;
			_agendamentoService = agendamentoService;
			_lancamentoFinanceiroService = lancamentoFinanceiroService;
			_lancamentoFinanceiroTotalService = lancamentoFinanceiroTotalService;
			_contratoService = contratoService;
			_comunicacaoService = comunicacaoService;
			_atividadeService = atividadeService;
			_notificacaoService = notificacaoService;

			_solicitacaoQueries = solicitacaoQueries;
			_laudoFotoService = laudoFotoService;
		}
		#region Listas Dashboards
		public async Task<List<Solicitacao>> ListarSolicitacoesGerencia()
		{
			return await _solicitacaoRepositorio.ListarSolicitacoesGerencia();
		}
		public IEnumerable<Solicitacao> ListarSolicitacoesVistoriador()
		{
			return TryCatch(() =>
			{
				return _solicitacaoRepositorio.ListarSolicitacoesVistoriador();
			});
		}
		public async Task<List<Solicitacao>> ListarSolicitacoesAnalista()
		{

			return await _solicitacaoRepositorio.ListarSolicitacoesAnalista();

		}
		public async Task<List<Solicitacao>> ListarSolicitacoesAnalistaMinhas()
		{
			return await _solicitacaoRepositorio.ListarSolicitacoesAnalistaMinhas();


		}
		public IEnumerable<Solicitacao> ListarSolicitacoesSolicitante()
		{
			return TryCatch(() =>
			{
				var operador = _operadorService.Buscar(_usuarioService.Id);
				var idSeguradora = operador.Solicitante.Seguradora.Id;
				return _solicitacaoRepositorio
						.Where(w => w.IdSeguradora == idSeguradora)
						.ToList();
			});
		}
		public async Task<List<Solicitacao>> ListarSolicitacoesGerenciaAgendamento()
		{
			return await _solicitacaoRepositorio.ListarSolicitacoesGerenciaAgendamento();
		}
		public IEnumerable<Solicitacao> ListarSolicitacoesFinanceiro()
		{
			return _solicitacaoRepositorio.ListarSolicitacoesFinanceiro();
		}

		#endregion Listas Dashboards
		#region "Consultas"
		public IEnumerable<Solicitacao> ListarEmTramitacao()
		{
			return TryCatch(() =>
			{
				return _solicitacaoRepositorio.Where(w => w.TpSituacao != TipoSituacaoProcessoEnum.Concluido &&
													 w.TpSituacao != TipoSituacaoProcessoEnum.Cancelado &&
													  w.TpSituacao != TipoSituacaoProcessoEnum.CanceladoPelaSeguradora &&
													w.TpSituacao != TipoSituacaoProcessoEnum.EmElaboracao &&
													 w.TpSituacao != TipoSituacaoProcessoEnum.EmElaboracaoSolicitante).ToList();
			});
		}
		public IEnumerable<Solicitacao> ListarConcluidas()
		{
			return TryCatch(() =>
			{
				return _solicitacaoRepositorio.Where(w => w.TpSituacao == TipoSituacaoProcessoEnum.Concluido).ToList();
			});
		}
		public IEnumerable<Solicitacao> ListarTodasAgendas()
		{
			return _solicitacaoRepositorio.ListarTodasAgendas();
		}

		public IEnumerable<Solicitacao> ListarTodasSolicitacoes()
		{
			return _solicitacaoRepositorio.ListarTodasSolicitacoes();
		}

		#endregion "Consultas"
		public IEnumerable<Solicitacao> ListarTodasRotas(SolicitacaoFilter filtro)
		{
			return TryCatch(() =>
			{
				return _solicitacaoRepositorio.Where(filtro).ToList();
			});
		}
		public async Task SalvarSolicitacao(Solicitacao entidade)
		{
			await TryCatchAsync(() =>
			{
				if (entidade.CodSistemaLegado == 999)
					throw new ValidationException("oloko ");

				entidade.Cobertura = entidade.Cobertura.Where(c => c.NomeCobertura != null).ToList();
				entidade.Validate();
				RN_ValidarExisteContratoNoProduto(entidade.IdProduto);
				_contratoService.ValidarContratoLancamentoValorParametro(entidade, TipoContratoParametroEnum.ValorRisco);
				int? idContratoLancamentoValor = null;
				if (_contratoService.IndicaContratoLancamentoValorRisco(entidade, out idContratoLancamentoValor))
					entidade.IdContratoLancamentoValor = idContratoLancamentoValor;
				if (entidade.Id == 0)
					Inserir(entidade);
				else
					Editar(entidade);
				return Task.CompletedTask;
			});
		}
		public async Task Excluir(int id)
		{
			await TryCatch(async () =>
			{
				var solic = await _solicitacaoRepositorio.BuscarParaExcluir(id);

				if (solic.TpSituacao != TipoSituacaoProcessoEnum.EmElaboracao
				&& solic.TpSituacao != TipoSituacaoProcessoEnum.EmElaboracaoSolicitante
				&& solic.TpSituacao != TipoSituacaoProcessoEnum.ApropriadoPeloSolicitante)
					throw new ValidationException("Não é possível excluir o registro {0}".Formata(solic.TpSituacao.GetAttributeOfType<SituacaoProcessoAttribute>().Name));


				_laudoFotoService.ExcluirFotoLaudoFoto(solic.Foto);
				_comunicacaoService.Excluir(solic.Comunicacao);
				_coberturaService.Excluir(solic.Cobertura.Select(i => i.Id).ToArray());
				_movimentacaoProcessoService.Excluir(solic.MovimentacaoProcesso.Select(i => i.Id).ToArray());
				_atividadeService.Excluir(solic.AtividadeProcesso.Select(i => i.Id).ToArray());
				_solicitacaoRepositorio.Delete(solic);
			});
		}
		public async Task Excluir(int[] ids)
		{

			foreach (var id in ids)
			{
				await Excluir(id);
			}

		}
		public Solicitacao Buscar(int id)
		{
			return TryCatch(() =>
			{
				return _solicitacaoRepositorio.Find(id);
			});
		}
		private void Inserir(Solicitacao entidade)
		{
			RN_ValidarCodSeguradoraUnico(entidade.Id, entidade.IdSeguradora, entidade.CodSeguradora);
			if (_usuarioService.UsuarioAutenticado.IndSolicitante)
			{
				var op = _operadorService.Buscar(_usuarioService.Id);
				entidade.IdSolicitante = _usuarioService.Id;
				entidade.SolicitanteNome = _usuarioService.UsuarioAutenticado.NomeOperador;
				entidade.SolicitanteTelefone = op.Solicitante.Operador.Telefone;
				entidade.SolicitanteEmail = op.Solicitante.Operador.Email;
			}
			entidade.Seguradora = _produtoService.Buscar(entidade.IdProduto).Seguradora;
			_enderecoService.Salvar(entidade.Endereco);
			entidade.Cliente.ClienteEndereco.Add(new ClienteEndereco { Endereco = entidade.Endereco });
			_clienteService.Salvar(entidade.Cliente);
			_coberturaService.Salvar(entidade.Cobertura, entidade.Id);
			_workflow.Criar(entidade, _usuarioService.UsuarioAutenticado.IndSolicitante ? TipoSituacaoProcessoEnum.EmElaboracaoSolicitante : TipoSituacaoProcessoEnum.EmElaboracao);
			_solicitacaoRepositorio.Add(entidade);
		}
		private void Editar(Solicitacao entidade)
		{
			Solicitacao oldEntidade = Buscar(entidade.Id);
			oldEntidade.CodSistemaLegado = entidade.CodSistemaLegado;
			oldEntidade.CodSeguradora = entidade.CodSeguradora;
			oldEntidade.SolicitanteTelefone = entidade.SolicitanteTelefone;
			oldEntidade.SolicitanteEmail = entidade.SolicitanteEmail;
			oldEntidade.CorretorNome = entidade.CorretorNome;
			oldEntidade.CorretorTelefone = entidade.CorretorTelefone;
			oldEntidade.TxtInformacoesAdicionais = entidade.TxtInformacoesAdicionais;
			oldEntidade.VlrRiscoSegurado = entidade.VlrRiscoSegurado;
			oldEntidade.AreaConstruida = entidade.AreaConstruida;
			oldEntidade.BlocoConstruido = entidade.BlocoConstruido;
			oldEntidade.VlrRiscoSegurado = entidade.VlrRiscoSegurado;
			oldEntidade.VlrHonorarioPreAcordo = entidade.VlrHonorarioPreAcordo;
			oldEntidade.IdContratoLancamentoValor = entidade.IdContratoLancamentoValor;
			oldEntidade.IndUrgente = entidade.IndUrgente;
			oldEntidade.IdSolicitante = entidade.IdSolicitante;
			oldEntidade.IdFilial = entidade.IdFilial;
			if (oldEntidade.TpSituacao == TipoSituacaoProcessoEnum.EmElaboracao || oldEntidade.TpSituacao == TipoSituacaoProcessoEnum.ApropriadoGerente)
			{
				_enderecoService.Salvar(entidade.Endereco);
				_clienteService.Salvar(entidade.Cliente);
			}
			_coberturaService.Salvar(entidade.Cobertura, entidade.Id);
			_solicitacaoRepositorio.Update(oldEntidade);
		}
		#region Lancamentos Financeiros
		private void InserirLancamentosReceita(Solicitacao solicitacao)
		{
			var lstLancamentosValor = _contratoService.GerarLancamentosContrato(solicitacao);
			if (lstLancamentosValor.Count > 0)
			{
				//valor total
				var lancamentoTotal = new LancamentoFinanceiroTotal
				{
					IdSolicitacao = solicitacao.Id,
					Solicitacao = solicitacao,
					TipoLancamentoFinanceiro = TipoLancamentoFinanceiroEnum.ReceitaProdutoReceberSeguradora,
					DthLancamentoPagamento = DataSintetico(solicitacao.Seguradora, TipoLancamentoFinanceiroEnum.ReceitaProdutoReceberSeguradora), // TODO: ajustar data lancamento aqui,
					ValorLancamentoFinanceiroTotal = lstLancamentosValor.Sum(i => i.Value)
				};
				_lancamentoFinanceiroTotalService.Salvar(lancamentoTotal);
				//lancamento unitário
				foreach (var lancamentoValor in lstLancamentosValor)
				{
					_lancamentoFinanceiroService.Salvar(new LancamentoFinanceiro
					{
						IdSolicitacao = solicitacao.Id,
						Solicitacao = solicitacao,
						TipoLancamentoFinanceiro = TipoLancamentoFinanceiroEnum.ReceitaProdutoReceberSeguradora,
						ValorLancamentoFinanceiro = lancamentoValor.Value,
						DescricaoLancamentoFinanceiro = lancamentoValor.Key.Display().Name,
						LancamentoFinanceiroTotal = lancamentoTotal
					});
				}
			}
		}
		private DateTime DataSintetico(Seguradora seguradora, TipoLancamentoFinanceiroEnum tipoLancamentoFinanceiroEnum)
		{
			int diaFechamentoInspecoes = seguradora.ContabilInspecoesDiaFim;
			int diaLancamentoSintetico = tipoLancamentoFinanceiroEnum == TipoLancamentoFinanceiroEnum.DespesaPagamentoVistoriador ? seguradora.ContabilInspetorDia
																																	: seguradora.ContabilEmpresaDia;
			bool mesAdicional = diaFechamentoInspecoes > diaLancamentoSintetico;
			int diaLancamento = DateTime.Now.Date.Day;
			DateTime DthLancamentoSintetico;
			if (diaLancamento <= diaFechamentoInspecoes)
			{
				DthLancamentoSintetico = new DateTime(DateTime.Now.Year, DateTime.Now.Month + (mesAdicional ? 1 : 0), diaLancamentoSintetico);
			}
			else
			{
				DthLancamentoSintetico = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1 + (mesAdicional ? 1 : 0), diaLancamentoSintetico);
			}
			return DthLancamentoSintetico;
		}
		private void InserirLancamentosDespesaVistoriador(Solicitacao solicitacao)
		{
			RN_ValidarExisteValoresCustoVistoriador(solicitacao);
			//valor total
			var vlrCustoDeslocamentoRealizado = (solicitacao.IndCustoVistoriaAcordado || !solicitacao.IndCidadeBaseVistoriador.Value) ? solicitacao.CustoDeslocamentoRealizado.Value : 0;
			var lancamentoTotal = new LancamentoFinanceiroTotal
			{
				IdSolicitacao = solicitacao.Id,
				Solicitacao = solicitacao,
				TipoLancamentoFinanceiro = TipoLancamentoFinanceiroEnum.DespesaPagamentoVistoriador,
				DthLancamentoPagamento = DataSintetico(solicitacao.Seguradora, TipoLancamentoFinanceiroEnum.DespesaPagamentoVistoriador), // TODO: ajustar data lancamento aqui,
				ValorLancamentoFinanceiroTotal = solicitacao.VlrPagamentoVistoria.Value + vlrCustoDeslocamentoRealizado
			};
			_lancamentoFinanceiroTotalService.Salvar(lancamentoTotal);
			_lancamentoFinanceiroService.Salvar(new LancamentoFinanceiro
			{
				IdSolicitacao = solicitacao.Id,
				Solicitacao = solicitacao,
				TipoLancamentoFinanceiro = TipoLancamentoFinanceiroEnum.DespesaPagamentoVistoriador,
				ValorLancamentoFinanceiro = solicitacao.VlrPagamentoVistoria.Value,
				DescricaoLancamentoFinanceiro = "Honorários do Vistoriador - Valor pago pelo serviço de vistoria",
				LancamentoFinanceiroTotal = lancamentoTotal
			});
			if (solicitacao.IndCustoVistoriaAcordado || !solicitacao.IndCidadeBaseVistoriador.Value)
			{
				_lancamentoFinanceiroService.Salvar(new LancamentoFinanceiro
				{
					Solicitacao = solicitacao,
					TipoLancamentoFinanceiro = TipoLancamentoFinanceiroEnum.DespesaPagamentoVistoriador,
					ValorLancamentoFinanceiro = solicitacao.CustoDeslocamentoRealizado.Value,
					DescricaoLancamentoFinanceiro = "Deslocamento do Vistoriador - Valor pago a quilometragem de deslocamento",
					LancamentoFinanceiroTotal = lancamentoTotal
				});
			}
		}
		public void RegistrarLancamentoFinanceiro(int IdSolicitacao, LancamentoFinanceiro lancamento)
		{
			_lancamentoFinanceiroService.Salvar(new LancamentoFinanceiro
			{
				IdSolicitacao = IdSolicitacao,
				TipoLancamentoFinanceiro = lancamento.TipoLancamentoFinanceiro,
				ValorLancamentoFinanceiro = lancamento.ValorLancamentoFinanceiro,
				DescricaoLancamentoFinanceiro = lancamento.DescricaoLancamentoFinanceiro
			});
		}
		#endregion Lancamentos Financeiros
		#region WorkFlow Ações
		public async Task Enviar(int Id, string textoMovimentacao, IFormFile arquivo)
		{
			await TryCatch(async () =>
			  {
				  var solicitacao = await _solicitacaoRepositorio.BuscarParaEnviar(Id);
				  var movimentacaoRealizada = _workflow.Enviar(solicitacao, textoMovimentacao);
				  switch ((TipoSituacaoProcessoEnum)movimentacaoRealizada)
				  {
					  case TipoSituacaoProcessoEnum.EnviadoParaVistoria:
						  RN_ValidarExiteVistoriadorDefinido(solicitacao);
						  EnviarEmailSolicitacaoVistoriador(solicitacao, textoMovimentacao, arquivo);
						  break;
					  case TipoSituacaoProcessoEnum.EnviadoParaAnalise:
						  RN_ValidarExiteAgendamento(solicitacao);

						  break;
					  case TipoSituacaoProcessoEnum.EnviadoParaFinanceiro:
						  InserirLancamentosReceita(solicitacao);
						  InserirLancamentosDespesaVistoriador(solicitacao);
						  break;
					  case TipoSituacaoProcessoEnum.ApropriadoPeloFinanceiro:
						  break;
				  }
				  solicitacao.TpSituacao = (TipoSituacaoProcessoEnum)movimentacaoRealizada;
				  _solicitacaoRepositorio.Update(solicitacao);
			  });
		}
		public void Apropriar(int Id)
		{
			TryCatch(() =>
			{
				var solic = Buscar(Id);
				_workflow.Apropriar(solic);
				_solicitacaoRepositorio.Update(solic);
			});
		}
		public void Cancelar(int Id, string textoMovimentacao)
		{
			TryCatch(() =>
			{
				var solic = Buscar(Id);
				_workflow.Cancelar(solic, textoMovimentacao);
				_solicitacaoRepositorio.Update(solic);
			});
		}
		public void Devolver(int id, string textoMovimentacao, TipoMotivoEnum? tipoMotivo)
		{
			TryCatch(() =>
			{
				var motivo = tipoMotivo != null ? tipoMotivo.ToString() : string.Empty;  // tipoMotivo.Value.GetAttributeOfType<DisplayNameAttribute>();
				var solic = Buscar(id);
				_workflow.Devolver(solic, "{0}: {1}".Formata(motivo, textoMovimentacao));

				_solicitacaoRepositorio.Update(solic);
				_notificacaoService.SalvarTransmitirNotificacaoProcesso(solic);
			});
		}
		public void Concluir(int id)
		{
			TryCatch(() =>
			{
				var solic = Buscar(id);
				_workflow.Concluir(solic, string.Empty);
				_solicitacaoRepositorio.Update(solic);
			});
		}
		#endregion
		#region Atividades
		public void SalvarAtividadeDefinirVistoriador(int id, int idVistoriador, string txtJustificativaVistoriadorDefinido)
		{
			TryCatch(() =>
			{
				var solicitacao = BuscarSolicitacaoEndereco(id);
				solicitacao.IdVistoriador = idVistoriador;
				solicitacao.TxtJustificativaVistoriadorDefinido = txtJustificativaVistoriadorDefinido;
				var operadorDistancia = _operadorService.BuscarOperadorDistanciaSolicitacao(idVistoriador, id, solicitacao.Endereco.SiglaUf,
																							solicitacao.Endereco.NomeMunicipio, solicitacao.Endereco.Latitude.Value,
																							solicitacao.Endereco.Longitude.Value, solicitacao.IdProduto, solicitacao.IdContratoLancamentoValor);
				solicitacao.VlrPagamentoVistoria = operadorDistancia.VlrPagamentoVistoria;
				solicitacao.VlrQuilometroRodado = operadorDistancia.VlrQuilometroRodado;
				solicitacao.DeslocamentoPrevisto = Convert.ToDecimal(operadorDistancia.DistanciaRota.Value);
				solicitacao.CustoDeslocamentoPrevisto = operadorDistancia.VlrTotalQuilometroRodado;
				solicitacao.CustoTotalPrevisto = operadorDistancia.VlrTotalQuilometroRodadoMaisPagamentoVistoria;
				solicitacao.VistoriadorCidadeBase = operadorDistancia.NomeMunicipioSiglaUf;
				solicitacao.TipoRotaVistoriaPrevista = TipoRotaVistoriaEnum.CidadeBase;
				_atividadeService.Concluir(TipoAtividadeEnum.DefinirVistoriador, solicitacao);
				_solicitacaoRepositorio.Update(solicitacao);
			});
		}
		public void SalvarAtividadeDefinirAnalista(int id, int idAnalista)
		{
			TryCatch(() =>
			{
				var sol = Buscar(id);
				sol.IdAnalista = idAnalista;
				_atividadeService.Concluir(TipoAtividadeEnum.DefinirAnalista, sol);
				_solicitacaoRepositorio.Update(sol);
			});
		}
		public void SalvarAtividadeCroqui(int id, IFormFile arquivo, TipoArquivoAnexoEnum tipoArquivoAnexoEnum)
		{
			TryCatch(() =>
			{
				var lstAnexo = new List<Attachment>();
				if (arquivo != null)
				{
					var sol = Buscar(id);
					_atividadeService.Concluir(TipoAtividadeEnum.ElaborarCroquiAnalise, sol);
					_solicitacaoRepositorio.Update(sol);
					_arquivoAnexoService.EnviarArquivoSolicitacao(id, tipoArquivoAnexoEnum, arquivo);

				}
			});
		}
		public void SalvarAtividadeAgendar(Solicitacao solicitacao, TipoAgendamentoEnum tipoAgendamentoEnum, DateTime? dateAgendamentoRealizado = default(DateTime?), string motivoCancelamentoReagendamento = null, string contatoAgendamento = null)
		{
			TryCatch(() =>
			{
				DateTime dataRotaAgendamento = DateTime.MinValue;
				switch (tipoAgendamentoEnum)
				{
					case TipoAgendamentoEnum.Agendar:
						_agendamentoService.Agendar(dateAgendamentoRealizado.Value, solicitacao);
						dataRotaAgendamento = dateAgendamentoRealizado.Value;
						_atividadeService.Concluir(TipoAtividadeEnum.Agendamento, solicitacao);
						break;
					case TipoAgendamentoEnum.Reagendar:
						_agendamentoService.Reagendar(dateAgendamentoRealizado.Value, motivoCancelamentoReagendamento, solicitacao);
						dataRotaAgendamento = dateAgendamentoRealizado.Value;
						_atividadeService.Concluir(TipoAtividadeEnum.Agendamento, solicitacao);
						break;
					case TipoAgendamentoEnum.Cancelar:
						throw new NotImplementedException();
					default:
						break;
				}
				if (!contatoAgendamento.IsNullOrEmpty())
				{
					var cliente = solicitacao.Cliente;
					cliente.ContatoAgendamento = contatoAgendamento;
					_clienteService.Salvar(cliente);
				}
				List<Agendamento> lstAgendamentosVigentes;
				Agendamento solAtual = null;
				Agendamento solicitacaoPosterior = null;
				Agendamento solicitacaoPosteriorPosAtualizacao = null;
				int solIndex;
				// Consulta Agendamentos pré da atualização para pegar a Solicitação Posterior que será também Recalculada já que ela perde sua referencia SolicAtual
				var lstAgendamentosAnteriores = _agendamentoService.ListarAgendamentosVistoriadorDiaVigentes(solicitacao.Vistoriador.Id, dataRotaAgendamento).ToList();
				solAtual = lstAgendamentosAnteriores.FirstOrDefault(w => w.Solicitacao.Id == solicitacao.Id);
				//Então é nova no dia - não se faz necessário
				if (solAtual != null)
				{
					solIndex = lstAgendamentosAnteriores.FindIndex(a => a.Id == solAtual.Id);
					if (solIndex < lstAgendamentosAnteriores.Count - 1)
						solicitacaoPosterior = lstAgendamentosAnteriores.ElementAtOrDefault(solIndex + 1);
				}
				// Atualiza no entity a nova agenda
				SaveChange(_usuarioService.Id);
				// Consulta Agendamentos vigentes para realziar todos os calculos com os regisros já nas posições definidas
				lstAgendamentosVigentes = _agendamentoService.ListarAgendamentosVistoriadorDiaVigentes(solicitacao.Vistoriador.Id, dataRotaAgendamento).ToList();
				// RECALCULA A Solicitação Atual, pois ela já está em novo posicionamento
				solAtual = lstAgendamentosVigentes.FirstOrDefault(w => w.Solicitacao.Id == solicitacao.Id);
				RecalcularRota(ref lstAgendamentosVigentes, solAtual.Solicitacao);
				// RECALCULA outra Solicitação(Pré atualização)
				if (solicitacaoPosterior != null)
					RecalcularRota(ref lstAgendamentosVigentes, solicitacaoPosterior.Solicitacao);
				// RECALCULA A Solicitação(Pós Atualizacao)
				solIndex = lstAgendamentosVigentes.FindIndex(a => a.Id == solAtual.Id);
				if (solIndex < lstAgendamentosVigentes.Count - 1)
					solicitacaoPosteriorPosAtualizacao = lstAgendamentosVigentes.ElementAtOrDefault(solIndex + 1);
				if (solicitacaoPosteriorPosAtualizacao != null)
					RecalcularRota(ref lstAgendamentosVigentes, solicitacaoPosteriorPosAtualizacao.Solicitacao);
				//Se agenda atual é a RotaDeVolta e não é a unica do dia, então a solicitação anterior deve ser recalculada e "remover os valores de Rota de Volta"
				if (solAtual.Solicitacao.IndRotaDeVolta && lstAgendamentosVigentes.Count > 1)
				{
					// recalcula a anterior para remover o km de volta
					var solicitacaoAnterior = lstAgendamentosVigentes.ElementAtOrDefault(solIndex - 1);
					if (tipoAgendamentoEnum == TipoAgendamentoEnum.Reagendar && solicitacaoAnterior == null) // se for reagendamento a primeira do dia pode ser ela mesma
						return;

					RecalcularRota(ref lstAgendamentosVigentes, solicitacaoAnterior.Solicitacao);
				}
			});
		}
		public void SalvarAtividadeInformarRotaRealizada(int id, TipoOpcaoInformarRota tipoOpcaoIntinerario, string txtJustificativaDeslocamentoRealizado, decimal? deslocamentoRealizado, DateTime? dataAgenda)
		{
			TryCatch(() =>
			{
				var solicitacao = Buscar(id);
				switch (tipoOpcaoIntinerario)
				{
					case TipoOpcaoInformarRota.RealizadoDeslocamentoKmSistema:
						solicitacao.TipoRotaVistoriaRealizada = solicitacao.TipoRotaVistoriaPrevista;
						solicitacao.DeslocamentoRealizado = solicitacao.DeslocamentoPrevisto;
						solicitacao.CustoDeslocamentoRealizado = solicitacao.CustoDeslocamentoPrevisto;
						solicitacao.CustoTotalRealizado = solicitacao.CustoTotalPrevisto;
						break;
					case TipoOpcaoInformarRota.RealizadoDeslocamentoKmDiferente:
						solicitacao.TipoRotaVistoriaRealizada = solicitacao.TipoRotaVistoriaPrevista;
						solicitacao.DeslocamentoRealizado = deslocamentoRealizado;
						solicitacao.CustoDeslocamentoRealizado = solicitacao.DeslocamentoRealizado * solicitacao.VlrQuilometroRodado;
						solicitacao.CustoTotalRealizado = solicitacao.CustoDeslocamentoRealizado + solicitacao.VlrPagamentoVistoria;
						solicitacao.TxtJustificativaDeslocamentoRealizado = txtJustificativaDeslocamentoRealizado;
						break;
					case TipoOpcaoInformarRota.RealizadoDeslocamentoIntinerarioDiferente:
						SalvarAtividadeAgendar(solicitacao, TipoAgendamentoEnum.Reagendar, dataAgenda, "Realizado Intinerario Diferente");
						solicitacao.TipoRotaVistoriaRealizada = solicitacao.TipoRotaVistoriaPrevista;
						solicitacao.DeslocamentoRealizado = deslocamentoRealizado;
						solicitacao.CustoDeslocamentoRealizado = solicitacao.DeslocamentoRealizado * solicitacao.VlrQuilometroRodado;
						solicitacao.CustoTotalRealizado = solicitacao.CustoDeslocamentoRealizado + solicitacao.VlrPagamentoVistoria;
						solicitacao.TxtJustificativaDeslocamentoRealizado = txtJustificativaDeslocamentoRealizado;
						break;
					default:
						throw new NotImplementedException();
				}
				_atividadeService.Concluir(TipoAtividadeEnum.PrestacaoContaKm, solicitacao);
				_solicitacaoRepositorio.Update(solicitacao);
			});
		}
		public void SalvarAtividadeInformarAgendamento(int id, TipoNotificacaoEnum rbtTipoNotificacao, Comunicacao comunicacaoAssuntoTexto)
		{
			TryCatch(() =>
			{
				var solicitacao = _solicitacaoRepositorio
											.Include(i => i.Seguradora)
											.Include(i => i.Agendamento)
											.GetById(id);
				// verifica se seguradora recebe por email
				var IndAgendaRepostaPorEmail = solicitacao.Seguradora.IndAgendaRepostaPorEmail;
				// campos da atividade
				solicitacao.IndRelacionamentoAgendaInformada = true;
				solicitacao.NomeOperadorAgendaInformada = _usuarioService.UsuarioAutenticado.NomeOperador;
				solicitacao.DthRelacionamentoAgendaInformada = DateTime.Now;
				solicitacao.TipoNotificacaoAgendaInformada = IndAgendaRepostaPorEmail ? TipoNotificacaoEnum.EmailSistemaAuto : rbtTipoNotificacao;
				_atividadeService.Concluir(TipoAtividadeEnum.InformarAgendamento, solicitacao);
				_solicitacaoRepositorio.Update(solicitacao);
				if (IndAgendaRepostaPorEmail)
				{
					comunicacaoAssuntoTexto.IdSolicitacao = solicitacao.Id;
					comunicacaoAssuntoTexto.TipoComunicacao = TipoComunicacaoEnum.ContatoSeguradora;
					var strDataHora = AgendamentoVigenteSolicitacao(solicitacao).DthAgendamento.FormatoDataHora();
					comunicacaoAssuntoTexto.TextoComunicacao += Environment.NewLine + "Data e Hora Agendado: {0}".Formata(strDataHora);
					RegistrarComunicacao(comunicacaoAssuntoTexto, true);
				}
			});
		}
		public void ValorizarFinanceiro(int id)
		{
			TryCatch(() =>
			{
				var solic = Buscar(id);
				InserirLancamentosReceita(solic);
				InserirLancamentosDespesaVistoriador(solic);
				var tes = "nada";
				if (tes == "")
				{
					solic.LancamentoFinanceiro = solic.LancamentoFinanceiro.Where(w => w.TipoLancamentoFinanceiro != TipoLancamentoFinanceiroEnum.ReceitaProdutoReceberSeguradora).ToList();
					_solicitacaoRepositorio.Update(solic);
				}
			});
		}
		public void SalvarAtividadeCheckList(int id, decimal? areaConstruida, int? blocoConstruido)
		{
			TryCatch(() =>
			{
				var solic = Buscar(id);
				solic.AreaConstruida = areaConstruida;
				solic.BlocoConstruido = blocoConstruido;
			});
		}
		public void SalvarAtividadeLaudoAnalista(int id, IFormFile arquivolaudoanalista, decimal? areaConstruida, int? blocoConstruido, int? casaConstruida, int? qtdEquipamento, bool? indRelatorioExigenciaMelhoria)
		{
			TryCatch(() =>
			{
				var solic = _solicitacaoRepositorio.BuscarComContrato(id).Result;


				var lstParamObrig = _contratoService.ParametrosObrigatorios(solic.Produto.Contrato);
				if (lstParamObrig.Contains(TipoContratoParametroEnum.AreaConstruida))
					solic.AreaConstruida = areaConstruida;
				if (lstParamObrig.Contains(TipoContratoParametroEnum.BlocoConstruido))
					solic.BlocoConstruido = blocoConstruido;
				if (lstParamObrig.Contains(TipoContratoParametroEnum.CasaConstruida))
					solic.CasaConstruida = casaConstruida;
				if (lstParamObrig.Contains(TipoContratoParametroEnum.Equipamento))
					solic.QtdEquipamento = qtdEquipamento;
				if (lstParamObrig.Contains(TipoContratoParametroEnum.RelatorioMelhoria))
					solic.IndRelatorioExigenciaMelhoria = indRelatorioExigenciaMelhoria ?? false;

				_atividadeService.Concluir(TipoAtividadeEnum.ElaborarEnviarLaudo, solic);

				if (arquivolaudoanalista != null)
					_arquivoAnexoService.EnviarArquivoSolicitacao(id, TipoArquivoAnexoEnum.LaudoAnalise, arquivolaudoanalista);
			});
		}
		public void SalvarAtividadeRealizarVistoria(int id)
		{
			TryCatch(() =>
			{
				var solic = _solicitacaoRepositorio.Find(id);
				_atividadeService.Concluir(TipoAtividadeEnum.RealizarVistoria, solic);
			});
		}
		#endregion
		#region Acoes
		public void RegistrarComunicacao(Comunicacao entidade, bool indEnviarEmail)
		{
			_comunicacaoService.Salvar(entidade);
			if (indEnviarEmail)
			{
				var solicitacao = _solicitacaoRepositorio
												.Include(i => i.Seguradora)
												.Include(i => i.Vistoriador).ThenInclude(e => e.Operador)
												.GetById(entidade.IdSolicitacao);
				var strEmailDestinatario = "";
				if (entidade.TipoComunicacao == TipoComunicacaoEnum.ContatoSeguradora)
				{
					strEmailDestinatario = solicitacao.Seguradora.EmailRemetenteSolicitacao;
				}
				else if (entidade.TipoComunicacao == TipoComunicacaoEnum.ContatoVistoriador)
				{
					strEmailDestinatario = solicitacao.Vistoriador.Operador.Email;
				}
				if (strEmailDestinatario.IsNullOrEmpty())
					throw new ValidationException(MensagensValidacaoServicos.SolicitacaoRegistrarComunicacaoNaoHaEmail);
				_emailService.Enviar(entidade.Assunto, entidade.TextoComunicacao, new List<string> { strEmailDestinatario });
			}
		}
		#endregion
		#region Regras de Negócio  
		private void RN_ValidarExisteValoresCustoVistoriador(Solicitacao solicitacao)
		{
			if (!solicitacao.IndCidadeBaseVistoriador.Value && solicitacao.IndCustoVistoriaAcordado == false && !solicitacao.CustoDeslocamentoRealizado.HasValue)
			{
				throw new ValidationException(MensagensValidacaoServicos.SolicitacaoRnInserirLancamentosCustoDeslocamentoRealizado);
			}
			else if (solicitacao.IndCustoVistoriaAcordado == true && !solicitacao.CustoTotalAcordado.HasValue)
			{
				throw new ValidationException(MensagensValidacaoServicos.SolicitacaoRnInserirLancamentosCustoDeslocamentoAcordado);
			}
		}
		private void RN_ValidarCodSeguradoraUnico(int idSolicitacao, int idSeguradora, string codSeguradora)
		{
			codSeguradora = codSeguradora.Trim();
			var solic = _solicitacaoRepositorio.FirstOrDefault(w => w.Produto.IdSeguradora == idSeguradora && w.CodSeguradora == codSeguradora && w.Id != idSolicitacao);
			if (solic != null)
				throw new ValidationException(MensagensValidacaoServicos.SolicitacaoRnCadastroDuplicado);
		}
		private void RN_ValidarExisteContratoNoProduto(int idProduto)
		{

			if (!_produtoService.ExisteDadosFinanceiros(idProduto))
				throw new ValidationException(MensagensValidacaoServicos.SolicitacaoRnProdutoSemContrato);
		}
		public void RN_ValidarExiteAgendamento(Solicitacao solicitacao)
		{
			var agendamento = AgendamentoVigenteSolicitacao(solicitacao);
			if (agendamento == null)
				throw new ValidationException(MensagensValidacaoServicos.SolicitacaoRnValidarExiteAgendamento);
		}
		public void RN_ValidarExiteInspecaoRealizada(Solicitacao solicitacao)
		{
			var atividade = _atividadeService.Buscar(TipoAtividadeEnum.RealizarVistoria, solicitacao);
			if (atividade != null && atividade.TipoSituacaoAtividade != TipoSituacaoAtividadeEnum.Concluida)
				throw new ValidationException(MensagensValidacaoServicos.SolicitacaoRnValidarExiteInspecaoRealizada);
		}
		public void RN_ValidarExiteVistoriadorDefinido(Solicitacao solicitacao)
		{
			// TODO mudar para validar pela atividades
			if (!solicitacao.IdVistoriador.HasValue)
			{
				throw new ValidationException(MensagensValidacaoServicos.SolicitacaoRnEnviarParaVistoriador);
			}
		}
		public void RN_ValidarExiteLaudoConcluido(Solicitacao solicitacao)
		{
			var atividade = _atividadeService.Buscar(TipoAtividadeEnum.ElaborarEnviarLaudo, solicitacao);
			if (atividade != null && atividade.TipoSituacaoAtividade != TipoSituacaoAtividadeEnum.Concluida)
				throw new ValidationException(MensagensValidacaoServicos.SolicitacaoRnValidarExiteLaudoConcluido);
		}
		#endregion
		#region Metodos Auxiliares
		public void PreAcordoCustoDeslocamento(int id, decimal custoDeslocamentoAcordado)
		{
			TryCatch(() =>
			{
				var solic = Buscar(id);
				solic.IndCustoVistoriaAcordado = true;
				solic.CustoDeslocamentoAcordado = custoDeslocamentoAcordado;
				solic.CustoTotalAcordado = solic.CustoDeslocamentoAcordado ?? 0 + solic.VlrPagamentoVistoriaAcordado;
				_solicitacaoRepositorio.Update(solic);
			});
		}
		public void PreAcordoVlrPagamentoVistoriaAcordado(int id, decimal vlrPagamentoVistoriaAcordado)
		{
			TryCatch(() =>
			{
				var solic = Buscar(id);
				solic.IndCustoVistoriaAcordado = true;
				solic.VlrPagamentoVistoriaAcordado = vlrPagamentoVistoriaAcordado;
				solic.CustoTotalAcordado = solic.CustoDeslocamentoAcordado ?? 0 + solic.VlrPagamentoVistoriaAcordado;
				_solicitacaoRepositorio.Update(solic);
			});
		}
		/// <summary>
		/// Metodo recalcula a arota a partir da agenda anterior. Para realizar isso é sempre ordenado por data de vistoria e então verificado qual é o horario anterior
		/// </summary>
		/// <param name="lstAgendamentosVistoriadorDia"></param>
		/// <param name="solicitacao"></param>
		private void RecalcularRota(ref List<Agendamento> lstAgendamentosVistoriadorDia, Solicitacao solicitacao)
		{
			lstAgendamentosVistoriadorDia = lstAgendamentosVistoriadorDia.OrderBy(o => o.DthAgendamento).ToList();
			var vistoriador = solicitacao.Vistoriador;
			Endereco enderecoRotaAnterior;
			int indexAgenda = lstAgendamentosVistoriadorDia.FindIndex(a => a.Solicitacao == solicitacao);
			OperadorDistancia operadorDistancia;
			//verifica se é cancelamento(-1)  unica(0) ou a primeira(0) Se é então recalcula a partir do Endereco BASE.
			if (indexAgenda == -1 || indexAgenda == 0)
			{
				solicitacao.TipoRotaVistoriaPrevista = TipoRotaVistoriaEnum.CidadeBase;
				enderecoRotaAnterior = vistoriador.EnderecoBase;
			}
			else // Recalcula a partir da Anterior
			{
				solicitacao.TipoRotaVistoriaPrevista = TipoRotaVistoriaEnum.EntreVistoria;
				enderecoRotaAnterior = lstAgendamentosVistoriadorDia.ElementAt(indexAgenda - 1).Solicitacao.Endereco;
			}
			operadorDistancia = _operadorService.MontarOperadorDistanciaRota(vistoriador, enderecoRotaAnterior, solicitacao.Endereco.SiglaUf, solicitacao.Endereco.NomeMunicipio,
																				solicitacao.Endereco.Latitude.Value, solicitacao.Endereco.Longitude.Value, solicitacao.Id, solicitacao.IdProduto);
			solicitacao.DeslocamentoPrevisto = Convert.ToDecimal(operadorDistancia.DistanciaRota.Value);
			solicitacao.CustoDeslocamentoPrevisto = operadorDistancia.VlrTotalQuilometroRodado;
			solicitacao.CustoTotalPrevisto = operadorDistancia.VlrTotalQuilometroRodadoMaisPagamentoVistoria;
			solicitacao.IndRotaDeVolta = false;
			//verifica se é o ultimo da lista e traça rota de volta a base
			if (indexAgenda + 1 == lstAgendamentosVistoriadorDia.Count)
			{
				Endereco endBaseVolta = vistoriador.EnderecoBase;
				var operadorDistanciaVolta = _operadorService.MontarOperadorDistanciaRota(vistoriador, solicitacao.Endereco, endBaseVolta.SiglaUf, endBaseVolta.NomeMunicipio,
																						endBaseVolta.Latitude.Value, endBaseVolta.Longitude.Value, solicitacao.Id, solicitacao.IdProduto);
				solicitacao.DeslocamentoPrevisto += Convert.ToDecimal(operadorDistanciaVolta.DistanciaRota.Value);
				solicitacao.CustoDeslocamentoPrevisto += operadorDistanciaVolta.VlrTotalQuilometroRodado;
				solicitacao.CustoTotalPrevisto += operadorDistanciaVolta.VlrTotalQuilometroRodadoMaisPagamentoVistoria;
				solicitacao.IndRotaDeVolta = true;
			}
		}
		public string RotaAnteriorUrlMapa(Solicitacao solicitacao)
		{
			var agendamentoVigente = AgendamentoVigenteSolicitacao(solicitacao);
			Endereco enderecoRotaAnterior;
			if (agendamentoVigente != null)
			{
				var dthAgendamentoVigente = agendamentoVigente.DthAgendamento.Value;
				var vistoriador = solicitacao.Vistoriador;
				var lstAgendamentosVistoriadorDia = _agendamentoService.ListarAgendamentosVistoriadorDiaVigentes(vistoriador.Id, dthAgendamentoVigente).ToList();
				int indexAgenda = lstAgendamentosVistoriadorDia.FindIndex(a => a.Solicitacao == solicitacao);
				//verifica se é cancelamento(-1)  unica(0) ou a primeira(0), se sim pega Endereço Base Vistoriador
				if (indexAgenda == -1 || indexAgenda == 0)
					enderecoRotaAnterior = vistoriador.EnderecoBase;
				else
					enderecoRotaAnterior = RotaAnteriorSolicitacao(solicitacao).Endereco;
				return _enderecoService.URLMapaRota(enderecoRotaAnterior.Latitude.Value, enderecoRotaAnterior.Longitude.Value, solicitacao.Endereco.Latitude.Value, solicitacao.Endereco.Longitude.Value);
			}
			return string.Empty;
		}
		public string RotaDeVoltaUrlMapa(Solicitacao solicitacao)
		{
			Endereco enderecoRotaAnterior;
			if (solicitacao.IndRotaDeVolta)
			{
				enderecoRotaAnterior = solicitacao.Endereco;
				return _enderecoService.URLMapaRota(enderecoRotaAnterior.Latitude.Value, enderecoRotaAnterior.Longitude.Value,
														solicitacao.Vistoriador.EnderecoBase.Latitude.Value, solicitacao.Vistoriador.EnderecoBase.Longitude.Value);
			}
			return string.Empty;
		}
		public Solicitacao RotaAnteriorSolicitacao(Solicitacao solicitacao)
		{
			var vistoriador = solicitacao.Vistoriador;
			var agendamentoRealizado = AgendamentoVigenteSolicitacao(solicitacao);
			if (agendamentoRealizado == null)
			{
				throw new ValidationException("Solicitação atual não tem agendamento realizado previamente.");
			}
			else
			{
				var dateAgendamentoRealizado = agendamentoRealizado.DthAgendamento.Value;
				var lstAgendamentosVistoriadorDia = _agendamentoService.ListarAgendamentosVistoriadorDiaVigentes(vistoriador.Id, dateAgendamentoRealizado).ToList();
				int indexAgenda = lstAgendamentosVistoriadorDia.FindIndex(a => a.IdSolicitacao == solicitacao.Id);
				var solicitacaoAnterior = lstAgendamentosVistoriadorDia.ElementAt(indexAgenda - 1).Solicitacao;
				return solicitacaoAnterior;
			}
		}
		private void EnviarEmailSolicitacaoVistoriador(Solicitacao solicitacao, string textoMovimentacao, IFormFile arquivo)
		{
			var lstAnexo = new List<Attachment>();
			if (arquivo != null)
			{
				_arquivoAnexoService.EnviarArquivoSolicitacao(solicitacao.Id, TipoArquivoAnexoEnum.PedidoSeguradora, arquivo);
				arquivo.OpenReadStream().Seek(0, SeekOrigin.Begin);
				lstAnexo.Add(new Attachment(arquivo.OpenReadStream(), arquivo.FileName));
			}

			if (solicitacao?.Vistoriador?.Operador?.Email == null)
				throw new InvalidOperationException("Operador Vistoriador não preenchido");

			var tokenTransacao = CryptographyTDES.EncryptString(solicitacao.Vistoriador.Operador.Email, "emailVistoriador");
			var dominioApp = _configuracaoAplicativo.DominioAplicativo;
			var Endereco = solicitacao.Endereco;
			StringBuilder sb = new StringBuilder(Email.HtmlEmailSolicitacaoVistoria);
			sb = sb.Replace("{prazoentrega}", DateTime.Now.Date.AddDays(2).ToShortDateString());
			sb = sb.Replace("{data-maxima-retorno}", DateTime.Now.Date.AddDays(2).ToShortDateString());
			sb = sb.Replace("{mensagem-movimento}", textoMovimentacao);
			sb = sb.Replace("{link-agendamento}", $"{dominioApp}/Solicitacao/Editar/{solicitacao.Id}?acao=agendar&token={tokenTransacao}");
			sb = sb.Replace("{link-apropriar}", $"{dominioApp}/Solicitacao/Editar/{solicitacao.Id}?acao=apropriar&token={tokenTransacao}");
			sb = sb.Replace("{link-fotos}", $"{dominioApp}/Solicitacao/Editar/{solicitacao.Id}?acao=fotos&token={tokenTransacao}");
			sb = sb.Replace("{nome-seguradora}", solicitacao.Produto.Seguradora.NomeSeguradora);
			sb = sb.Replace("{idsolicitacao}", solicitacao.Id.ToString());
			sb = sb.Replace("{codseguradora}", solicitacao.CodSeguradora);
			sb = sb.Replace("{data-solicitacao}", solicitacao.DataCadastro.ToShortDateString());
			sb = sb.Replace("{nome-corretor}", solicitacao.CorretorNome);
			sb = sb.Replace("{telefone-corretor}", solicitacao.CorretorTelefone);
			sb = sb.Replace("{nome-razaosocial}", solicitacao.Cliente.NomeRazaoSocial);
			sb = sb.Replace("{cpf-cnpj}", solicitacao.Cliente.CpfCnpj);
			sb = sb.Replace("{nome-contato}", solicitacao.Cliente.ContatoNome);
			sb = sb.Replace("{telefone-contato}", solicitacao.Cliente.ContatoTelefone);
			sb = sb.Replace("{endereco}", string.Format("{0}, {1} {2}- {3}", Endereco.Logradouro, Endereco.Numero, Endereco.Complemento, Endereco.Bairro));
			sb = sb.Replace("{cep}", Endereco.Cep);
			sb = sb.Replace("{municipio}", Endereco.NomeMunicipio);
			sb = sb.Replace("{siglauf}", Endereco.SiglaUf);
			sb = sb.Replace("{atividade}", solicitacao.Cliente.AtividadeNome);
			var strHtmlTrCobertura = string.Empty;
			foreach (var cobertura in solicitacao.Cobertura)
			{
				strHtmlTrCobertura += string.Format("<tr> <td>{0}</td> <td>{1}</td> </tr>", cobertura.NomeCobertura, cobertura.VlrCobertura.FormatoMoeda());
			}
			sb = sb.Replace("{cobertura}", strHtmlTrCobertura);
			_emailService.Enviar(string.Format("Solicitação de Inspeção nº: {0} - {1}", solicitacao.Id, solicitacao.Produto.Seguradora.NomeSeguradora), sb.ToString(), new List<string> { solicitacao.Vistoriador.Operador.Email }, lstAnexos: lstAnexo);
		}

		/// <summary>
		///  Essa função tem um controle por "idempotência" -- Uma vez enviado no dia não executa novamente;
		/// </summary>
		/// <param name="solicitacao"></param>
		public void EnviarEmailCobrancaVistoria(SolicitacaoCobrancaVistoriaDao solicitacao, int usuarioServiceId)
		{

			if (solicitacao.ControleDthEmailCobrancaVistoria.HasValue
			   && solicitacao.ControleDthEmailCobrancaVistoria.Value.Date == DateTime.Now.Date)
				return;

			StringBuilder sb = new StringBuilder(Email.HtmlEmailCobrancaVistoria);
			sb = sb.Replace("{idsolicitacao}", solicitacao.Id.ToString());
			sb = sb.Replace("{codseguradora}", solicitacao.CodSeguradora);
			sb = sb.Replace("{nome-razaosocial}", solicitacao.NomeRazaoSocial);
			sb = sb.Replace("{endereco}", $"{solicitacao.Logradouro}, {solicitacao.Numero} {solicitacao.Complemento}");
			sb = sb.Replace("{bairro}", solicitacao.Bairro);
			sb = sb.Replace("{cep}", solicitacao.Cep);
			sb = sb.Replace("{municipio}", solicitacao.NomeMunicipio);
			sb = sb.Replace("{siglauf}", solicitacao.SiglaUf);

			var assunto = $"COBRANÇA DE AGENDAMENTO DA INSPEÇÃO Nº {solicitacao.Id} - {solicitacao.NomeRazaoSocial}";
			_emailService.Enviar(assunto, sb.ToString(), new List<string> { solicitacao.EmailOperador });

			var entity = _solicitacaoRepositorio.Find(solicitacao.Id);
			entity.ControleDthEmailCobrancaVistoria = DateTime.Now;
			_solicitacaoRepositorio.Update(entity);

			SaveChange(usuarioServiceId);
		}

		public Agendamento AgendamentoVigenteSolicitacao(Solicitacao solicitacao)
		{
			return solicitacao.Agendamento.LastOrDefault(w => w.IndCancelado == false && w.TipoAgendamento != TipoAgendamentoEnum.Comunicar);
		}


		#endregion
		public Solicitacao Reinspecao(int id)
		{
			var original = BuscarUI(id).Result;

			//Clonar Dados Solicitacao
			var reinspecao = original.ShallowClone();
			reinspecao.Id = 0;
			reinspecao.IdSolicitacaoOrigemReinspecao = original.Id;

			//Reset Dados Solicitacao
			reinspecao.CodSistemaLegado = 0;

			//Reset Relacionamento 1 para 1
			reinspecao.IdVistoriador = null;
			reinspecao.Vistoriador = null;
			reinspecao.IdAnalista = null;
			reinspecao.Analista = null;
			reinspecao.CustoDeslocamentoPrevisto = null;

			reinspecao.AreaConstruida = null;
			reinspecao.BlocoConstruido = null;
			reinspecao.CasaConstruida = null;
			reinspecao.CustoDeslocamentoAcordado = null;
			reinspecao.CustoDeslocamentoPrevisto = null;
			reinspecao.CustoDeslocamentoRealizado = null;
			reinspecao.CustoTotalAcordado = null;
			reinspecao.CustoTotalPrevisto = null;
			reinspecao.CustoTotalRealizado = null;
			reinspecao.DeslocamentoPrevisto = null;
			reinspecao.DeslocamentoRealizado = null;
			reinspecao.DthRelacionamentoAgendaInformada = null;
			reinspecao.DthVistoriaRealizada = null;
			reinspecao.IndCustoVistoriaAcordado = false;
			reinspecao.IndRelacionamentoAgendaInformada = false;
			reinspecao.IndRelatorioExigenciaMelhoria = false;
			reinspecao.IndRotaDeVolta = false;
			reinspecao.IndUrgente = false;
			reinspecao.LancamentoFinanceiro = null;
			reinspecao.QtdEquipamento = null;
			reinspecao.TipoRotaVistoriaPrevista = null;
			reinspecao.TipoRotaVistoriaRealizada = null;
			reinspecao.TxtJustificativaAnalistaDefinido = null;
			reinspecao.TxtJustificativaDeslocamentoRealizado = null;
			reinspecao.TxtJustificativaVistoriadorDefinido = null;
			reinspecao.VistoriadorCidadeBase = null;
			reinspecao.VlrPagamentoVistoria = null;
			reinspecao.VlrPagamentoVistoriaAcordado = null;
			reinspecao.VlrQuilometroRodado = null;
			reinspecao.VlrRiscoSegurado = original.VlrRiscoSegurado;

			reinspecao.NomeOperadorAgendaInformada = null;
			reinspecao.DthRelacionamentoAgendaInformada = null;
			reinspecao.TipoNotificacaoAgendaInformada = null;

			//Manter Relacionamento 1 para 1
			reinspecao.Cliente = original.Cliente;
			reinspecao.Endereco = original.Cliente.ClienteEndereco.First(i => i.IdEndereco == reinspecao.IdEnderecoCliente).Endereco;


			//Manter Relacionamento 1 para Muitos
			original.Cobertura.ToList().ForEach((cobertura) =>
			{

				var coberturaClone = (Cobertura)cobertura.Clone();

				coberturaClone.VlrCobertura = cobertura.VlrCobertura;
				reinspecao.Cobertura.Add(coberturaClone);
			});
			_coberturaService.Salvar(reinspecao.Cobertura, reinspecao.Id);


			//Processo - Em Elaboração           
			_workflow.Criar(reinspecao);
			_solicitacaoRepositorio.Add(reinspecao);
			return reinspecao;
		}


		public async Task<Solicitacao> BuscarUI(int id)
		{
			return await _solicitacaoRepositorio.BuscarUI(id);
		}

		public Solicitacao BuscarParaAgendar(int id) => _solicitacaoRepositorio.Include(i => i.Cliente)
																				.Include(i => i.Vistoriador)
																				.GetById(id);
		public Solicitacao BuscarParaInformarAgendamento(int id) => _solicitacaoRepositorio.Include(i => i.Cliente)
																				.Include(i => i.Vistoriador)
																				.Include(i => i.Seguradora)
																				.Include(i => i.Agendamento)
																				.GetById(id);

		public void CobrarVistoria(int usuarioServiceId)
		{

			var lst = _solicitacaoQueries.SolicitacoesCobrancaVistoria();
			lst.ForEach(cobranca =>
			{
				EnviarEmailCobrancaVistoria(cobranca, usuarioServiceId);
			});

		}

		public Solicitacao BuscarSolicitacaoEndereco(int id)
		{
			return _solicitacaoRepositorio.Include(i => i.Endereco)
										  .GetById(id);
		}
		public Solicitacao BuscarComMovimento(int id)
		{
			return _solicitacaoRepositorio.Include(i => i.MovimentacaoProcesso).ThenInclude(e => e.OperadorOrigem)
										  .GetById(id);
		}

		//      public void Salvar(Solicitacao entidade)
		//      {
		//          throw new NotImplementedException(); // por conta da impelemnetãção da IDashboar, q deve ser refatorada e separada desssas implementaçoes aqui
		//      }

		//void IBaseService<Solicitacao, SolicitacaoFilter>.Excluir(int id)
		//{
		//	throw new NotImplementedException();
		//}
	}
}
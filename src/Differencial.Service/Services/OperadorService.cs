using Differencial.Domain;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Contracts.Util;
using Differencial.Domain.DTO;
using Differencial.Domain.Entities;
using Differencial.Domain.Exceptions;
using Differencial.Domain.Filters;
using Differencial.Domain.Resources;
using Differencial.Domain.UOW;
using Differencial.Domain.Util;
using Differencial.Domain.Util.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differencial.Service.Services
{
	public class OperadorService : Service, IOperadorService
	{
		IOperadorRepository _operadorRepositorio;
		IVistoriadorRepository _vistoriadorRepositorio;
		IEnderecoService _enderecoService;
		IVistoriadorService _vistoriadorService;
		IConfiguracaoAplicativo _configuracaoAplicativo;
		ISolicitanteService _solicitanteService;
		IAnalistaService _analistaService;
		IEmailService _emailService;

		public OperadorService(IUnitOfWork uow, IOperadorRepository operadorRepositorio,
			IVistoriadorRepository vistoriadorRepositorio,
			IConfiguracaoAplicativo configuracaoAplicativo,
			IEnderecoService enderecoService,
			IVistoriadorService vistoriadorService,
			ISolicitanteService solicitanteService,
			IAnalistaService analistaService,
			IEmailService emailService)
			: base(uow)
		{
			_operadorRepositorio = operadorRepositorio;
			_vistoriadorRepositorio = vistoriadorRepositorio;
			_enderecoService = enderecoService;
			_vistoriadorService = vistoriadorService;
			_configuracaoAplicativo = configuracaoAplicativo;
			_solicitanteService = solicitanteService;
			_analistaService = analistaService;
			_emailService = emailService;
		}

		public IEnumerable<Operador> Listar(OperadorFilter filtro)
		{
			return TryCatch(() =>
			{
				return _operadorRepositorio.Listar(filtro);
			});
		}
		public IEnumerable<Operador> ListarOperadorCadastro(OperadorFilter filtro)
		{
			return TryCatch(() =>
			{
				return _operadorRepositorio.ListarOperadorCadastro(filtro);
			});
		}

		public async Task Salvar(Operador entidade, IFormFile inputFoto)
		{
			await TryCatchAsync(async () =>
			{
				#region Tratamento Mínimo de Dados
				entidade.Email = entidade.Email.IsNullOrEmpty() ? entidade.Email : entidade.Email.Trim();
				entidade.NomeOperador = entidade.NomeOperador.IsNullOrEmpty() ? entidade.NomeOperador : entidade.NomeOperador.Trim();
				entidade.Telefone = entidade.Telefone.IsNullOrEmpty() ? entidade.Telefone : entidade.Telefone.Trim();
				#endregion  Tratamento Mínimo de Dados

				entidade.Validate();
				RnEmailDuplicado(entidade.Id, entidade.Email);

				if (entidade.Id == 0)
				{
					_enderecoService.Salvar(entidade.Endereco);

					PrepararVistoriador(entidade.IndVistoriador, entidade, entidade.Vistoriador);
					PrepararSolicitante(entidade.IndSolicitante, entidade, entidade.Solicitante);
					PrepararAnalista(entidade.IndAnalista, entidade, entidade.Analista);

					entidade.UrlFoto = await SalvarFoto(entidade.UrlFoto, inputFoto);
					entidade.IndAtivo = true;
					_operadorRepositorio.Add(entidade);
				}
				else
				{

					Operador oldEntidade = await _operadorRepositorio.BuscarParaEditarUpdate(entidade.Id);

					oldEntidade.NomeOperador = entidade.NomeOperador;
					oldEntidade.Cpf = entidade.Cpf;
					oldEntidade.Rg = entidade.Rg;
					oldEntidade.DataNascimento = entidade.DataNascimento;
					oldEntidade.Telefone = entidade.Telefone;
					oldEntidade.Email = entidade.Email;
					oldEntidade.IndAnalista = entidade.IndAnalista;
					oldEntidade.IndGerente = entidade.IndGerente;
					oldEntidade.IndSolicitante = entidade.IndSolicitante;
					oldEntidade.IndVistoriador = entidade.IndVistoriador;
					oldEntidade.IndFinanceiro = entidade.IndFinanceiro;
					oldEntidade.IndAssessor = entidade.IndAssessor;
					oldEntidade.UrlFoto = await SalvarFoto(oldEntidade.UrlFoto, inputFoto);

					_enderecoService.Salvar(entidade.Endereco);

					PrepararVistoriador(entidade.IndVistoriador, oldEntidade, entidade.Vistoriador);
					PrepararSolicitante(entidade.IndSolicitante, oldEntidade, entidade.Solicitante);
					PrepararAnalista(entidade.IndAnalista, oldEntidade, entidade.Analista);

					_operadorRepositorio.Update(oldEntidade);
				}
			});
		}

		private void PrepararAnalista(bool indAnalista, Operador entidade, Analista analista)
		{

			if (indAnalista)
			{

				//Novo  
				if (entidade.Analista == null)
				{
					entidade.Analista = analista ?? new Analista();
				}
				entidade.Analista.IndAtivo = indAnalista;
				_analistaService.Salvar(entidade.Analista);

			}
			else
			{
				//Exclui vistoriador no caso de alteração de cadastro e remoção do papel
				if (entidade.Analista != null && entidade.Analista.Id > 0)
				{
					_analistaService.Excluir(entidade.Analista.Id);
				}
				entidade.Analista = null;
			}
		}

		public void Salvar(Operador entidade)
		{
			Salvar(entidade, null);
		}

		private void PrepararVistoriador(bool indVistoriador, Operador entidade, Vistoriador vistoriador)
		{
			// VISTORIADOR

			if (indVistoriador)
			{

				//Novo Vistoriador
				if (entidade.Vistoriador == null)
				{
					entidade.Vistoriador = vistoriador;
				}
				entidade.Vistoriador.IndAtivo = indVistoriador;

				// SE É ALTERANDO INDICADOR ENDEREÇO BASE
				// É alterado de IndEnderecoBase para Outro
				if (entidade.Vistoriador.IndEnderecoBaseIgual == true && vistoriador.IndEnderecoBaseIgual == false)
				{
					// Limpa dados da interface para não haver edição do endereço base
					vistoriador.IdEnderecoBase = 0;
					vistoriador.EnderecoBase.Id = 0;
					_enderecoService.Salvar(vistoriador.EnderecoBase);
					entidade.Vistoriador.EnderecoBase = vistoriador.EnderecoBase;
				}
				// É alterado de Outro para IndEnderecoBase 
				else if (entidade.Vistoriador.IndEnderecoBaseIgual == false && vistoriador.IndEnderecoBaseIgual == true)
				{
					// Então deve excluir o endereço base outro e atualizalo com mesmo do operador
					_enderecoService.Excluir(entidade.Vistoriador.EnderecoBase.Id);
					entidade.Vistoriador.EnderecoBase = entidade.Endereco;
				}
				// É mantido Indicador == Outro Endereço
				else if (entidade.Vistoriador.IndEnderecoBaseIgual == false && vistoriador.IndEnderecoBaseIgual == false)
				{
					// Então apenas altera dados de endereço

					entidade.Vistoriador.EnderecoBase.Bairro = vistoriador.EnderecoBase.Bairro;
					entidade.Vistoriador.EnderecoBase.Cep = vistoriador.EnderecoBase.Cep;
					entidade.Vistoriador.EnderecoBase.Complemento = vistoriador.EnderecoBase.Complemento;
					entidade.Vistoriador.EnderecoBase.Latitude = vistoriador.EnderecoBase.Latitude;
					entidade.Vistoriador.EnderecoBase.Longitude = vistoriador.EnderecoBase.Longitude;
					entidade.Vistoriador.EnderecoBase.NomeMunicipio = vistoriador.EnderecoBase.NomeMunicipio;
					entidade.Vistoriador.EnderecoBase.Numero = vistoriador.EnderecoBase.Numero;
					entidade.Vistoriador.EnderecoBase.SiglaUf = vistoriador.EnderecoBase.SiglaUf;

					_enderecoService.Salvar(entidade.Vistoriador.EnderecoBase);
				}
				// Se mantem o endereço base igual ao principal
				else if (vistoriador.IndEnderecoBaseIgual == true && vistoriador.IndEnderecoBaseIgual == true)
				{
					entidade.Vistoriador.EnderecoBase = entidade.Endereco;
				}

				entidade.Vistoriador.IndEnderecoBaseIgual = vistoriador.IndEnderecoBaseIgual;
				_vistoriadorService.Salvar(1, entidade.Vistoriador);
			}
			else
			{
				//TODO: Excluir será substituido por indAtivado = false
				//Exclui vistoriador no caso de alteração de cadastro e remoção do papel
				if (entidade.Vistoriador != null && entidade.Vistoriador.Id > 0)
				{
					//if (!entidade.Vistoriador.IndEnderecoBaseIgual)
					//    _enderecoService.Excluir(1, entidade.Vistoriador.EnderecoBase.Id);

					//_vistoriadorService.Excluir(1, entidade.Vistoriador.Id);
					//entidade.IndVistoriador = false;
					entidade.Vistoriador.IndAtivo = false;
				}
				entidade.Vistoriador = null;
			}

		}

		private void PrepararSolicitante(bool indSolicitante, Operador entidade, Solicitante solicitante)
		{

			if (indSolicitante)
			{

				if (entidade.IndAnalista || entidade.IndAssessor || entidade.IndFinanceiro || entidade.IndGerente || entidade.IndVistoriador)
					throw new ValidationException(MensagensValidacaoServicos.SolicitanteRnOperadorExclusivo);


				solicitante.IndAtivo = indSolicitante;
				solicitante.TipoSolicitante = TipoSolicitanteEnum.AcessoAoSistema;
				//Novo  
				if (entidade.Solicitante == null)
				{
					entidade.Solicitante = solicitante;
				}

				entidade.Solicitante.IdSeguradora = solicitante.IdSeguradora;
				_solicitanteService.Salvar(entidade.Solicitante);

			}
			else
			{
				//TODO: Excluir será substituido por indAtivado = false
				//Exclui vistoriador no caso de alteração de cadastro e remoção do papel
				if (entidade.Solicitante != null && entidade.Solicitante.Id > 0)
				{
					_solicitanteService.Excluir(entidade.Vistoriador.Id);
					//entidade.Solicitante.IndAtivo = false;
				}
				entidade.Solicitante = null;
			}
		}

		public void Excluir(int id)
		{
			TryCatch(() =>
			{
				_operadorRepositorio.Delete(id);
			});
		}

		public void Excluir(int[] ids)
		{
			TryCatch(() =>
			{
				_operadorRepositorio.Delete(ids);
			});
		}

		public Operador Buscar(int id)
		{
			return TryCatch(() =>
			 {
				 return _operadorRepositorio.Find(id);
			 });
		}
		public Task<Operador> BuscarParaEditar(int id)
		{
			return TryCatch(() =>
			 {
				 return _operadorRepositorio.BuscarParaEditarView(id);
			 });
		}

		public IEnumerable<OperadorDistancia> ListarOperadorDistanciaPorProximidadeGeo(string siglaUf, string municipio, int idProduto, int idSolicitacao, int? IdContratoLancamentoValor, double latDestino, double longDestino)
		{
			return TryCatch(() =>
			{
				var lstOperadorDistancia = new List<OperadorDistancia>();

				foreach (var operadorDistancia in _vistoriadorService.ListarOperadorDistancia(latDestino, longDestino, idProduto, IdContratoLancamentoValor))
				{
					lstOperadorDistancia.Add(MontarOperadorDistancia(operadorDistancia.Operador.Vistoriador, siglaUf, municipio, latDestino, longDestino, idSolicitacao, idProduto, IdContratoLancamentoValor));
				}
				//TODO: Mudar Ordenação não por VlrPagamento, mas sim por IndAtivo(VistoriadorProduto)
				//return lstOperadorDistancia.OrderByDescending(o => o.VlrPagamentoVistoria.HasValue).ThenByDescending(o => o.IndSolicitacaoMesmaCidade).ThenByDescending(o => o.IndCidadeBase).ThenBy(o => o.VlrTotal).ThenBy(o => o.DistanciaRota).ToList();
				return lstOperadorDistancia.Distinct().OrderBy(o => o.VlrTotalQuilometroRodadoMaisPagamentoVistoria).ThenBy(o => o.DistanciaRota).ToList();
			});
		}

		private OperadorDistancia MontarOperadorDistancia(Vistoriador Vistoriador, string siglaUf, string municipio, double latDestino, double longDestino, int idSolicitacao, int idProduto, int? IdContratoLancamentoValor)
		{



			var operadorDistancia = new OperadorDistancia
			{
				Id = Vistoriador.Id,
				NomeOperador = Vistoriador.Operador.NomeOperador
			};


			var EnderecoBase = Vistoriador.EnderecoBase;
			operadorDistancia.NomeMunicipioSiglaUf = string.Format("{0} ({1})", EnderecoBase.NomeMunicipio, EnderecoBase.SiglaUf);


			operadorDistancia.DistanciaRota = _enderecoService.DistanciaRota(EnderecoBase.Latitude.Value, EnderecoBase.Longitude.Value, latDestino, longDestino);



			operadorDistancia.UrlVerMapa = string.Format("http://maps.google.com.br/maps?saddr={0}&daddr={1}&ie=UTF8&t=m&z=5&layer=m&mode=driving&units=metric",
				EnderecoBase.Latitude.Value.ToString(new CultureInfo("en-US")) + ", " + EnderecoBase.Longitude.Value.ToString(new CultureInfo("en-US")),
				string.Format(new CultureInfo("en-US"), "{0}, {1}", latDestino, longDestino));


			if (operadorDistancia.DistanciaRota.HasValue)
			{
				operadorDistancia.DistanciaRota = Math.Round(operadorDistancia.DistanciaRota.Value, 1);

				operadorDistancia.IndCidadeBase = (EnderecoBase.SiglaUf.ToLower() == siglaUf.ToLower() && EnderecoBase.NomeMunicipio.Igualar() == municipio.Igualar());
				operadorDistancia.IndSolicitacaoMesmaCidade = VerificarHaAgendamento(municipio, siglaUf, Vistoriador.Id, idSolicitacao);
				operadorDistancia.IndHistoricoMesmaCidade = VerificarHaAtendimentoMunicipio(municipio, siglaUf, Vistoriador.Id, idSolicitacao);

				VistoriadorProduto vistoriadorProduto = null;

				if (IdContratoLancamentoValor.HasValue)
				{
					vistoriadorProduto = Vistoriador.VistoriadorProduto.FirstOrDefault(vp => vp.IdProduto == idProduto && vp.IdContratoLancamentoValor == IdContratoLancamentoValor);
				}
				else
				{
					vistoriadorProduto = Vistoriador.VistoriadorProduto.FirstOrDefault(vp => vp.IdProduto == idProduto);
				}

				//(Vistoriador != null && Vistoriador.VistoriadorProduto != null) ??????

				operadorDistancia.VlrQuilometroRodado = vistoriadorProduto != null ? vistoriadorProduto.VlrQuilometroRodado : null;
				operadorDistancia.VlrPagamentoVistoria = vistoriadorProduto != null ? vistoriadorProduto.VlrPagamentoVistoria : null;

				operadorDistancia.VlrTotalQuilometroRodado = operadorDistancia.IndCidadeBase || operadorDistancia.IndSolicitacaoMesmaCidade ? 0 : (decimal)operadorDistancia.DistanciaRota.Value * operadorDistancia.VlrQuilometroRodado;
				operadorDistancia.VlrTotalQuilometroRodadoMaisPagamentoVistoria = operadorDistancia.VlrTotalQuilometroRodado + operadorDistancia.VlrPagamentoVistoria;
			}

			return operadorDistancia;
		}

		public OperadorDistancia MontarOperadorDistanciaRota(Vistoriador Vistoriador, Endereco EnderecoRotaParida, string siglaUf, string municipio, double latDestino, double longDestino, int idSolicitacao, int idProduto)
		{
			var operadorDistancia = new OperadorDistancia();

			operadorDistancia.Id = Vistoriador.Id;
			operadorDistancia.NomeOperador = Vistoriador.Operador.NomeOperador;


			var EnderecoBase = EnderecoRotaParida;
			operadorDistancia.NomeMunicipioSiglaUf = string.Format("{0} ({1})", EnderecoBase.NomeMunicipio, EnderecoBase.SiglaUf);

			operadorDistancia.DistanciaRota = _enderecoService.DistanciaRota(EnderecoBase.Latitude.Value, EnderecoBase.Longitude.Value, latDestino, longDestino);

			operadorDistancia.UrlVerMapa = _enderecoService.URLMapaRota(EnderecoBase.Latitude.Value, EnderecoBase.Longitude.Value, latDestino, longDestino);

			if (operadorDistancia.DistanciaRota.HasValue)
			{
				operadorDistancia.DistanciaRota = Math.Round(operadorDistancia.DistanciaRota.Value, 1);
				//TODO: vER COMO TRATAR MESMA CIDADE OU MUDANCA DE CIDADE E TAMBEM QUANDO HA SOLIC NA MESMA CIDADE;
				// operadorDistancia.IndCidadeBase = (EnderecoBase.SiglaUf.ToLower() == siglaUf.ToLower() && EnderecoBase.NomeMunicipio.Igualar() == municipio.Igualar());
				// operadorDistancia.IndSolicitacaoMesmaCidade = VerificarHaAgendamento(municipio, siglaUf, Vistoriador.Id, idSolicitacao);
				operadorDistancia.IndHistoricoMesmaCidade = VerificarHaAtendimentoMunicipio(municipio, siglaUf, Vistoriador.Id, idSolicitacao);


				var vistoriadorProduto = (Vistoriador != null && Vistoriador.VistoriadorProduto != null) ? Vistoriador.VistoriadorProduto.FirstOrDefault(vp => vp.IdProduto == idProduto) : null;

				operadorDistancia.VlrQuilometroRodado = vistoriadorProduto != null ? vistoriadorProduto.VlrQuilometroRodado : null;
				operadorDistancia.VlrPagamentoVistoria = vistoriadorProduto != null ? vistoriadorProduto.VlrPagamentoVistoria : null;

				operadorDistancia.VlrTotalQuilometroRodado = operadorDistancia.IndCidadeBase ? 0 : (decimal)operadorDistancia.DistanciaRota.Value * operadorDistancia.VlrQuilometroRodado;
				operadorDistancia.VlrTotalQuilometroRodadoMaisPagamentoVistoria = operadorDistancia.VlrTotalQuilometroRodado + operadorDistancia.VlrPagamentoVistoria;
			}

			return operadorDistancia;
		}

		private async Task<string> SalvarFoto(string UrlFotoAtual, IFormFile inputFoto)
		{
			if (inputFoto != null && inputFoto.FileName != null)
			{
				//TODO implementar exclusão da foto anterior
				if (String.IsNullOrEmpty(UrlFotoAtual))
					UrlFotoAtual = Guid.NewGuid() + Path.GetExtension(inputFoto.FileName.ToString());

				string path = _configuracaoAplicativo.RepositorioOperadorImagem;

				await inputFoto.Salvar(path, UrlFotoAtual);

			}
			return UrlFotoAtual;
		}

		public OperadorDistancia BuscarOperadorDistanciaSolicitacao(int idVistoriador, int idSolicitacao, string siglaUf, string municipio, double latDestino, double longDestino, int idProduto, int? IdContratoLancamentoValor)
		{
			var vistoriador = _vistoriadorRepositorio.Buscar(idVistoriador);
			return MontarOperadorDistancia(vistoriador, siglaUf, municipio, latDestino, longDestino, idSolicitacao, idProduto, IdContratoLancamentoValor);
		}

		/// <summary>
		/// Metodo verifica se o vistoriador já realizou vistoria neste mesmo municipio
		/// </summary>
		/// <param name="municipio"></param>
		/// <param name="siglaUf"></param>
		/// <param name="idVistoriador"></param>
		/// <param name="idSolicitacaoDesconsiderar">Desconsidare a solicitação em contexto pois verifica se há outro atendimento</param>
		/// <returns></returns>
		private bool VerificarHaAtendimentoMunicipio(string municipio, string siglaUf, int idVistoriador, int idSolicitacaoDesconsiderar)
		{
			return TryCatch(() =>
			{
				siglaUf = siglaUf.Trim().ToLower();
				municipio = municipio.Trim().ToLower();

				var obj = _operadorRepositorio.Where(i => i.Vistoriador.Id == idVistoriador &&
														i.Vistoriador.Solicitacao.Any(s =>
														s.Endereco.NomeMunicipio.ToLower() == municipio &&
														s.Endereco.SiglaUf.ToLower() == siglaUf &&
														s.IdVistoriador == idVistoriador &&
														s.Id != idSolicitacaoDesconsiderar)).ToList();

				return obj.Count > 0;
			});
		}

		private bool VerificarHaAgendamento(string municipio, string siglaUf, int idVistoriador, int idSolicitacaoDesconsiderar)
		{
			return TryCatch(() =>
			{
				siglaUf = siglaUf.Trim().ToLower();
				municipio = municipio.Trim().ToLower();

				var obj = _operadorRepositorio.Where(i => i.Vistoriador.Id == idVistoriador &&
														i.Vistoriador.Solicitacao.Any(s =>
														s.Endereco.NomeMunicipio.ToLower() == municipio &&
														s.Endereco.SiglaUf.ToLower() == siglaUf &&
														s.IdVistoriador == idVistoriador &&
														s.TpSituacao == Domain.TipoSituacaoProcessoEnum.ApropriadoVistoriador &&
														s.Agendamento.Any(a => a.IndCancelado == false) &&
														s.Id != idSolicitacaoDesconsiderar));

				var resul = obj.ToList();

				return resul.Count > 0;
			});
		}

		public Operador BuscarPorToken(string tokenTransacao)
		{

			return TryCatch(() =>
			{
				tokenTransacao = tokenTransacao.Replace(" ", "+");
				tokenTransacao = CryptographyTDES.DecryptString(tokenTransacao, "emailVistoriador");

				return _operadorRepositorio.FirstOrDefault(o => o.Email == tokenTransacao);
			});
		}

		#region "Regras de Negócio"

		private void RnEmailDuplicado(int idOperador, string email)
		{
			if (idOperador > 0)
			{
				if (_operadorRepositorio.Where(i => i.Email == email && i.Id != idOperador).Any())
					throw new ValidationException(MensagensValidacaoServicos.OperadorRnEmailDuplicado);

			}
			else
			{
				if (_operadorRepositorio.Where(i => i.Email == email).Any())
					throw new ValidationException(MensagensValidacaoServicos.OperadorRnEmailDuplicado);
			}
		}

		#endregion  "Regras de Negócio"

		public void SalvarGerarAcesso(int idOperador)
		{
			TryCatch(() =>
		   {
			   var operador = Buscar(idOperador);
			   if (operador.IndAcessoSistema)
			   {
				   GerarNovoAcesso(operador.Email);
			   }
			   else
			   {

				   GerarPrimeiroAcesso(idOperador);
			   }

		   });
		}
		private void GerarPrimeiroAcesso(int idOperador)
		{
			var operador = Buscar(idOperador);
			operador.IndAcessoSistema = true;
			operador.IndPrimeiroAcesso = true;
			operador.Senha = CryptographyTDES.EncryptString(DateTime.Now.FormatoDataHora(), _configuracaoAplicativo.PrivateKey);
			_operadorRepositorio.Update(operador);

			StringBuilder sb = new StringBuilder(Email.HtmlEmailOperadorGerarPrimeiroAcesso);

			sb = sb.Replace("{link-ativar}", "{0}/Home/AtivarAcesso/{1}?token={2}".Formata(_configuracaoAplicativo.DominioAplicativo, operador.Id.ToString(), operador.Senha));
			sb = sb.Replace("{link-images}", _configuracaoAplicativo.DominioAplicativo);
			sb = sb.Replace("{nomeoperador}", operador.NomeOperador);
			sb = sb.Replace("{username}", operador.Email);

			_emailService.Enviar("Bem-vindo a {0}".Formata(_configuracaoAplicativo.NomeEmpresaCompleto), sb.ToString(), operador.Email);
		}
		public void GerarNovoAcesso(string usuario)
		{
			TryCatch(() =>
			{
				var operador = _operadorRepositorio.FirstOrDefault(i => i.Email == usuario);
				if (!operador.IndAcessoSistema)
					throw new ValidationException(MensagensValidacaoServicos.OperadorRnLogonAcessoSistema);

				operador.Senha = CryptographyTDES.EncryptString(DateTime.Now.FormatoDataHora(), _configuracaoAplicativo.PrivateKey);
				_operadorRepositorio.Update(operador);


				StringBuilder sb = new StringBuilder(Email.HtmlEmailOperadorGerarNovoAcesso);

				sb = sb.Replace("{link-ativar}", "{0}/Home/AtivarAcesso/{1}?token={2}".Formata(_configuracaoAplicativo.DominioAplicativo, operador.Id.ToString(), operador.Senha));
				sb = sb.Replace("{link-images}", _configuracaoAplicativo.DominioAplicativo);
				sb = sb.Replace("{nomeoperador}", operador.NomeOperador);
				sb = sb.Replace("{username}", operador.Email);

				_emailService.Enviar("Solicitado nova senha ao sistema {0}".Formata(_configuracaoAplicativo.NomeEmpresaSimples), sb.ToString(), operador.Email);


			});
		}

		public void SalvarMudarSenha(int idOperador, string senhaAnterior, string senhaNova)
		{
			TryCatch(() =>
			{
				senhaAnterior = senhaAnterior.Replace(" ", "+");
				var operador = _operadorRepositorio.FirstOrDefault(o => o.Id == idOperador && o.Senha == senhaAnterior);
				if (operador == null)
					throw new ValidationException(MensagensValidacaoServicos.OperadorRnMudarSenha);

				operador.IndPrimeiroAcesso = false;
				operador.Senha = CryptographyTDES.EncryptString(senhaNova, _configuracaoAplicativo.PrivateKey);
				_operadorRepositorio.Update(operador);
			});
		}

		public Operador BuscarLogon(string usuario, string senha)
		{
			return TryCatch(() =>
			{
				usuario = usuario.Trim();

				var operador = _operadorRepositorio.FirstOrDefault(i => i.Email == usuario);

				if (operador == null)
					throw new ValidationException(MensagensValidacaoServicos.OperadorRnLogonInvalido);

				if (operador.IndPrimeiroAcesso)
					throw new ValidationException(MensagensValidacaoServicos.OperadorRnLogonPrimeiroAcesso);

				if (!operador.IndAcessoSistema)
					throw new ValidationException(MensagensValidacaoServicos.OperadorRnLogonAcessoSistema);

				if (operador.Senha == CryptographyTDES.EncryptString(senha, _configuracaoAplicativo.PrivateKey))
					return operador;
				else
					throw new ValidationException(MensagensValidacaoServicos.OperadorRnLogonSenhaInvalida);

			});
		}

		public Operador BuscarPorUsuario(string usuario)
		{
			return TryCatch(() =>
			{
				return _operadorRepositorio.FirstOrDefault(i => i.Email == usuario);

			});
		}

		public void SalvarSolicitanteSemAcesso(Operador entidade, int idSeguradora)
		{
			TryCatch(() =>
			{
				entidade.IndSolicitante = true;
				#region Tratamento Mínimo de Dados
				entidade.Email = entidade.Email.IsNullOrEmpty() ? entidade.Email : entidade.Email.Trim();
				entidade.NomeOperador = entidade.NomeOperador.IsNullOrEmpty() ? entidade.NomeOperador : entidade.NomeOperador.Trim();
				entidade.Telefone = entidade.Telefone.IsNullOrEmpty() ? entidade.Telefone : entidade.Telefone.Trim();
				#endregion  Tratamento Mínimo de Dados

				RnEmailDuplicado(entidade.Id, entidade.Email);

				if (entidade.Id == 0)
				{
					entidade.Solicitante = new Solicitante
					{
						IndAtivo = true,
						TipoSolicitante = Domain.TipoSolicitanteEnum.Cadastro,
						IdSeguradora = idSeguradora
					};


					_solicitanteService.Salvar(entidade.Solicitante);
					_operadorRepositorio.Add(entidade);
				}
				else
				{
					Operador oldEntidade = Buscar(entidade.Id);
					oldEntidade.NomeOperador = entidade.NomeOperador;
					oldEntidade.Telefone = entidade.Telefone;
					oldEntidade.Email = entidade.Email;
					_operadorRepositorio.Update(oldEntidade);
				}
			});
		}

		public void ExcluirSolicitante(int[] ids)
		{
			TryCatch(() =>
			{
				foreach (var id in ids)
				{
					_solicitanteService.Excluir(id);
					Excluir(id);
				}
			});
		}
	}
}
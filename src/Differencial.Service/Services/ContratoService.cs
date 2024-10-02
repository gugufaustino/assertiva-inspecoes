using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Collections.Generic;
using Differencial.Domain.Contracts.Entities;
using System.Linq;
using Differencial.Domain;
using System;
using Differencial.Domain.Exceptions;
using Differencial.Domain.Resources;
using Differencial.Domain.Util.ExtensionMethods;


namespace Differencial.Service.Services
{
	public class ContratoService : Service, IContratoService
	{
		IContratoRepository _contratoRepositorio;
		IContratoLancamentoService _contratoLancamentoService;

		ISeguradoraRepository seguradoraRepository;

		public ContratoService(IUnitOfWork uow,
			IContratoRepository contratoRepositorio,
			IContratoLancamentoService contratoLancamentoService,
			ISeguradoraRepository seguradoraRepository)
			: base(uow)
		{
			_contratoRepositorio = contratoRepositorio;
			_contratoLancamentoService = contratoLancamentoService;
			this.seguradoraRepository = seguradoraRepository;
		}

		public IEnumerable<Contrato> Listar(ContratoFilter filtro)
		{
			return TryCatch(() =>
			{
				return _contratoRepositorio.Where(filtro);
			});
		}

		public void Salvar(Contrato entidade)
		{
			TryCatch(() =>
			{
				entidade.Validate();

				_contratoLancamentoService.Salvar(entidade.ContratoLancamento);

				if (entidade.Id == 0)
					_contratoRepositorio.Add(entidade);
				else
				{
					var oldEntidade = Buscar(entidade.Id);
					_contratoRepositorio.Update(oldEntidade);
				}

			});
		}

		public void Excluir(int id)
		{
			TryCatch(() =>
			{
				_contratoRepositorio.Delete(id);
			});
		}

		public Contrato Buscar(int id)
		{
			return TryCatch(() =>
			{
				return _contratoRepositorio.Find(id);
			});
		}

		public Contrato BuscarPorProduto(int idProduto)
		{
			return TryCatch(() =>
			{
				return _contratoRepositorio.BuscarPorProduto(idProduto);
			});
		}

		public Dictionary<TipoContratoParametroEnum, decimal> GerarLancamentosContrato(IContratoInstanciaValorParametro solicitacaoInstancia)
		{
			Dictionary<TipoContratoParametroEnum, decimal> lstLancmto = new Dictionary<TipoContratoParametroEnum, decimal>();
			var contrato = BuscarPorProduto(solicitacaoInstancia.IdProduto);

			foreach (var lancamento in contrato.ContratoLancamento)
			{
				if (lancamento.ContratoLancamentoValor.Any())
				{
					decimal? vlrLancar = null;
					switch (lancamento.TipoParametroQuantitativoVariavel)
					{
						case TipoContratoParametroEnum.Comum:
							vlrLancar = LancamentoFixo(lancamento);

							break;

						case TipoContratoParametroEnum.RelatorioMelhoria:
							if (solicitacaoInstancia.IndRelatorioExigenciaMelhoria)
								vlrLancar = LancamentoFixo(lancamento);

							break;
						case TipoContratoParametroEnum.ValorRisco:
							if (solicitacaoInstancia.VlrRiscoSegurado.HasValue)
								vlrLancar = LancamentoQuantitativoParametro(lancamento, solicitacaoInstancia.VlrRiscoSegurado.Value, solicitacaoInstancia);

							break;
						case TipoContratoParametroEnum.BlocoConstruido:
							if (solicitacaoInstancia.BlocoConstruido.HasValue)
								vlrLancar = LancamentoQuantitativoParametro(lancamento, solicitacaoInstancia.BlocoConstruido.Value, solicitacaoInstancia);

							break;
						case TipoContratoParametroEnum.AreaConstruida:
							if (solicitacaoInstancia.AreaConstruida.HasValue)
								vlrLancar = LancamentoQuantitativoParametro(lancamento, solicitacaoInstancia.AreaConstruida.Value, solicitacaoInstancia);

							break;
						case TipoContratoParametroEnum.Equipamento:
							if (solicitacaoInstancia.QtdEquipamento.HasValue)
								vlrLancar = LancamentoQuantitativoParametro(lancamento, solicitacaoInstancia.QtdEquipamento.Value, solicitacaoInstancia);
							break;

						case TipoContratoParametroEnum.Estado:
							if (lancamento.ContratoLancamentoValor.Any(clv => clv.SiglaUf.ToUpper() == solicitacaoInstancia.Endereco.SiglaUf.ToUpper()))
								vlrLancar = LancamentoEstadoParametro(lancamento, solicitacaoInstancia.Endereco.SiglaUf.ToUpper(), solicitacaoInstancia);
							break;

						default:
							break;
					}

					if (vlrLancar.HasValue && vlrLancar > 0)
						lstLancmto.Add(lancamento.TipoParametroQuantitativoVariavel, vlrLancar.Value);
				}

			}

			//Quilometragem 
			var vlrQuilometragem = LancamentosQuilometragem(contrato.Produto.Seguradora, solicitacaoInstancia.DeslocamentoRealizado);
			if (vlrQuilometragem > 0)
				lstLancmto.Add(TipoContratoParametroEnum.Quilometragem, vlrQuilometragem);

			return lstLancmto;
		}

		private decimal? LancamentoEstadoParametro(ContratoLancamento lancamento, string siglaUf, IContratoInstanciaValorParametro contratLancamentoValor)
		{
			decimal vlrRetorno = lancamento.ContratoLancamentoValor.FirstOrDefault(clv => clv.SiglaUf.ToUpper() == siglaUf).ValorLancamento.Value;
			return vlrRetorno;
		}

		#region Montar Lancamento
		private decimal LancamentosQuilometragem(Seguradora seguradora, decimal? deslocamentoRealizado, bool RnValidar = false)
		{
			if (RnValidar)
				Rn_DeslocamentoRealizado(deslocamentoRealizado);

			decimal vlrRetorno = 0;
			if (seguradora.VlrQuilometroExcedente.HasValue)
			{
				if (seguradora.QtdQuilometroFranquia == 0)
				{
					vlrRetorno = (decimal)deslocamentoRealizado * seguradora.VlrQuilometroExcedente.Value;
					//var descr = $"{deslocamentoRealizado} km * R$ {seguradora.VlrQuilometroExcedente.Value.FormataMoeda()}";
				}
				else if (seguradora.QtdQuilometroFranquia > 0 && deslocamentoRealizado > seguradora.QtdQuilometroFranquia)
				{
					vlrRetorno = (decimal)(deslocamentoRealizado - (decimal)seguradora.QtdQuilometroFranquia) * seguradora.VlrQuilometroExcedente.Value;
					//var descr = $"({deslocamentoRealizado} km - {seguradora.QtdQuilometroFranquia} km franquia) * R$ {seguradora.VlrQuilometroExcedente.Value.FormataMoeda()}";
				}
			}


			return vlrRetorno;
		}

		private decimal LancamentoFixo(ContratoLancamento lancamento)
		{
			return lancamento.ContratoLancamentoValor.FirstOrDefault().ValorLancamento.Value;
		}

		private decimal LancamentoQuantitativoParametro(ContratoLancamento lancamento, decimal? parametroValor, IContratoInstanciaValorParametro solicitacao, bool RnValidar = false)
		{

			var nameParametro = lancamento.TipoParametroQuantitativoVariavel.Display().ShortName;
			if (RnValidar && !parametroValor.HasValue)
			{
				throw new ValidationException("É obrigatório o preenchimento do atributo " + nameParametro);
			}

			decimal vlrRetorno = 0;
			foreach (var contratoValores in lancamento.ContratoLancamentoValor)
			{
				switch (contratoValores.TipoQuantitativoVariacao)
				{
					case TipoQuantitativoVariacaoEnum.UnicoNaoVariavel:
						throw new NotImplementedException();

					case TipoQuantitativoVariacaoEnum.DeAte:
						if (parametroValor >= contratoValores.QuantitativoA && parametroValor <= contratoValores.QuantitativoB)
						{
							vlrRetorno += contratoValores.ValorLancamento.Value;

						}

						break;
					case TipoQuantitativoVariacaoEnum.AcimaDe:
						if (parametroValor > contratoValores.QuantitativoA)
						{
							if (contratoValores.IndPreAcordo)
							{
								if (!solicitacao.VlrHonorarioPreAcordo.HasValue)
									throw new ValidationException("O valor informado para o atributo \"{0}\" exige pré-acordo de honorários junto a Seguradora. O valor estabelecido para os honorários deve ser preenchido no campo \"Valor Honorário Pré Acordo\" ".Formata(nameParametro));
								else
									vlrRetorno += solicitacao.VlrHonorarioPreAcordo.Value;
							}
							else
								vlrRetorno += contratoValores.ValorLancamento.Value;
						}
						break;
					default:
						break;
				}


			}
			return vlrRetorno;
		}

		#endregion Montar Lancamento

		public void ValidarContratoLancamentoValorParametro(IContratoInstanciaValorParametro contratLancamentoValor, TipoContratoParametroEnum tipoContratoParametroValidar)
		{
			var contrato = BuscarPorProduto(contratLancamentoValor.IdProduto);

			var lstObrigatorios = ParametrosObrigatorios(contrato);

			if (lstObrigatorios.Contains(tipoContratoParametroValidar))
			{
				var lancamento = contrato.ContratoLancamento.First(f => f.TipoParametroQuantitativoVariavel == tipoContratoParametroValidar);

				switch (tipoContratoParametroValidar)
				{
					case TipoContratoParametroEnum.ValorRisco:
						LancamentoQuantitativoParametro(lancamento, contratLancamentoValor.VlrRiscoSegurado, contratLancamentoValor, true);

						break;
					case TipoContratoParametroEnum.BlocoConstruido:
						LancamentoQuantitativoParametro(lancamento, contratLancamentoValor.BlocoConstruido, contratLancamentoValor, true);
						break;
					case TipoContratoParametroEnum.CasaConstruida:
						LancamentoQuantitativoParametro(lancamento, contratLancamentoValor.CasaConstruida.Value, contratLancamentoValor, true);
						break;
					case TipoContratoParametroEnum.AreaConstruida:
						LancamentoQuantitativoParametro(lancamento, contratLancamentoValor.AreaConstruida.Value, contratLancamentoValor, true);
						break;
					case TipoContratoParametroEnum.Equipamento:
						LancamentoQuantitativoParametro(lancamento, contratLancamentoValor.QtdEquipamento.Value, contratLancamentoValor, true);
						break;
					default:
						break;
				}
			}
		}

		public List<TipoContratoParametroEnum> ParametrosObrigatorios(Contrato contrato)
		{
			List<TipoContratoParametroEnum> lstParametroObrigatorio = new();


			foreach (var lancamento in contrato.ContratoLancamento)
			{
				if (lancamento.ContratoLancamentoValor.Any())
				{
					switch (lancamento.TipoParametroQuantitativoVariavel)
					{
						case TipoContratoParametroEnum.RelatorioMelhoria:
						case TipoContratoParametroEnum.ValorRisco:
						case TipoContratoParametroEnum.BlocoConstruido:
						case TipoContratoParametroEnum.CasaConstruida:
						case TipoContratoParametroEnum.AreaConstruida:
						case TipoContratoParametroEnum.Equipamento:
							lstParametroObrigatorio.Add(lancamento.TipoParametroQuantitativoVariavel);
							break;
						default:
							break;
					}
				}
			}

			//Quilometragem



			if (contrato.Produto.Seguradora.VlrQuilometroExcedente.HasValue)
				lstParametroObrigatorio.Add(TipoContratoParametroEnum.Quilometragem);

			return lstParametroObrigatorio;
		}
		#region "Regras de Negócio"

		private void Rn_DeslocamentoRealizado(decimal? deslocamentoRealizado)
		{
			if (!deslocamentoRealizado.HasValue)
			{
				throw new ValidationException(MensagensValidacaoServicos.RnDeslocamentoRealizado_NaoInformado);
			}
		}





		#endregion  "Regras de Negócio"

		public bool IndicaContratoLancamentoValorRisco(IContratoInstanciaValorParametro contratLancamentoValor, out int? IdContratoLancamentoValor)
		{
			var contrato = BuscarPorProduto(contratLancamentoValor.IdProduto);
			var lstObrigatorios = ParametrosObrigatorios(contrato);
			var parametroValor = contratLancamentoValor.VlrRiscoSegurado;
			IdContratoLancamentoValor = null;

			if (lstObrigatorios.Contains(TipoContratoParametroEnum.ValorRisco))
			{
				if (contrato.ContratoLancamento.Count(f => f.TipoParametroQuantitativoVariavel == TipoContratoParametroEnum.ValorRisco) > 1)
				{
					throw new ValidationException("Esse produto está cadastrado com mais de um lançamento para esse 'Valor do Risco'. Não é possível gerar lançamento, o cadastro desse produto deve ser revisado.");
				}

				if (!parametroValor.HasValue)
				{
					throw new ValidationException("Esse produto é por valor do risco. Campo 'Valor do Risco' é obrigatório.");
				}


				var lancamento = contrato.ContratoLancamento.First(f => f.TipoParametroQuantitativoVariavel == TipoContratoParametroEnum.ValorRisco);

				foreach (var contratoValores in lancamento.ContratoLancamentoValor)
				{
					switch (contratoValores.TipoQuantitativoVariacao)
					{
						case TipoQuantitativoVariacaoEnum.DeAte:
							if (parametroValor >= contratoValores.QuantitativoA && parametroValor <= contratoValores.QuantitativoB)
							{
								IdContratoLancamentoValor = contratoValores.Id;
							}

							break;
						case TipoQuantitativoVariacaoEnum.AcimaDe:
							if (parametroValor > contratoValores.QuantitativoA)
							{
								IdContratoLancamentoValor = contratoValores.Id;
							}
							break;

						case TipoQuantitativoVariacaoEnum.UnicoNaoVariavel:
						default:
							throw new NotImplementedException();
					}
				}

			}

			return IdContratoLancamentoValor.HasValue;
		}

	}
}
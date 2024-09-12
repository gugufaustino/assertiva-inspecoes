using Differencial.Domain.Contracts.Services;
using Differencial.Web.Filters;
using System.Collections.Generic;
using System.Linq;
using Differencial.Domain.Entities;
using Differencial.Web.DTO;
using Differencial.Domain.Filters;
using Differencial.Domain.Contracts.Util;
using Differencial.Domain.DTO;
using Differencial.Web.Helpers;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Resources;
using Differencial.Domain;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Differencial.Domain.UOW;

namespace Differencial.Web.Controllers
{
    public class OperadorController : BaseController
    {
        private readonly IOperadorService _service;
        private readonly IConfiguracaoAplicativo _configuracaoAplicativo;
        private readonly IVistoriadorProdutoService _vistoriadorProdutoService;
        private readonly IVistoriadorService _vistoriadorService;
        private readonly IProdutoService _produtoService;
        private readonly ISeguradoraService _seguradoraService;
        private readonly ILogAuditoriaService _serviceLogAuditoria;
        private INotificacaoService _notificacaoService;

        public OperadorController(IOperadorService operadorService, IConfiguracaoAplicativo configuracaoAplicativo,
            IVistoriadorProdutoService vistoriadorProdutoService,
            IVistoriadorService vistoriadorService,
            IProdutoService produtoService,
            ISeguradoraService seguradoraService,
            ILogAuditoriaService serviceLogAuditoria,
            INotificacaoService notificacaoService)
        {
            _service = operadorService;
            _configuracaoAplicativo = configuracaoAplicativo;
            _vistoriadorProdutoService = vistoriadorProdutoService;
            _vistoriadorService = vistoriadorService;
            _produtoService = produtoService;
            _serviceLogAuditoria = serviceLogAuditoria;
            _seguradoraService = seguradoraService;
            _notificacaoService = notificacaoService;

            ViewBag.ConfiguracaoAplicativo = _configuracaoAplicativo;
        }

        public ActionResult Listar()
        {
            var lstOperadores = _service.ListarOperadorCadastro(new OperadorFilter() { CampoOrdenacao = CampoOrdenacaoOperador.NomeOperador });

            ViewBag.ConfiguracaoAplicativo = _configuracaoAplicativo;

            return View(lstOperadores);
        }

        [HttpPost]
        public JsonResult ListarVistoriadorContratoLancamentoValor(int IdVistoriador)
        {
            var result = MontarListaDiponivelParaVistoriador(_produtoService.ListarDiponivelParaVistoriador(IdVistoriador));
            return ResponseResult(true, content: result);
        }

        [HttpGet]        
        public JsonResult ListarVistoriadorContratoLancamentoValorCache(int IdVistoriador)
        {
            var result = MontarListaDiponivelParaVistoriador(_produtoService.ListarDiponivelParaVistoriador(IdVistoriador));
            return ResponseResult(true, content: result);
        }

        [HttpGet]
        public async Task<JsonResult> ListarVistoriadorContratoLancamentoValorAsync(int IdVistoriador)
        {
            var task = Task.Run(() => MontarListaDiponivelParaVistoriador(_produtoService.ListarDiponivelParaVistoriador(IdVistoriador)));

            var result = await task;
            return ResponseResult(true, content: result);
        }

        [HttpGet]
       // [ResponseCache(Duration = 30, VaryByParam = "none")]
        public async Task<JsonResult> ListarVistoriadorContratoLancamentoValorAsyncCache(int IdVistoriador)
        {
            var task = Task.Run(() => MontarListaDiponivelParaVistoriador(_produtoService.ListarDiponivelParaVistoriador(IdVistoriador)));

            var result = await task;
            return ResponseResult(true, content: result);
        }

        public ActionResult Inserir()
        {
            return View();
        }

        public async Task<ActionResult> Editar(int? Id)
        {
            Operador entidade = null;
            ViewBag.ConfiguracaoAplicativo = _configuracaoAplicativo;

            if (Id.HasValue)
            {
                entidade = await _service.BuscarParaEditar(Id.Value);
                List<SelectListItem> lstSeguradora = _seguradoraService.Listar(new SeguradoraFilter()).ToList().ToSelectList(i => i.Id, i => i.NomeSeguradora, entidade.Solicitante != null ? (object)entidade.Solicitante.IdSeguradora : null).ToList();
                ViewBag.lstSeguradora = lstSeguradora;

                CarregaLogsAuditoria(entidade);
            }
            else
            {
                List<SelectListItem> lstSeguradora = _seguradoraService.Listar(new SeguradoraFilter()).ToList().ToSelectList(i => i.Id, i => i.NomeSeguradora, null).ToList();
                ViewBag.lstSeguradora = lstSeguradora;
            }
            return View(entidade);
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult Editar(RetornoSalvarEnum retornosalvar, Operador entidade, IFormFile inputFoto)
        {
            List<SelectListItem> lstSeguradora = _seguradoraService.Listar(new SeguradoraFilter()).ToList().ToSelectList(i => i.Id, i => i.NomeSeguradora, entidade.Solicitante != null ? (object)entidade.Solicitante.IdSeguradora : null).ToList();
            ViewBag.lstSeguradora = lstSeguradora;

            _service.Salvar(entidade, inputFoto);
            Commit();
            return base.RetornoSalvar(retornosalvar, entidade.Id);

            //return View(entidade);
        }



        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult Excluir(int[] Id)
        {

            _service.Excluir(Id);
            Commit();

            return RedirectToAction("Listar", new { msg = "Excluído com sucesso!" });
        }


        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public JsonResult Ativar(int IdVistoriador, int[] Ids)
        {

            _vistoriadorProdutoService.Ativar(Ids);
            Commit();
            var result = MontarListaDiponivelParaVistoriador(_produtoService.ListarDiponivelParaVistoriador(IdVistoriador));
            return ResponseResult(true, content: result);


        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public JsonResult Desativar(int IdVistoriador, int[] Ids)
        {
            _vistoriadorProdutoService.Desativar(Ids);
            Commit();
            var result = MontarListaDiponivelParaVistoriador(_produtoService.ListarDiponivelParaVistoriador(IdVistoriador));
            return ResponseResult(true, content: result);

        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public JsonResult SalvarProdutoVistoriador(int IdVistoriador, KeyVistoriadorProdutoLancamentoDTO[] arrVistoriadorProduto, decimal VlrQuilometroRodado, decimal VlrPagamentoVistoria)
        {

            _vistoriadorProdutoService.SalvarValoresVistoriadorProduto(IdVistoriador, arrVistoriadorProduto, VlrQuilometroRodado, VlrPagamentoVistoria);
            Commit();
            var result = MontarListaDiponivelParaVistoriador(_produtoService.ListarDiponivelParaVistoriador(IdVistoriador));

            return ResponseResult(true, content: result);

        }

        private IEnumerable<object> MontarListaDiponivelParaVistoriador(IEnumerable<VistoriadorProdutoValorDTO> listaProdutoDTO)
        {
            var lst = listaProdutoDTO.Select(i =>
                    new
                    {
                        IdVistoriadorProduto = i.IdVistoriadorProduto,
                        IdProduto = i.IdProduto,
                        IdContratoLancamento = i.IdContratoLancamento,
                        IdContratoLancamentoValor = i.IdContratoLancamentoValor,
                        KeyVistoriadorProdutoLancamentoValor = i.KeyVistoriadorProdutoLancamentoValor,
                        NomeSeguradora = i.NomeSeguradora,
                        NomeProduto = i.NomeProduto,
                        FaixaParametro = MontarParametro(i),
                        VlrQuilometroRodado = i.VistoriadorProduto != null ? i.VistoriadorProduto.VlrQuilometroRodado.FormatoMoeda() : null,
                        VlrPagamentoVistoria = i.VistoriadorProduto != null ? i.VistoriadorProduto.VlrPagamentoVistoria.FormatoMoeda() : null,
                        DataModificacao = i.VistoriadorProduto == null ? null : i.VistoriadorProduto.DataModificacao.ToShortDateString() + " " + i.VistoriadorProduto.DataModificacao.ToLongTimeString(),
                        IndAtivo = i.VistoriadorProduto != null ? HtmlGridHelper.Indicador(i.VistoriadorProduto.IndAtivo, true).ToString() : string.Empty,

                    }).ToList();

            return lst;
        }

        private string MontarParametro(VistoriadorProdutoValorDTO i)
        {
            string strRetorno = string.Empty;
            string strParam = string.Empty;

            strRetorno += i.TipoParametroQuantitativoVariavel.FwDisplayEnum(true).ToString();
            switch (i.TipoParametroQuantitativoVariavel)
            {
                case TipoContratoParametroEnum.Comum:
                    strParam = "<br><small> - </small>";
                    break;

                case TipoContratoParametroEnum.ValorRisco:
                    switch (i.ContratoLancamentoValor.TipoQuantitativoVariacao)
                    {
                        case TipoQuantitativoVariacaoEnum.DeAte:
                            strParam = "<br><small>De {0} Até {1}</small>".Formata(i.ContratoLancamentoValor.QuantitativoA.Value.FormataMoeda(), i.ContratoLancamentoValor.QuantitativoB.Value.FormataMoeda());
                            //   strParam = "<br><small>De {0} Até {1}</small>".Formata(i.ContratoLancamentoValor.QuantitativoA.Value.ToString("#"), i.ContratoLancamentoValor.QuantitativoB.Value.ToString("#"));
                            break;

                        case TipoQuantitativoVariacaoEnum.AcimaDe:
                            strParam = "<br><small>Acima de {0}</small>".Formata(i.ContratoLancamentoValor.QuantitativoA.Value.FormataMoeda());
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;
                default:
                    throw new NotImplementedException();

            }
            strRetorno += strParam;
            return strRetorno;
        }


        public ActionResult FormTemplate()
        {
            var model = _service.Listar(new OperadorFilter()).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult FormTemplate(RetornoSalvarEnum retornosalvar, Operador model)
        {
            if (ModelState.IsValid)
            {
                return base.RetornoSalvar(retornosalvar, model.Id);
            }
            return View(model);
        }

        #region Metodos Auxiliares
        private void CarregaLogsAuditoria(Operador entidade)
        {
            ViewBag.lstLog = new List<LogAuditoria>();
            var lstEntidadeLogs = new List<IEntity>();
            lstEntidadeLogs.Add(entidade);

            if (entidade.Endereco != null)
                lstEntidadeLogs.Add(entidade.Endereco);

            if (entidade.Vistoriador != null)
                lstEntidadeLogs.Add(entidade.Vistoriador);

            if (entidade.Vistoriador != null && entidade.Vistoriador.EnderecoBase != null)
                lstEntidadeLogs.Add(entidade.Vistoriador.EnderecoBase);

            if (entidade.Vistoriador != null && entidade.Vistoriador.VistoriadorProduto.Any())
                lstEntidadeLogs.AddRange(entidade.Vistoriador.VistoriadorProduto.ToList());

            if (entidade.Analista != null)
                lstEntidadeLogs.Add(entidade.Analista);

            if (entidade.Solicitante != null)
                lstEntidadeLogs.Add(entidade.Solicitante);

            //if (entidade.Agendamento.Any())
            //    lstEntidadeLogs.AddRange(entidade.Agendamento.ToList());

            //if (entidade.LancamentoFinanceiro.Any())
            //    lstEntidadeLogs.AddRange(entidade.LancamentoFinanceiro.ToList());


            ViewBag.lstLog = _serviceLogAuditoria.Listar(lstEntidadeLogs);
        }

        #endregion


        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public JsonResult GerarAcesso(int Id)
        {
            _service.SalvarGerarAcesso(Id);
            Commit();
            return ResponseResult(true, message: MensagensSucesso.OperadorGerarAcesso);

        }


        [HttpGet]
        public JsonResult ListarMinhasNotificacoes()
        {
            var result = _notificacaoService.MinhasNotificacoes().Select(i =>
                new NotificacaoDTO
                {
                    mensagem = i.Notificacao.Descricao,
                    indLido = i.IndLido,
                    data = i.Notificacao.DataCadastro,
                    cod = i.Notificacao.IdSolicitacao

                });

            return ResponseResult(true, content: result);
        }
        [HttpPost]
        public JsonResult ExcluirTodasMinhasNotificacoes()
        {
            _notificacaoService.ExcluirTodasMinhas();
            Commit();
            return ResponseResult(true);
        }
        

    }
}
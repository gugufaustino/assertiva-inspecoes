using Differencial.Domain.Contracts.Services;
using Differencial.Web.Filters;
using Differencial.Domain.Entities;
using Differencial.Web.DTO;
using Differencial.Domain.Filters;
using System.Collections.Generic;
using System.Linq;
using Differencial.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.UOW;

namespace Differencial.Web.Controllers
{
    public class ProdutoController : BaseController
    {
        private readonly IProdutoRepository produtoRepository;
        private readonly IProdutoService _service;
        private readonly ISeguradoraService _seguradoraService;
        private readonly ITipoInspecaoService _tipoInspecaoService;

        private readonly ILogAuditoriaService _serviceLogAuditoria;

        public ProdutoController(IProdutoRepository produtoRepository,
                                    IProdutoService produtoService,
                                    ISeguradoraService seguradoraService,
                                    ITipoInspecaoService tipoInspecaoService,
                                    ILogAuditoriaService serviceLogAuditoria)
        {
            this.produtoRepository = produtoRepository;
            this._service = produtoService;
            this._seguradoraService = seguradoraService;
            this._tipoInspecaoService = tipoInspecaoService;
            this._serviceLogAuditoria = serviceLogAuditoria;
        }

        public ActionResult Listar()
        {
            var lstProduto = produtoRepository.Listar(new ProdutoFilter() { CampoOrdenacao = CampoOrdenacaoProduto.IdSeguradora });
            return View(lstProduto);
        }
        public ActionResult Editar(int? Id)
        {
            Produto model = null;
            if (Id.HasValue)
            {
                model = _service.Buscar(Id.Value);
                ViewBag.lstLog = _serviceLogAuditoria.Listar(model.Id, model);
            }
            CarregaDropDown(model);
            return View(model);
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        [Validacao(IgnorarId = true)]
        public ActionResult Editar(RetornoSalvarEnum retornosalvar, Produto model, List<ContratoLancamento> contratoLancamento)
        {

            foreach (var contratoLanc in contratoLancamento.Where(w => w.TipoParametroQuantitativoVariavel == TipoContratoParametroEnum.Comum || w.TipoParametroQuantitativoVariavel == TipoContratoParametroEnum.RelatorioMelhoria))
            {
                contratoLanc.ContratoLancamentoValor = contratoLanc.ContratoLancamentoValor
                                                        .Where(lv =>
                                                                lv.TipoQuantitativoVariacao == TipoQuantitativoVariacaoEnum.UnicoNaoVariavel
                                                             && lv.ValorLancamento > 0).ToList();
            }

            model.IdSeguradora = model.Seguradora.Id;
            model.IdTipoInspecao = model.TipoInspecao.Id;
            model.Seguradora = null;
            model.TipoInspecao = null;
            model.Contrato = new Contrato { Id = model.Contrato.Id, ContratoLancamento = contratoLancamento };


            _service.Salvar(model);
            AppSaveChanges();

            return base.RetornoSalvar(retornosalvar, model.Id);


        }
        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult Excluir(int[] Id)
        {
            foreach (var item in Id)
            {
                _service.Excluir(item);
            }

            AppSaveChanges();

            return RedirectToAction("Listar", new { msg = Differencial.Domain.Resources.MensagensSucesso.SucessoExcluir });
        }

        private void CarregaDropDown(Produto model)
        {
            ViewBag.IdTipoInspecao = new SelectList(_tipoInspecaoService.Listar(new TipoInspecaoFilter()), "Id", "NomeTipoInspecao");
            ViewBag.IdSeguradora = new SelectList(_seguradoraService.Listar(new SeguradoraFilter()), "Id", "NomeSeguradora");
        }

    }
}
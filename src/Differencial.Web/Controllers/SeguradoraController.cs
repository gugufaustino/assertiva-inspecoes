using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Web.DTO;
using Differencial.Web.Filters;
using Differencial.Domain.Util.ExtensionMethods;
using System.Linq;
using Differencial.Domain.Resources;
using Microsoft.AspNetCore.Mvc;
using Differencial.Domain.UOW;

namespace Differencial.Web.Controllers
{
    public class SeguradoraController : BaseController
    {
        private readonly ISeguradoraService _service;
        private readonly ILogAuditoriaService _serviceLogAuditoria;
        private readonly ISolicitanteService _serviceSolicitante;
        private readonly IFilialService _serviceFilial;
        public SeguradoraController(ISeguradoraService seguradoraService,
                ISolicitanteService solicitanteService,
            ILogAuditoriaService serviceLogAuditoria,
            IFilialService serviceFilial)
        {
            _service = seguradoraService;
            _serviceLogAuditoria = serviceLogAuditoria;
            _serviceSolicitante = solicitanteService;
            _serviceFilial = serviceFilial;
        }


        public ActionResult Listar()
        {
            var lst = _service.Listar(new SeguradoraFilter());
            return View(lst);
        }

        [HttpGet]
        public ActionResult Editar(int? Id)
        {
            Seguradora model = null;
            if (Id.HasValue)
            {
                model = _service.Buscar(Id.Value);
                ViewBag.lstLog = _serviceLogAuditoria.Listar(model.Id, model);
            }
            return View(model);
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult Editar(RetornoSalvarEnum retornosalvar, Seguradora model)
        {
            if (ModelState.IsValid)
            {

                _service.Salvar(model);

                AppSaveChanges();
                return RetornoSalvar(retornosalvar, model.Id);

            }
            ViewBag.lstLog = _serviceLogAuditoria.Listar(model.Id, model);
            return View(model);
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult Excluir(int[] Id)
        {
            _service.Excluir(Id);
            AppSaveChanges();
            return base.RetornoSalvar(RetornoSalvarEnum.Listar, 0, Differencial.Domain.Resources.MensagensSucesso.SucessoExcluir);

        }

        public JsonResult ObterEmail(int Id)
        {
            if (Id > 0)
            {
                return ResponseResult(true, content: _service.Buscar(Id).EmailRemetenteSolicitacao);
            }

            return ResponseResult(false);
        }
        #region Solicitante
        public JsonResult GridSeguradoraSolicitante(int Id)
        {
            var lst = _serviceSolicitante.Listar(
                new SolicitanteFilter { IdSeguradora = Id }).Select(i => new
                {
                    DT_RowId = i.Id,
                    NomeOperador = i.Operador.NomeOperador,
                    TipoSolicitante = i.TipoSolicitante.Display().Name,
                    i.Operador.Telefone,
                    i.Operador.Email,
                    DataCadastro = i.DataCadastro.FormatoDataHora(),
                    DataModificacao = i.DataModificacao.FormatoDataHora()
                }).ToList();

            return Json(new { data = lst });
        }
        #endregion


        #region Filial
        public JsonResult GridSeguradoraFilial(int Id)
        {
            var lst = _serviceFilial.Listar(
                new FilialFilter { IdSeguradora = Id }).Select(i => new
                {
                    DT_RowId = i.Id,
                    NomeFlial = i.NomeFilial,
                    DataCadastro = i.DataCadastro.FormatoDataHora(),
                    DataModificacao = i.DataModificacao.FormatoDataHora()
                }).ToList();

            return Json(new { data = lst });
        }
        public ActionResult InserirFilial(int idSeguradora)
        {
            ViewBag.IdSeguradora = idSeguradora;
            return View("/Views/Filial/ModalEditar.cshtml");
        }
        public ActionResult EditarFilial(int id)
        {
            var entidade = _serviceFilial.Buscar(id);
            ViewBag.IdSeguradora = entidade.IdSeguradora;
            return View("/Views/Filial/ModalEditar.cshtml", entidade);
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult EditarFilial(int idSeguradora, Filial entidade)
        {
            _serviceFilial.Salvar(entidade);
            AppSaveChanges();
            return ResponseResult(true, content: entidade.Id, message: MensagensSucesso.SucessoSalvar);
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult ExcluirFilial(int[] Id)
        {
            _serviceFilial.Excluir(Id);
            AppSaveChanges();
            return ResponseResult(true, message: MensagensSucesso.SucessoExcluir);
        }

        #endregion

    }
}
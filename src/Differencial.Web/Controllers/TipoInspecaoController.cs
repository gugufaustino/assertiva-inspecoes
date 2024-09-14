using Differencial.Domain.Contracts.Services;
using Differencial.Web.Filters;
using Differencial.Domain.Entities;
using Differencial.Web.DTO;
using Differencial.Domain.Filters;
using Differencial.Domain.Resources;
using Differencial.Domain.Contracts.Infra;
using Microsoft.AspNetCore.Mvc;
using Differencial.Domain.UOW;

namespace Differencial.Web.Controllers
{
    public class TipoInspecaoController : BaseController
    {

        private readonly ITipoInspecaoService _service;
        private readonly ILogAuditoriaService _serviceLogAuditoria;
        
        public TipoInspecaoController(ITipoInspecaoService tipoinspecaoService,
           ILogAuditoriaService serviceLogAuditoria )
        {
            _service = tipoinspecaoService;
            _serviceLogAuditoria = serviceLogAuditoria; 
        }


        public ActionResult Listar()
        {
            var lstTipoInspecao = _service.Listar(new TipoInspecaoFilter());
            return View(lstTipoInspecao);
        }
        public ActionResult Editar(int? Id)
        {
            TipoInspecao model = null;
            if (Id.HasValue)
            {
                model = _service.Buscar(Id.Value);
                ViewBag.lstLog = _serviceLogAuditoria.Listar(model.Id, model);
            }
            return View(model);
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult Editar(RetornoSalvarEnum retornosalvar, TipoInspecao entidade)
        {
            if (ModelState.IsValid)
            {
                _service.Salvar(entidade);
                AppSaveChanges();

                return base.RetornoSalvar(retornosalvar, entidade.Id);
            }

            ViewBag.lstLog = _serviceLogAuditoria.Listar(entidade.Id, entidade);
            return View(entidade);
        }


        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult Excluir(int[] Id)
        {

            _service.Excluir(Id);

            AppSaveChanges();

            return RetornoSalvar(RetornoSalvarEnum.Listar, mensagemSucesso: MensagensSucesso.SucessoExcluir);
        }

    }
}
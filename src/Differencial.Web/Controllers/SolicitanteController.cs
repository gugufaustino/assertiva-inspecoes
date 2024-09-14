using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Contracts.Util;
using Differencial.Domain.Entities;
using Differencial.Domain.Resources;
using Differencial.Domain.UOW;
using Differencial.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Differencial.Web.Controllers
{
    public class SolicitanteController : BaseController
    {
        private readonly IOperadorService _service;
        private readonly IConfiguracaoAplicativo _configuracaoAplicativo;
        private readonly ISeguradoraService _seguradoraService;
        private readonly ILogAuditoriaService _serviceLogAuditoria;

        public SolicitanteController(IOperadorService operadorService,
            IConfiguracaoAplicativo configuracaoAplicativo,
            ISeguradoraService seguradoraService,
            ILogAuditoriaService serviceLogAuditoria)
        {
            _service = operadorService;
            _configuracaoAplicativo = configuracaoAplicativo;
            _serviceLogAuditoria = serviceLogAuditoria;
            _seguradoraService = seguradoraService;

            ViewBag.ConfiguracaoAplicativo = _configuracaoAplicativo;
        }


        public ActionResult Inserir(int idSeguradora)
        {
            ViewBag.IdSeguradora = idSeguradora;
            return View("Editar");
        }

        public ActionResult Editar(int id)
        {
            Operador entidade = _service.Buscar(id);
            ViewBag.IdSeguradora = entidade.Solicitante.IdSeguradora;
            return View(entidade);
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public JsonResult Editar(Operador entidade, int idSeguradora)
        {
            _service.SalvarSolicitanteSemAcesso(entidade, idSeguradora);
            AppSaveChanges();
            return ResponseResult(true, content: entidade.Id, message: MensagensSucesso.SucessoSalvar);
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public JsonResult Excluir(int[] Id)
        {
            _service.ExcluirSolicitante(Id);
            AppSaveChanges();
            return ResponseResult(true, message: MensagensSucesso.SucessoExcluir);
        }

    }
}
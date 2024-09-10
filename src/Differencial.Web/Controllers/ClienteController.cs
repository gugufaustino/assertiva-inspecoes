using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Web.Controllers;
using Differencial.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace WEB.Controllers
{
    public class ClienteController : BaseController
    {
        private readonly IClienteService _service;
        private readonly ILogAuditoriaService _serviceLogAuditoria;

        public ClienteController(IClienteService clienteService,
           ILogAuditoriaService serviceLogAuditoria)
        {
            _service = clienteService;
            _serviceLogAuditoria = serviceLogAuditoria;
        }

        public ActionResult Listar()
        {
            var lstEntidade = _service.Listar(new ClienteFilter()).Where(w=> w.Solicitacao.Any());
            return View(lstEntidade);
        }
        public ActionResult Editar(int? Id)
        {
            Cliente model = null;
            if (Id.HasValue)
            {
                model = _service.Listar(new ClienteFilter { Id = Id.Value }).First();
                ViewBag.lstLog = _serviceLogAuditoria.Listar(model.Id, model);
            }
            return View(model);
        }
    }
}
﻿using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Exceptions;
using Differencial.Domain.Filters;
using Differencial.Domain.Resources;
using Differencial.Domain.UOW;
using Differencial.Web.Controllers;
using Differencial.Web.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace WEB.Controllers
{
    public class TipoAssuntoController : BaseController
    {
        private readonly ITipoAssuntoService _service;
        private readonly ILogAuditoriaService _serviceLogAuditoria;
        private readonly ITipoAssuntoRepository _tipoAssuntoRepositorio;

        public TipoAssuntoController(ITipoAssuntoService service,
                                    ILogAuditoriaService serviceLogAuditoria,
                                    ITipoAssuntoRepository tipoAssuntoRepositorio)
        {
            this._service = service;
            this._serviceLogAuditoria = serviceLogAuditoria;
            this._tipoAssuntoRepositorio = tipoAssuntoRepositorio;
        }

        public JsonResult ObterTipoAssunto(int Id)
        {
            if (Id > 0)
            {
                return ResponseResult(true, content: _service.Buscar(Id).TextoPadrao);
            }
            return ResponseResult(false);
        }

        public ActionResult Listar()
        {
            var lst = Lista();

            return View(lst);
        }

        private IEnumerable<TipoAssunto> Lista()
        {
            return _tipoAssuntoRepositorio.Where(new TipoAssuntoFilter { CampoOrdenacao = CampoOrdenacaoTipoAssunto.Id });
        }

        [HttpGet]
        public ActionResult Editar(int? Id)
        {
            TipoAssunto model = null;
            if (Id.HasValue)
            {
                model = _service.Buscar(Id.Value);
                ViewBag.lstLog = _serviceLogAuditoria.Listar(model.Id, model);
            }
            return View(model);
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult Editar(RetornoSalvarEnum retornosalvar, TipoAssunto model)
        {
            ViewBag.lstLog = _serviceLogAuditoria.Listar(model.Id, model);

            if (!ModelState.IsValid) return View(model);

            if (model.NomeAssunto == "999")
                throw new ValidationException("validation");

            _service.Salvar(model);
            AppSaveChanges();
            return RetornoSalvar(retornosalvar, model.Id);

        }
        
        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult Excluir(int[] Id)
        { 
            _service.Excluir(Id);
            AppSaveChanges();
            var result = MontarLista(Lista());
            return ResponseResult(true, content: result, message: MensagensSucesso.SucessoExcluir);
        }

        private IEnumerable<object> MontarLista(IEnumerable<TipoAssunto> lstEntidade)
        {
            var lst = lstEntidade.Select(item =>
                new
                {
                    item.Id,
                    item.NomeAssunto,
                    DataCadastro = item.DataCadastro.ToString(),
                    DataModificacao = item.DataModificacao.ToString()
                });

            return lst;
        }

    }
}
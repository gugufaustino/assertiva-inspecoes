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
using Microsoft.AspNetCore.Mvc;

namespace Differencial.Web.Controllers
{
    public class VistoriadorController : BaseController
    {

        private readonly IVistoriadorService _service;


        public VistoriadorController(IVistoriadorService vistoriadorService)
        {
            _service = vistoriadorService;

        }

        public JsonResult ObterEmail(int Id)
        {
            if (Id > 0)
            {
                return ResponseResult(true, content: _service.Buscar(Id).Operador.Email);
            }

            return ResponseResult(false);
        }

    }
}
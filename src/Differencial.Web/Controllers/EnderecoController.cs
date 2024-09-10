using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Differencial.Web.Controllers
{
    public class EnderecoController : BaseController
    {

        IEnderecoService _service;
        public EnderecoController(IEnderecoService service)
        {
            _service = service;

        }
        public ActionResult Index()
        {
            Endereco endereco = _service.BuscarPorCEP("88056820");

            return View();
        }

        [HttpPost]
        public JsonResult PesquisaCEP(string cep)
        {
            try
            {
                Endereco endereco = _service.BuscarPorCEP(cep);
                return Json(new { Erro = false, Result = endereco });
            }
            catch (Exception ex)
            {

                return Json(new { Erro = true, Result = string.Format("{0};<br/>\r\n{1}", ex.Message, ex.StackTrace) });
            }
        }

        [HttpPost]
        public ActionResult PesquisaGeoLatLong(Endereco endereco, Operador operador)
        {

            if (operador != null && operador.Vistoriador != null)
                endereco = operador.Vistoriador.EnderecoBase;

            var geoCordenadas = _service.BuscarGeoCordenadas(endereco);

            var result = new
            {
                Latitude = geoCordenadas?.Latitude,
                Longitude = geoCordenadas?.Longitude
            };

            return ResponseResultObject(true, content: result); 
        } 
    }
}
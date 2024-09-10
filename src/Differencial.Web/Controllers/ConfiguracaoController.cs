using Differencial.Domain.Contracts.Infra;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Differencial.Web.Controllers
{
    public class ConfiguracaoController : BaseController
    {
        IUsuarioService _usuario;
        public ConfiguracaoController(
           IUsuarioService usuario)
        {
            _usuario = usuario;
        }

        [HttpPost]
        public ActionResult Aplicar(string chave, string valor)
        { 
            _usuario.UsuarioAutenticado.ConfigMenuColapso = valor;
            return ResponseResult(true);
        }

    }
}
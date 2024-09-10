using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Contracts.Infra;
using Microsoft.AspNetCore.Mvc;
using Differencial.Domain.Contracts.Util;

namespace Differencial.Web.API
{
    [Route("DefaultApi")]
    public class TarefasController : ControllerBase
    {
        private readonly IOperadorService _operadorService;
        private readonly ISolicitacaoService _solicitacaoService;
        private readonly IConfiguracaoAplicativo _configuracaoAplicativo;

        public TarefasController(ISolicitacaoService solicitacaoService,
                                IOperadorService operadorService,                                
                                IConfiguracaoAplicativo configuracaoAplicativo)
        {
            _solicitacaoService = solicitacaoService;
            _operadorService = operadorService;
            _configuracaoAplicativo = configuracaoAplicativo;
        }
                
        [HttpPost]
        [Route("")]
        public ActionResult Post(string acao)
        {
            var op = _operadorService.BuscarLogon(_configuracaoAplicativo.UsuarioRoot, _configuracaoAplicativo.UsuarioRootPwd);
            if (op == null)
            {
                return Unauthorized();
            }

            switch (acao.ToLower())
            {
                case "cobrarvistoria":
                    _solicitacaoService.CobrarVistoria(op.Id);
                    break;
                default:                    
                    break;
            }


            return Ok(new { op.NomeOperador, op.DataCadastro });
        }


    }
}
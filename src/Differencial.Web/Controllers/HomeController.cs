﻿using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Resources;
using Differencial.Domain.UOW;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Service.ServiceUtility;
using Differencial.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Differencial.Web.Controllers
{

    public class HomeController : BaseController
    {
        private readonly IOperadorService _operadorService;
        private readonly IUsuarioService _usuarioService;

        private readonly IArquivoAnexoService _arquivoService;
        private readonly IEnderecoService _enderecoService;



        public HomeController(
            IOperadorService operadorService,
            IUsuarioService usuario,
            IArquivoAnexoService arquivoService
            , IEnderecoService enderecoService,
            ISolicitacaoService solicitacaoServ)
        {
            _operadorService = operadorService;
            _usuarioService = usuario;
            _arquivoService = arquivoService;
            _enderecoService = enderecoService;


            //solicitacaoServ.BuscarSolicitacaoEndereco(1);

        }

        [AllowAnonymous]
        public ActionResult Index()        
        {
            if (_usuarioService.Autenticado())
                return Redirect(@"~/Home/Inicio");

            return View("Login");
        }
        [AllowAnonymous]
        public JsonResult Teste()
        {
            var oo = new
            {
                Idade = 34,
                Nome = "gugu",
                Pilinha = 11000d
            };

            return base.Json(oo);
        }

        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl = null)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        public ActionResult Inicio()
        {
            return View();
        }

        public ActionResult TelaBloqueio()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Error()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Error404()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Error401()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(string usuario, string senha, string ReturnUrl)
        {
            var op = _operadorService.BuscarLogon(usuario, senha);
            if (op != null)
            {

                await _usuarioService.Autenticar(op);


                if (!string.IsNullOrEmpty(ReturnUrl))
                    return Redirect(ReturnUrl);
                else
                    return Redirect(Url.Action("Inicio", "Home"));

            }
            else
            {
                return Redirect(Url.Action("Login", "Home"));
            }
        }


        public async Task<ActionResult> Logout()
        {

            await _usuarioService.Remover();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult EsqueceuSenha()
        {
            return View();
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult EsqueceuSenha(string usuario)
        {
            _operadorService.GerarNovoAcesso(usuario);
            var op = _operadorService.BuscarPorUsuario(usuario);
            Commit(op.Id);
            return ResponseResult(true, message: MensagensSucesso.EsqueceuSenhaSucesso.Formata(usuario.ToLower()));
        }

        [HttpGet]
        public ActionResult AtivarAcesso(int id, string token)
        {
            ViewBag.Id = id;
            ViewBag.Token = token;
            ViewBag.NomeOperador = _operadorService.Buscar(id).NomeOperador;
            return View();
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public Task<ActionResult> AtivarAcesso(int id, string token, string novasenha)
        {
            _operadorService.SalvarMudarSenha(id, token, novasenha);
            Commit(id);
            return Login(_operadorService.Buscar(id).Email, novasenha, string.Empty);
        }

        //public EmptyResult GerarThumbnail(int id)
        //{

        //    _arquivoService.GerarTodosThumbnail(id);

        //    return new EmptyResult();
        //}

        [HttpGet]
        public JsonResult Notificar(string mensagem)
        {

            Service.NotificationBroadcast.Instance.NovaNotificacaoTodos(new Domain.DTO.NotificacaoDTO
            {
                indLido = false,
                mensagem = mensagem,
                data = System.DateTime.Now

            });

            return Json("ok");
        }

        [HttpGet]
        public JsonResult NotificarGrupo(string mensagem, string grupo)
        {
            Service.NotificationBroadcast.Instance.NovaNotificacaoGroup(new Domain.DTO.NotificacaoDTO
            {
                indLido = false,
                mensagem = mensagem,
                data = System.DateTime.Now

            }, grupo);
            return Json("ok");
        }
    }

    //public class Principal : IPrincipal
    //{
    //    public IIdentity Identity => throw new System.NotImplementedException();

    //    public bool IsInRole(string role)
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}

    //public class Identidade : IIdentity
    //{
    //    public string AuthenticationType => throw new System.NotImplementedException();

    //    public bool IsAuthenticated => throw new System.NotImplementedException();

    //    public string Name => throw new System.NotImplementedException();
    //}

}
using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Resources;
using Differencial.Domain.UOW;
using Differencial.Domain.Util.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace Differencial.Web.Controllers
{

    public class HomeController : BaseController
    {
        private readonly IOperadorService _operadorService;
        private readonly IUsuarioService _usuarioService;

        private readonly IArquivoAnexoService _arquivoService;
        private readonly IEnderecoService _enderecoService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IOperadorService operadorService,
            IUsuarioService usuario,
            IArquivoAnexoService arquivoService,
            IEnderecoService enderecoService,
            ISolicitacaoService solicitacaoServ,
            IHttpContextAccessor httpContextAccessor, ILogger<HomeController> logger)
        {
            _operadorService = operadorService;
            _usuarioService = usuario;
            _arquivoService = arquivoService;
            _enderecoService = enderecoService;
            this.httpContextAccessor = httpContextAccessor;
            _logger = logger;

            _logger.LogInformation("Construtor da classe: ola mundo do log");


        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
             

            if (_usuarioService.Autenticado())
                return Redirect(@"~/Home/Inicio");

            ViewBag.Version = GetVersion();
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
            ViewBag.Version = GetVersion();
            return View();
        }
        public static string GetVersion()
        {
            var version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            var exePath = Assembly.GetExecutingAssembly().Location;
			var lastModified = System.IO.File.GetLastWriteTime(exePath); 
			var versionInfo = FileVersionInfo.GetVersionInfo(exePath);

			version = $"Versão: {versionInfo}, Data de Modificação: {lastModified:dd/MM/yyyy HH:mm}";

			if (version != null)
                return version ;
 
            return "";
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
			ViewBag.Version = Environment.GetEnvironmentVariable("Version") + "v";

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
        [AllowAnonymous]
        public ActionResult EsqueceuSenha()
        {
            return View();
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        [AllowAnonymous]
        public ActionResult EsqueceuSenha(string usuario)
        {


            _operadorService.GerarNovoAcesso(usuario);
            var op = _operadorService.BuscarPorUsuario(usuario);
            AppSaveChanges(op.Id);
            return ResponseResult(true, message: MensagensSucesso.EsqueceuSenhaSucesso.Formata(usuario.ToLower()));
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult AtivarAcesso(int id, string token)
        {
            ViewBag.Id = id;
            ViewBag.Token = token;
            ViewBag.NomeOperador = _operadorService.Buscar(id).NomeOperador;
            return View();
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        [AllowAnonymous]
        public Task<ActionResult> AtivarAcesso(int id, string token, string novasenha)
        {
            _operadorService.SalvarMudarSenha(id, token, novasenha);
            AppSaveChanges(id);
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
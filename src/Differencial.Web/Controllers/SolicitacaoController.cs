using Differencial;
using Differencial.Domain;
using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.DTO;
using Differencial.Domain.Entities;
using Differencial.Domain.Enums.WorkFlow;
using Differencial.Domain.Exceptions;
using Differencial.Domain.Filters;
using Differencial.Domain.Resources;
using Differencial.Domain.UOW;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Service.Services;
using Differencial.Web.Controllers;
using Differencial.Web.DTO;
using Differencial.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB.Controllers
{
    public class SolicitacaoController : BaseController
    {
        readonly ISolicitacaoService _solicitacaoService;
        readonly ISolicitacaoRepository _solicitacaoRepository;

        readonly IWorkFlowSolicitacaoService _solicitacaoWorkFlowService;
        readonly IVistoriadorService _vistoriadorService;
        readonly ISeguradoraService _seguradoraService;
        readonly IProdutoService _produtoService;
        readonly IEnderecoService _enderecoService;
        readonly IAgendamentoService _agendamentoService;
        readonly ILogAuditoriaService _logAuditoriaService;
        readonly IOperadorService _operadorService;
        readonly ISolicitanteService _solicitanteService;
        readonly IUsuarioService _usuario;
        readonly IWorkFlowService _workflow;
        readonly IAnalistaService _analistaService;
        readonly IContratoService _contratoService;
        readonly ITipoAssuntoService _tipoAssuntoService;
        readonly IFilialService _filialService;

        readonly IComunicacaoService _comunicacaoService;

        public SolicitacaoController(ISolicitacaoService solicitacaoService,
            ISolicitacaoRepository solicitacaoRepository,
            IWorkFlowSolicitacaoService solicitacaoWorkFlowService,
            IVistoriadorService vistoriadorService,
            ISeguradoraService seguradopraService,
            IEnderecoService enderecoService,
            IProdutoService produtoService,
            ILogAuditoriaService logAuditoriaService,
            IAgendamentoService agendamentoService,
            IOperadorService operadorService,
            ISolicitanteService solicitanteService,
            IUsuarioService usuario,
            IWorkFlowService workflow,
            IAnalistaService analistaService,
            IContratoService contratoService,
            IComunicacaoService comunicacaoService,
             ITipoAssuntoService tipoAssuntoService,
             IFilialService filialService)
        {
            _solicitacaoService = solicitacaoService;
            _solicitacaoRepository = solicitacaoRepository;
            _solicitacaoWorkFlowService = solicitacaoWorkFlowService;
            _vistoriadorService = vistoriadorService;
            _seguradoraService = seguradopraService;
            _enderecoService = enderecoService;
            _produtoService = produtoService;
            _logAuditoriaService = logAuditoriaService;
            _agendamentoService = agendamentoService;
            _operadorService = operadorService;
            _solicitanteService = solicitanteService;
            _usuario = usuario;
            _workflow = workflow;
            _analistaService = analistaService;
            _contratoService = contratoService;

            _comunicacaoService = comunicacaoService;
            _tipoAssuntoService = tipoAssuntoService;
            _filialService = filialService;

        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Inserir()
        {
            Solicitacao model = new Solicitacao();
            var op = _operadorService.Buscar(_usuario.Id);

            ViewBag.InclusaoSolicitante = op.IndSolicitante;
            if (ViewBag.InclusaoSolicitante)
            {
                model.IdSeguradora = op.Solicitante.IdSeguradora.Value;
                ViewBag.SolicitanteTelefone = op.Telefone;
                ViewBag.SolicitanteNome = op.NomeOperador;
                ViewBag.SolicitanteEmail = op.Email;
            }


            CarregaDropDown(model, op.IndSolicitante);

            return View();
        }

        [HttpGet]
        [ServiceFilter(typeof(TransactionFilter))]
        [AllowAnonymous]
        public async Task<ActionResult> EditarToken(int? Id, string acao, string token)
        {
            var returnRedirect = await ApropriarVistoriadorPorTokenAsync(Id, acao, token);
            if (returnRedirect != null)
                return Redirect(returnRedirect);

            return RedirectToAction("Editar", new { Id = Id });
        }

        [HttpGet]      
        public ActionResult Editar(int? Id)
        {
            ViewBag.OrigemForm = "Editar";
            Solicitacao entidade;

            entidade = _solicitacaoService.BuscarUI(Id ?? 0);

            ViewBag.lstLancamentoFinanceiro = entidade.LancamentoFinanceiro.ToList();
            ViewBag.lstContratoParametroObrig = _contratoService.ParametrosObrigatorios(entidade.Produto.Contrato);

            ViewBag.InclusaoSolicitante = entidade.IdOperadorCadastro == entidade.IdSolicitante && entidade.Solicitante.TipoSolicitante == TipoSolicitanteEnum.AcessoAoSistema;
            if (entidade.IdSolicitante.HasValue)
            {
                ViewBag.SolicitanteTelefone = entidade.Solicitante.Operador.Telefone;
                ViewBag.SolicitanteNome = entidade.Solicitante.Operador.NomeOperador;
                ViewBag.SolicitanteEmail = entidade.Solicitante.Operador.Email;
            }

            CarregaLogsAuditoria(entidade);

            ViewBag.UrlMapaRotaAnterior = _solicitacaoService.RotaAnteriorUrlMapa(entidade);
            ViewBag.UrlMapaRotaDeVolta = entidade.IndRotaDeVolta ? _solicitacaoService.RotaDeVoltaUrlMapa(entidade) : string.Empty;

            CarregaDropDown(entidade);

            return View(entidade);

        }

        private async Task<string> ApropriarVistoriadorPorTokenAsync(int? Id, string acao, string token)
        {
            
            if (!token.IsNullOrEmpty())
            {
                Operador op;
                @ViewBag.Token = token;
                @ViewBag.Acao = acao;
                op = _operadorService.BuscarPorToken(token);
                if (op != null)
                    await _usuario.Autenticar(op);

                if (!_usuario.Autenticado())
                  return "~/Login";

                Solicitacao solicitacao = _solicitacaoRepository.GetById((int)Id);
                if (solicitacao.IdVistoriador != op.Id)
                    return @"~/Home/Error404";

                if (acao == "apropriar")
                {
                    if (solicitacao.TpSituacao != TipoSituacaoProcessoEnum.EnviadoParaVistoria)
                        throw new ValidationException("Essa solicitação não pode mais ser apropriada.");

                    _solicitacaoWorkFlowService.Apropriar(Id.Value);
                    Commit();
                }
            }
            return null;
        }

        [HttpPost]
        //[ServiceFilter(typeof(TransactionFilter))]
        [Validacao(IgnorarId = true)]
        [ValidationExceptionFilter]
        public ActionResult Editar(RetornoSalvarEnum retornosalvar, Solicitacao entidade)
        {

            CarregaDropDown(entidade);
            CarregaLogsAuditoria(entidade);

            if (!ModelState.IsValid)
            {
                GetModelValidation(ModelState);
            }
            else
            {
                _solicitacaoService.Salvar(entidade);
                //Commit();
                return base.RetornoSalvar(retornosalvar, entidade.Id);
            }

            return View(entidade);
        }
        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        [Validacao(IgnorarId = true)]
        [ValidationExceptionFilter]
        public ActionResult Inserir(Solicitacao entidade)
        {
            try
            {
                CarregaDropDown(entidade);

                if (ModelState.IsValid)
                {
                    _solicitacaoService.Salvar(entidade);
                    Commit();
                    return RedirectToAction("Editar", new { Id = entidade.Id });
                }
                else
                {
                    GetModelValidation(ModelState);
                }
            }
            catch (ValidationException vEx)
            {
                GetValidationException(vEx);
            }
            return View(entidade);
        }


        [HttpPost]
        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult InserirEmail(EmailModelDTO email)
        {
            //TODO pendente baixar como anexo dois PDF. 
            //1 solic por email da seguradora; 2 pagina/site com dados da solicitacao(ver questão de remover valores antes de enviar para o vitoriador
            var solicitacao = new FactorySolicitacaoEmail(_solicitacaoService, _seguradoraService, null, _produtoService).Criar(email);

            var endereco = solicitacao.Endereco;

            TryValidation(() =>
            {
                var geoCordenadas = _enderecoService.BuscarGeoCordenadas(endereco);
                endereco.Latitude = geoCordenadas.Latitude;
                endereco.Longitude = geoCordenadas.Longitude;

            }, true);

            CarregaDropDown(solicitacao, true);
            solicitacao.TpSituacao = TipoSituacaoProcessoEnum.EmElaboracao;
            ViewBag.lstLog = new List<LogAuditoria>();

            return View("Inserir", solicitacao);

        }

        [HttpGet]
        public ActionResult InserirEmail(Solicitacao model)
        {
            CarregaDropDown(model);

            return View("Inserir");
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult Reinspecao(int id)
        {

            var resinspecao = _solicitacaoService.Reinspecao(id);
            Commit();
            return ResponseResult(true, message: @"Solicitação de reinspeção gerada com sucesso. Consulte o registro <a href='/Solicitacao/Editar/{0}'> Cód.: {0}</a>".Formata(resinspecao.Id.ToString()));


        }

        #region DropDown

        private void CarregaDropDown(Solicitacao model, bool indSolicitacaoEmail = false)
        {
            if (ViewBag.OrigemForm != "Editar")
            {
                ViewBag.lstSeguradora = _seguradoraService.Listar(new SeguradoraFilter()
                {
                    Id = indSolicitacaoEmail ? model.IdSeguradora : (int?)null,
                    CampoOrdenacao = CampoOrdenacaoSeguradora.NomeSeguradora
                }).ToSelectList(i => i.Id, i => i.NomeSeguradora, !indSolicitacaoEmail, model.IdSeguradora);

                ViewBag.lstProduto = _produtoService.Listar(new ProdutoFilter()
                {
                    IdSeguradora = model.IdSeguradora,
                    CampoOrdenacao = CampoOrdenacaoProduto.NomeProduto
                }).ToSelectList(i => i.Id, i => i.NomeProduto, indSolicitacaoEmail, model.IdProduto);

            }

            ViewBag.lstSolicitante = _solicitanteService.ddlSolicitante(model.IdSeguradora)
                                            .ToSelectList(i => i.Id, i => i.Operador.NomeOperador, true, model.IdSolicitante);

            ViewBag.lstFilial = _filialService.Listar(new FilialFilter
            {
                IdSeguradora = model.IdSeguradora,
                CampoOrdenacao = CampoOrdenacaoFilial.NomeFilial
            }).ToSelectList(i => i.Id, i => i.NomeFilial, true, model.IdFilial);
        }

        [HttpGet]
        public JsonResult SelecionarProdutos(int idSeguradora)
        {
            return ResponseResult(true, content: _produtoService.Listar(new ProdutoFilter { IdSeguradora = idSeguradora, CampoOrdenacao = CampoOrdenacaoProduto.NomeProduto })
                .Select(i => new { i.Id, i.NomeProduto }));
        }

        [HttpGet]
        public JsonResult SelecionarSolicitantes(int idSeguradora)
        {
            return ResponseResult(true, content: _solicitanteService.Listar(new SolicitanteFilter { IdSeguradora = idSeguradora, CampoOrdenacao = CampoOrdenacaoSolicitante.NomeSolicitante })
                .Select(i => new { i.Id, i.Operador.NomeOperador }));
        }

        [HttpGet]
        public JsonResult SelecionarSolicitante(int idSolicitante)
        {
            var op = _operadorService.Buscar(idSolicitante);

            return ResponseResult(true,
                content: new
                {
                    Email = op?.Email,
                    Telefone = op?.Telefone
                });
        }

        [HttpGet]
        public JsonResult SelecionarFilial(int idSeguradora)
        {
            var lst = _filialService.Listar(new FilialFilter { IdSeguradora = idSeguradora, CampoOrdenacao = CampoOrdenacaoFilial.NomeFilial });

            return ResponseResult(true, content: lst.Select(i => new { i.NomeFilial, i.Id }));
        }

        #endregion DropDown

        #region Metodos Auxiliares
        private void CarregaLogsAuditoria(Solicitacao entidade)
        {
            ViewBag.lstLog = new List<LogAuditoria>();
            return;
            //var lstEntidadeLogs = new List<IEntity>();
            //lstEntidadeLogs.Add(entidade);
            //if (entidade.Endereco != null)
            //    lstEntidadeLogs.Add(entidade.Endereco);

            //if (entidade.Foto.Any())
            //    lstEntidadeLogs.AddRange(entidade.Foto.ToList());

            //if (entidade.Cobertura.Any())
            //    lstEntidadeLogs.AddRange(entidade.Cobertura.ToList());

            //if (entidade.Agendamento.Any())
            //    lstEntidadeLogs.AddRange(entidade.Agendamento.ToList());

            //if (entidade.LancamentoFinanceiro.Any())
            //    lstEntidadeLogs.AddRange(entidade.LancamentoFinanceiro.ToList());


            //ViewBag.lstLog = _serviceLogAuditoria.Listar(lstEntidadeLogs);
        }

        #endregion

        #region " Aba Gestao "

        [HttpGet]
        public ActionResult VistoriadorSugerido(int Id)
        {
            Solicitacao solicitacao = _solicitacaoService.BuscarSolicitacaoEndereco(Id);

            if (solicitacao.Endereco.IsNull() || !solicitacao.Endereco.Latitude.HasValue)
                throw new ValidationException(MensagensValidacaoServicos.RnEnderecoBuscarGeoZeroResult);

            string uf = solicitacao.Endereco.SiglaUf;
            string muni = solicitacao.Endereco.NomeMunicipio;
            double llat = solicitacao.Endereco.Latitude.Value;
            double llong = solicitacao.Endereco.Longitude.Value;

            ViewBag.lstOperadorDistancia = _operadorService.ListarOperadorDistanciaPorProximidadeGeo(uf, muni, solicitacao.IdProduto, solicitacao.Id, solicitacao.IdContratoLancamentoValor, llat, llong);


            ViewBag.Id = Id;
            return View("AbaGestao/VistoriadorSugerido", solicitacao);
        }


        [HttpGet]
        public ActionResult VistoriadorPesquisar(int Id)
        {

            ViewBag.IdSolicitacao = Id;
            Solicitacao solicitacao = _solicitacaoService.Buscar(Id);

            List<Vistoriador> vistoriador = _vistoriadorService.ListarVistoriadorPorProduto(solicitacao.IdProduto).ToList();

            return View("AbaGestao/VistoriadorPesquisar", vistoriador);
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult SalvarVistoriador(int Id, int IdVistoriador, string txtJustificativaVistoriadorDefinido)
        {
            _solicitacaoService.SalvarAtividadeDefinirVistoriador(Id, IdVistoriador, txtJustificativaVistoriadorDefinido);
            Commit();

            TempData["abaAtiva"] = "idtabVistoriador";
            return base.RetornoSalvar(RetornoSalvarEnum.Editar, Id);
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult EditarCustoDeslocamentoAcordado(int Id, decimal CustoDeslocamentoAcordado)
        {
            _solicitacaoService.PreAcordoCustoDeslocamento(Id, CustoDeslocamentoAcordado);
            Commit();
            TempData["abaAtiva"] = "idtabVistoriador";
            return ResponseResult(true);
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult EditarVlrPagamentoVistoriaAcordado(int Id, decimal VlrPagamentoVistoriaAcordado)
        {

            _solicitacaoService.PreAcordoVlrPagamentoVistoriaAcordado(Id, VlrPagamentoVistoriaAcordado);
            Commit();
            TempData["abaAtiva"] = "idtabVistoriador";
            return ResponseResult(true);
        }

        [HttpGet]
        public ActionResult InformarAgendamento(int Id)
        {
            Solicitacao solicitacao = _solicitacaoService.Buscar(Id);

            _solicitacaoService.RN_ValidarExiteAgendamento(solicitacao);


            ViewBag.IdSolicitacao = solicitacao.Id;
            ViewBag.CodSeguradora = solicitacao.CodSeguradora;
            ViewBag.IndAgendaRepostaPorEmail = solicitacao.Seguradora.IndAgendaRepostaPorEmail;

            if (solicitacao.Seguradora.IndAgendaRepostaPorEmail)
            {

                ViewBag.EmailAgendaRepostaPorEmail = solicitacao.Seguradora.EmailRemetenteSolicitacao;


                Agendamento agendaRealizada = _solicitacaoService.AgendamentoVigenteSolicitacao(solicitacao);
                ViewBag.valorData = string.Empty;
                ViewBag.valorHora = string.Empty;

                if (agendaRealizada != null && (agendaRealizada.TipoAgendamento == TipoAgendamentoEnum.Agendar || agendaRealizada.TipoAgendamento == TipoAgendamentoEnum.Reagendar))
                {
                    ViewBag.valorData = agendaRealizada.DthAgendamento.FormatoData();
                    ViewBag.valorHora = agendaRealizada.DthAgendamento.FormatoHora();
                }

                ViewBag.EmailRemetenteSolicitacao = solicitacao.Seguradora.EmailRemetenteSolicitacao;
            }



            return View("AbaGestao/InformarAgendamento");
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult InformarAgendamento(int Id, TipoNotificacaoEnum rbtTipoNotificacao, Comunicacao comunicacaoAssuntoTexto)
        {

            _solicitacaoService.SalvarAtividadeInformarAgendamento(Id, rbtTipoNotificacao, comunicacaoAssuntoTexto);
            Commit();
            TempData["abaAtiva"] = "idtabRelacionamento";
            return base.RetornoSalvar(RetornoSalvarEnum.Editar, Id);
        }


        [HttpGet]
        public ActionResult RegistrarComunicacao(int IdSolicitacao)
        {
            Solicitacao model = _solicitacaoService.Buscar(IdSolicitacao);

            ViewBag.IdSolicitacao = IdSolicitacao;
            ViewBag.IdSeguradora = model.IdSeguradora;
            ViewBag.IdVistoriador = model.IdVistoriador;
            ViewBag.lstTipoAssunto = _tipoAssuntoService.Listar(new TipoAssuntoFilter()).ToSelectList(i => i.Id, i => i.NomeAssunto, true);


            return View("AbaGestao/RegistrarComunicacao");
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult RegistrarComunicacao(Comunicacao comunicacao, bool SalvarEnviar)
        {
            _solicitacaoService.RegistrarComunicacao(comunicacao, SalvarEnviar);

            Commit();
            TempData["abaAtiva"] = "idtabRelacionamento";
            return base.RetornoSalvar(RetornoSalvarEnum.Editar, comunicacao.IdSolicitacao);
        }


        [HttpGet]
        public ActionResult PesquisarAnalista(int Id)
        {
            List<Analista> model = _analistaService.Listar(new AnalistaFilter()).ToList();
            ViewBag.IdSolicitacao = Id;

            return View("AbaGestao/PesquisarAnalista", model);
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult DefinirAnalista(int Id, int IdAnalista)
        {
            _solicitacaoService.SalvarAtividadeDefinirAnalista(Id, IdAnalista);
            Commit();

            TempData["abaAtiva"] = "idtabAnalista";
            return base.RetornoSalvar(RetornoSalvarEnum.Editar, Id);
        }

        #endregion " Aba Gestao "

        #region "Aba Inspecao"
        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult Agendar(int Id, string txtDataAgenda, string txtHoraAgenda, string MotivoCancelamentoReagendamento, string acaoagenda, string txtContatoAgendamento)
        {

            Solicitacao solicitacao = _solicitacaoService.BuscarParaAgendar(Id);

            switch (acaoagenda)
            {
                case "comunicar":
                    _agendamentoService.Comunicar(solicitacao, MotivoCancelamentoReagendamento);
                    break;
                case "reagendar":
                    _solicitacaoService.SalvarAtividadeAgendar(solicitacao, TipoAgendamentoEnum.Reagendar, DateTime.Parse(string.Format("{0} {1}", txtDataAgenda, txtHoraAgenda)), MotivoCancelamentoReagendamento, txtContatoAgendamento);
                    break;
                case "cancelaragenda":
                    _solicitacaoService.SalvarAtividadeAgendar(solicitacao, TipoAgendamentoEnum.Cancelar, motivoCancelamentoReagendamento: MotivoCancelamentoReagendamento, contatoAgendamento: txtContatoAgendamento);
                    break;
                default:
                    _solicitacaoService.SalvarAtividadeAgendar(solicitacao, TipoAgendamentoEnum.Agendar, DateTime.Parse(string.Format("{0} {1}", txtDataAgenda, txtHoraAgenda)), contatoAgendamento: txtContatoAgendamento);
                    break;
            }

            Commit();
            TempData["abaAtiva"] = "idtabAgenda";
            return base.RetornoSalvar(RetornoSalvarEnum.Editar, Id);



        }

        [HttpGet]
        public ActionResult Agendar(int Id, string acaoagenda)
        {
            Agendamento model = new Agendamento();
            model.Solicitacao = _solicitacaoService.Buscar(Id);
            ViewBag.IdSolicitacao = Id;
            ViewBag.AcaoAgenda = acaoagenda;
            return View("AbaInspecao/Agendar", model);
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public JsonResult SalvarAtividadeCroquiVistoriador(int Id, IFormFile arquivocroquie)
        {
            _solicitacaoService.SalvarAtividadeCroqui(Id, arquivocroquie, TipoArquivoAnexoEnum.Croqui);
            Commit();
            TempData["abaAtiva"] = "idtabCroqui";
            return ResponseResult(true, message: MensagensSucesso.AtividadeSucesso);
        }

        [HttpGet]
        public ActionResult InformarRotaRealizada(int Id)
        {
            Solicitacao model = _solicitacaoService.Buscar(Id);


            if (model.TipoRotaVistoriaPrevista == TipoRotaVistoriaEnum.EntreVistoria)
            {
                Solicitacao solVistoriaAnterior = _solicitacaoService.RotaAnteriorSolicitacao(model);
                var strDadosEndereco = solVistoriaAnterior.Endereco.Logradouro;
                strDadosEndereco += solVistoriaAnterior.Endereco.Numero.HasValue ? ", {0}".Formata(solVistoriaAnterior.Endereco.Numero.ToString()) : string.Empty;
                strDadosEndereco += ", {0}".Formata(solVistoriaAnterior.Endereco.Bairro);
                strDadosEndereco += " - {0}({1})".Formata(solVistoriaAnterior.Endereco.NomeMunicipio, solVistoriaAnterior.Endereco.SiglaUf);

                ViewBag.strTipoRota = "após a <abbr title=\"{0} - {1}\">Cód {2}</abbr>.".Formata(solVistoriaAnterior.Cliente.NomeRazaoSocial, strDadosEndereco, solVistoriaAnterior.Id.ToString());
            }
            else
            {
                ViewBag.strTipoRota = "a partir da \"cidade base\", ou seja, a primeira do dia.";
            }



            return View("AbaInspecao/InformarRotaRealizada", model);
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult InformarRotaRealizada(int Id, TipoOpcaoInformarRota rbtTipoIntinerario, string txtDataAgenda = null, string txtHoraAgenda = null, string TxtJustificativaDeslocamentoRealizado = null, decimal? DeslocamentoRealizado = null)
        {

            //TODO: Corrigir aqui quando não informado DeslocamentoRealizado
            Solicitacao model = _solicitacaoService.Buscar(Id);

            DateTime? dataAgenda = rbtTipoIntinerario == TipoOpcaoInformarRota.RealizadoDeslocamentoIntinerarioDiferente ? DateTime.Parse(string.Format("{0} {1}", txtDataAgenda, txtHoraAgenda)) : default(DateTime);

            _solicitacaoService.SalvarAtividadeInformarRotaRealizada(Id, rbtTipoIntinerario, TxtJustificativaDeslocamentoRealizado, DeslocamentoRealizado, dataAgenda);
            Commit();
            TempData["abaAtiva"] = "idtabRoda";
            return base.RetornoSalvar(RetornoSalvarEnum.Editar, Id);

        }


        [HttpPost]
        public ActionResult RealizarVistoria(int Id)
        {
            _solicitacaoService.SalvarAtividadeRealizarVistoria(Id);
            Commit();
            TempData["abaAtiva"] = "idtabFotos";
            return ResponseResult(true, message: MensagensSucesso.AtividadeSucesso);
        }

        #endregion "Aba Inspecao"

        #region "Acoes WorkFlow"       

        [HttpGet]
        public async Task<ActionResult> Enviar(int Id)
        {
            try
            {


                Solicitacao solicitacao = await _solicitacaoRepository.BuscarParaEnviar(Id);
                var lstRet = _workflow.ProximoMovimento(solicitacao.TpSituacao, WFTipoAcao.Enviar);
                var lstTipoSituacao = lstRet.Cast<TipoSituacaoProcessoEnum>().ToList();
                switch (lstTipoSituacao.FirstOrDefault())
                {
                    case TipoSituacaoProcessoEnum.EnviadoParaVistoria:
                        _solicitacaoService.RN_ValidarExiteVistoriadorDefinido(solicitacao);
                        break;
                    case TipoSituacaoProcessoEnum.EnviadoParaAnalise:
                        _solicitacaoService.RN_ValidarExiteAgendamento(solicitacao);
                        _solicitacaoService.RN_ValidarExiteInspecaoRealizada(solicitacao);
                        break;
                    case TipoSituacaoProcessoEnum.EnviadoParaFinanceiro:
                        _solicitacaoService.RN_ValidarExiteLaudoConcluido(solicitacao);
                        break;
                }

                return View("WorkFlow/Enviar", solicitacao);

            }
            catch (ValidationException vEx)
            {
                return base.JsonObject(new ValidationExceptionResult(this.Log, this.Url).ResponseResultException(vEx));
            }

        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public async Task<ActionResult> Enviar(int Id, string txtMensagemMovimento, IFormFile arquivo)
        {

            await _solicitacaoWorkFlowService.Enviar(Id, txtMensagemMovimento, null);
            Commit();

            return base.RetornoSalvar(RetornoSalvarEnum.Editar, Id);
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult Apropriar(int Id)
        {

            _solicitacaoWorkFlowService.Apropriar(Id);
            Commit();
            return ResponseResult(true);

        }

        [HttpGet]
        public ActionResult Cancelar(int Id)
        {
            Solicitacao solicitacao = _solicitacaoService.Buscar(Id);
            return View("WorkFlow/Cancelar", solicitacao);
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult Cancelar(int Id, string txtMensagemMovimento)
        {

            _solicitacaoWorkFlowService.Cancelar(Id, txtMensagemMovimento);
            Commit();
            return base.RetornoSalvar(RetornoSalvarEnum.Editar, Id);

        }



        [HttpGet]
        public ActionResult Devolver(int Id)
        {
            Solicitacao solicitacao = _solicitacaoService.BuscarComMovimento(Id);
            return View("WorkFlow/Devolver", solicitacao);
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult Devolver(int Id, string txtMotivo, TipoMotivoEnum? tipoMotivo)
        {

            _solicitacaoWorkFlowService.Devolver(Id, txtMotivo, tipoMotivo);
            Commit();
            return base.RetornoSalvar(RetornoSalvarEnum.Editar, Id);

        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult Concluir(int Id)
        {
            _solicitacaoWorkFlowService.Concluir(Id);
            Commit();
            return ResponseResult(true);
        }


        #endregion

        #region "Aba Financeiro"
        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public JsonResult GerarValorizar(int Id)
        {
            _solicitacaoService.ValorizarFinanceiro(Id);
            Commit();
            TempData["abaAtiva"] = "idtabLancamentosFinanceiros";
            return ResponseResult(true, message: MensagensSucesso.AtividadeSucesso);
        }


        [HttpGet]
        public ActionResult RegistrarLancamentoFinanceiro(int Id)
        {
            ViewBag.IdSolicitacao = Id;
            return View("AbaLancamentoFinanceiro/RegistrarLancamentoFinanceiro");
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult RegistrarLancamentoFinanceiro(int Id, LancamentoFinanceiro lancamento)
        {

            _solicitacaoService.RegistrarLancamentoFinanceiro(Id, lancamento);
            Commit();
            TempData["abaAtiva"] = "idtabLancamentosFinanceiros";
            return base.RetornoSalvar(RetornoSalvarEnum.Editar, Id);
        }
        #endregion

        #region "Aba Analise "

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public JsonResult SalvarAtividadeLaudoAnalista(int Id, IFormFile arquivolaudoanalista, decimal? AreaConstruida = null, int? BlocoConstruido = null, int? CasaConstruida = null, int? QtdEquipamento = null, bool? IndRelatorioExigenciaMelhoria = null)
        {

            _solicitacaoService.SalvarAtividadeLaudoAnalista(Id, arquivolaudoanalista, AreaConstruida, BlocoConstruido, CasaConstruida, QtdEquipamento, IndRelatorioExigenciaMelhoria);
            Commit();
            TempData["abaAtiva"] = "idtabAnalise";
            return ResponseResult(true, message: MensagensSucesso.AtividadeSucesso);
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public JsonResult SalvarAtividadeCroquiAnalista(int Id, IFormFile arquivocroquieanalista)
        {
            _solicitacaoService.SalvarAtividadeCroqui(Id, arquivocroquieanalista, TipoArquivoAnexoEnum.CroquiAnalista);
            Commit();
            TempData["abaAtiva"] = "idtabCroquiAnalista";
            return ResponseResult(true, message: "Atividade Concluída!");
        }

        #endregion

    }

}





using Differencial.Domain.Contracts;
using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Contracts.Util;
using Differencial.Domain.Contracts.Validation;
using Differencial.Domain.Queries;
using Differencial.Domain.UOW;
using Differencial.Domain.Util;
using Differencial.Domain.Validation;
using Differencial.Infra;
using Differencial.Queries.Queries;
using Differencial.Repository;
using Differencial.Repository.Context;
using Differencial.Repository.Repositories;
using Differencial.Service;
using Differencial.Service.Services;
using Differencial.Service.ServiceUtility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Differencial.IOC
{
    public static class IOC
    {

        //public Container Container { get; private set; }
        //public T GetInstance<T>()
        //{
        //    return (T)Container.GetInstance(typeof(T));
        //}


        public static void ResolveDependencies(this IServiceCollection services)
        {
             
            services.AddTransient<DifferencialContext>();
            services.AddScoped<IDbContextFactory, ContextFactory>();
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<TransactionFilter>();
             

            #region Repositories
            services.AddScoped<ILogAuditoriaRepository, LogAuditoriaRepository>();
            services.AddScoped<ISeguradoraRepository, SeguradoraRepository>();
            services.AddScoped<ITipoInspecaoRepository, TipoInspecaoRepository>();
            services.AddScoped<IOperadorRepository, OperadorRepository>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IVistoriadorRepository, VistoriadorRepository>();
            services.AddScoped<IVistoriadorProdutoRepository, VistoriadorProdutoRepository>();
            services.AddScoped<ISolicitacaoRepository, SolicitacaoRepository>();
            services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IClienteEnderecoRepository, ClienteEnderecoRepository>();
            services.AddScoped<IArquivoAnexoRepository, ArquivoAnexoRepository>();
            services.AddScoped<ILaudoFotoRepository, LaudoFotoRepository>();
            services.AddScoped<ILaudoRepository, LaudoRepository>();

            services.AddScoped<ICoberturaRepository, CoberturaRepository>();
            services.AddScoped<IMovimentacaoProcessoRepository, MovimentacaoProcessoRepository>();
            services.AddScoped<IAtividadeProcessoRepository, AtividadeProcessoRepository>();
            services.AddScoped<ISolicitanteRepository, SolicitanteRepository>();
            services.AddScoped<ILancamentoFinanceiroRepository, LancamentoFinanceiroRepository>();
            services.AddScoped<ILancamentoFinanceiroTotalRepository, LancamentoFinanceiroTotalRepository>();
            services.AddScoped<IAnalistaRepository, AnalistaRepository>();
            services.AddScoped<IAnalistaProdutoRepository, AnalistaProdutoRepository>();


            services.AddScoped<IContratoRepository, ContratoRepository>();
            services.AddScoped<IContratoLancamentoRepository, ContratoLancamentoRepository>();
            services.AddScoped<IContratoLancamentoValorRepository, ContratoLancamentoValorRepository>();
            services.AddScoped<IComunicacaoRepository, ComunicacaoRepository>();
            services.AddScoped<ITipoAssuntoRepository, TipoAssuntoRepository>();
            services.AddScoped<IFilialRepository, FilialRepository>();
            services.AddScoped<INotificacaoRepository, NotificacaoRepository>();
            services.AddScoped<INotificacaoOperadorRepository, NotificacaoOperadorRepository>();

            services.AddScoped<ILogAuditoriaQueries, LogAuditoriaQueries>();
            services.AddScoped<IVistoriadorQueries, VistoriadorQueries>();
            services.AddScoped<ISolicitacaoQueries, SolicitacaoQueries>();
            #endregion

            #region Services                  
            services.AddScoped<ISolicitacaoService, SolicitacaoService>();
            services.AddScoped<IWorkFlowSolicitacaoService, SolicitacaoService>();
            services.AddScoped<IDashboardsService, SolicitacaoService>();
            services.AddScoped<IConsultasService, SolicitacaoService>();


            services.AddScoped<ILogAuditoriaService, LogAuditoriaService>();
            services.AddScoped<ISeguradoraService, SeguradoraService>();
            services.AddScoped<ITipoInspecaoService, TipoInspecaoService>();
            services.AddScoped<IOperadorService, OperadorService>();
            services.AddScoped<IEnderecoService, EnderecoService>();
            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddScoped<IVistoriadorService, VistoriadorService>();
            services.AddScoped<IVistoriadorProdutoService, VistoriadorProdutoService>();
            services.AddScoped<IAgendamentoService, AgendamentoService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IClienteEnderecoService, ClienteEnderecoService>();
            services.AddScoped<IArquivoAnexoService, ArquivoAnexoService>();
            services.AddScoped<ILaudoFotoService, LaudoFotoService>();
            services.AddScoped<ILaudoService, LaudoService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<ICoberturaService, CoberturaService>();
            services.AddScoped<IMovimentacaoProcessoService, MovimentacaoProcessoService>();
            services.AddScoped<IAtividadeProcessoService, AtividadeProcessoService>();
            services.AddScoped<ISolicitanteService, SolicitanteService>();
            services.AddScoped<ILancamentoFinanceiroService, LancamentoFinanceiroService>();
            services.AddScoped<ILancamentoFinanceiroTotalService, LancamentoFinanceiroTotalService>();
            services.AddScoped<IWorkFlowService, WorkFlowService<Domain.Entities.MovimentacaoProcesso>>();

            services.AddScoped<IAnalistaService, AnalistaService>();
            services.AddScoped<IAnalistaProdutoService, AnalistaProdutoService>();
            services.AddScoped<IDocXService, DocXService>();

            services.AddScoped<IContratoService, ContratoService>();
            services.AddScoped<IContratoLancamentoService, ContratoLancamentoService>();
            services.AddScoped<IContratoLancamentoValorService, ContratoLancamentoValorService>();

            services.AddScoped<IComunicacaoService, ComunicacaoService>();
            services.AddScoped<ITipoAssuntoService, TipoAssuntoService>();
            services.AddScoped<IFilialService, FilialService>();

            services.AddScoped<INotificacaoService, NotificacaoService>();
            services.AddScoped<INotificacaoOperadorService, NotificacaoOperadorService>();

            #endregion

            #region "Infra"
            services.AddScoped<IServicesValidation, ServicesValidation>();
            services.AddScoped<IUsuarioService, UsuarioAplicacao>();

            services.AddSingleton<IConfiguracaoAplicativo, ConfiguracaoAplicativo>();
            services.AddSingleton<IConfiguracaoEmail, ConfiguracaoAplicativo>();
            services.AddSingleton<ICache, Cache>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ILog, Log>();


            services.AddHttpClient<IGoogleMapsService, GoogleMapsService>();
             
             
            #endregion



        }
    }
}

using Differencial.Domain;
using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Exceptions;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Differencial.Service.Services
{
    public class AgendamentoService : Service, IAgendamentoService
    {
        readonly IAgendamentoRepository _agendamentoRepositorio;
        readonly IOperadorService _operadorService;
        readonly IUsuarioService _usuarioService;

        public AgendamentoService(IUnitOfWork uow,
            IAgendamentoRepository agendamentoRepositorio,
            IOperadorService operadorService,
            IUsuarioService usuarioService)
            : base(uow)
        {
            _agendamentoRepositorio = agendamentoRepositorio;
            _operadorService = operadorService;
            _usuarioService = usuarioService;
        }

        public IEnumerable<Agendamento> Listar(AgendamentoFilter filtro)
        {
            return TryCatch(() =>
            {
                return _agendamentoRepositorio.Where(filtro);
            });
        }

        public void Salvar(Agendamento entidade)
        {
            TryCatch(() =>
            {
                entidade.Validate();

                if (entidade.Id == 0)
                    _agendamentoRepositorio.Add(entidade);
                else
                    _agendamentoRepositorio.Update(entidade);

              //  SaveChange(_usuarioService.Id);
            });
        }

        public void Excluir(int codigoUsuarioLogado, int id)
        {
            TryCatch(() =>
            {
                _agendamentoRepositorio.Delete(id);
            });
        }

        public void Agendar(DateTime dthAgendamento, Solicitacao solicitacao)
        {
            TryCatch(() =>
            {
                Operador operadorLogado = _operadorService.Buscar(_usuarioService.Id);

                RN_VistoriadorSemPermissao(operadorLogado, solicitacao);
                RN_AgendaConflitante(dthAgendamento, solicitacao.IdVistoriador.Value, solicitacao.Id);

                //if (dthAgendamento.Date < DateTime.Now.Date)
                //    throw new ValidationException("Data Agendamento inválida, não é possivel realizar um agandamento para uma data passada.");

                var agendasAnteriores = _agendamentoRepositorio.Where(w => w.IdSolicitacao == solicitacao.Id && w.IndCancelado == false).ToList();
                if (agendasAnteriores.Any())
                {
                    SalvarCancelamento(agendasAnteriores);
                }
                

                var agenda = new Agendamento
                {
                    IdSolicitacao = solicitacao.Id,
                    DthAgendamento = dthAgendamento,
                    IdVistoriador = solicitacao.IdVistoriador.Value,
                    IndCancelado = false,
                    TipoAgendamento = TipoAgendamentoEnum.Agendar
                };

                Salvar(agenda);
               
            });
        }

        private void SalvarCancelamento(List<Agendamento> agendasAnteriores)
        {
            foreach (var agendaAnterior in agendasAnteriores)
            {
                agendaAnterior.IndCancelado = true; 
                _agendamentoRepositorio.Update(agendaAnterior); 
                
            }
          //  SaveChange(_usuarioService.Id);
        }


        public void Reagendar(DateTime dthAgendamento, string motivoCancelamentoReagendamento, Solicitacao solicitacao)
        {
            TryCatch(() =>
            {
                Operador operadorLogado = _operadorService.Buscar(_usuarioService.Id);

                RN_VistoriadorSemPermissao(operadorLogado, solicitacao);
                RN_AgendaConflitante(dthAgendamento, solicitacao.IdVistoriador.Value, solicitacao.Id);
                //                if (dthAgendamento.Date < DateTime. .Date)
                //                    throw new ValidationException("Data Agendamento inválida, não é possivel realizar um agandamento para uma data passada.");

                var agendasAnteriores = _agendamentoRepositorio.Where(w => w.IdSolicitacao == solicitacao.Id && w.IndCancelado == false).ToList();

                if (!String.IsNullOrEmpty(motivoCancelamentoReagendamento) && agendasAnteriores.Any())
                    agendasAnteriores.Last().MotivoCancelamentoReagendamento = motivoCancelamentoReagendamento;

                if (agendasAnteriores.Any())
                { 
                    SalvarCancelamento(agendasAnteriores); 
                }


                var agenda = new Agendamento
                {
                    IdSolicitacao = solicitacao.Id,
                    DthAgendamento = dthAgendamento,
                    IdVistoriador = solicitacao.IdVistoriador.Value,
                    IndCancelado = false,
                    TipoAgendamento = TipoAgendamentoEnum.Reagendar
                };

                Salvar(agenda);
            });
        }

        
        

        public void Comunicar( Solicitacao solicitacao, string mensagem)
        {
             TryCatch(() =>
            {
                var agendamentoComincacao = new Agendamento
                {
                    TipoAgendamento = TipoAgendamentoEnum.Comunicar,
                    MotivoCancelamentoReagendamento = mensagem,
                    IndCancelado = false,
                    IdVistoriador = solicitacao.IdVistoriador.Value,
                    IdSolicitacao = solicitacao.Id
                };

                Salvar(agendamentoComincacao); 
           
            });
        }

        private void RN_VistoriadorSemPermissao(Operador operadorLogado, Solicitacao solicitacao)
        {
            if (!(operadorLogado.IndGerente || operadorLogado.IndAssessor) && operadorLogado.Id != solicitacao.IdVistoriador)
                throw new ValidationException("Vistoriador não encontrado ou não tem permissão para realizar esse agendamento.");

        }

        private void RN_AgendaConflitante(DateTime dthAgendamento, int idVistoriador, int idSolicitacao)
        {
             var lstAgendasDoDia = ListarAgendamentosVistoriadorDiaVigentes(idVistoriador, dthAgendamento).ToList();

            if(lstAgendasDoDia.Any(i=> i.DthAgendamento.Value.Date == dthAgendamento.Date 
                                  && i.DthAgendamento.Value.Hour == dthAgendamento.Hour
                                  && i.DthAgendamento.Value.Minute == dthAgendamento.Minute
                                  && i.IdSolicitacao != idSolicitacao))
            {
                throw new ValidationException("Não é possível registrar essa agenda, o vistoriador possúi uma agenda para mesma hora e minuto de outra solicitação.");
            }


        }

        public IEnumerable<Agendamento> ListarAgendamentosVistoriadorDiaVigentes(int idVistoriador, DateTime dataAgenda)
        {
            return TryCatch(() =>
            {
                return _agendamentoRepositorio.ListarAgendamentosVistoriadorDiaVigentes(idVistoriador, dataAgenda);
            });
        }

        public IEnumerable<Agendamento> ListarAgendamentosVistoriadorDiaVigentesCancelada(int idVistoriador, DateTime dataAgenda, int IdSolicCancelada)
        {
            return TryCatch(() =>
            {
                return _agendamentoRepositorio.ListarAgendamentosVistoriadorDiaVigentesCancelada(idVistoriador, dataAgenda, IdSolicCancelada);
            });
        }
    }
}
using Differencial.Domain.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Differencial.Domain;
using Differencial.Domain.UOW;
using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Exceptions;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Enums.WorkFlow;
using Differencial.Domain.Annotation;

namespace Differencial.Service.Services
{
    public class WorkFlowService<M> : Service, IWorkFlowService
        where M : IWorkFlowMovimentacaoProcesso, new() 
    {
        IMovimentacaoProcessoService _movimentacaoProcessoService;
        IUsuarioService _usuario;
        IAtividadeProcessoService _atividadeProcessoService;

        public WorkFlowService(IUnitOfWork uow,
            IMovimentacaoProcessoService movimentacaoProcessoService,
            IUsuarioService usuario,
              IAtividadeProcessoService atividadeProcessoService)
            : base(uow)
        {
            _movimentacaoProcessoService = movimentacaoProcessoService;
            _usuario = usuario;
            _atividadeProcessoService = atividadeProcessoService;

        }

        private int WorkFlowMapMovimentar(int? situacaoAtual, WFTipoAcao tipoAcao)
        {

            TipoSituacaoProcessoEnum? retorno = null;

            //Trata Inicio de Processo
            if (tipoAcao == WFTipoAcao.Criar)
            {
                switch ((TipoSituacaoProcessoEnum?)situacaoAtual)
                {
                    case TipoSituacaoProcessoEnum.EmElaboracaoSolicitante:
                    case TipoSituacaoProcessoEnum.EmElaboracao:
                        retorno = (TipoSituacaoProcessoEnum)situacaoAtual;
                        break;
                }
            }
            //Trata Fim de Processo
            else if (tipoAcao == WFTipoAcao.Concluir)
            {
                switch ((TipoSituacaoProcessoEnum?)situacaoAtual)
                {
                    case TipoSituacaoProcessoEnum.ApropriadoPeloFinanceiro:
                        retorno = TipoSituacaoProcessoEnum.Concluido;
                        break;
                }
            }
            else//Trata Meio de Processo
            {

                switch ((TipoSituacaoProcessoEnum?)situacaoAtual)
                {
                    case null:
                        break;

                    case TipoSituacaoProcessoEnum.ApropriadoPeloSolicitante:
                    case TipoSituacaoProcessoEnum.EmElaboracaoSolicitante:
                        switch (tipoAcao)
                        {
                            case WFTipoAcao.Apropriar:
                                retorno = TipoSituacaoProcessoEnum.ApropriadoPeloSolicitante;
                                break;
                            case WFTipoAcao.Enviar:
                                retorno = TipoSituacaoProcessoEnum.EnviadoParaGerencia;
                                break;
                            case WFTipoAcao.Cancelar:
                                retorno = TipoSituacaoProcessoEnum.CanceladoPelaSeguradora;
                                break;

                        }

                        break;

                    case TipoSituacaoProcessoEnum.DevolvidoParaSeguradora:
                        switch (tipoAcao)
                        {
                            case WFTipoAcao.Apropriar:
                                retorno = TipoSituacaoProcessoEnum.ApropriadoPeloSolicitante;
                                break;
                        }

                        break;
                    case TipoSituacaoProcessoEnum.EmElaboracao:
                    case TipoSituacaoProcessoEnum.DevolvidoParaGerencia:
                    case TipoSituacaoProcessoEnum.ApropriadoGerente:
                    case TipoSituacaoProcessoEnum.EnviadoParaGerencia:

                        switch (tipoAcao)
                        {
                            case WFTipoAcao.Criar:
                                retorno = TipoSituacaoProcessoEnum.EmElaboracao;
                                break;
                            case WFTipoAcao.Enviar:
                                retorno = TipoSituacaoProcessoEnum.EnviadoParaVistoria;
                                break;
                            case WFTipoAcao.Apropriar:
                                retorno = TipoSituacaoProcessoEnum.ApropriadoGerente;
                                break;
                            case WFTipoAcao.Cancelar:
                                retorno = TipoSituacaoProcessoEnum.Cancelado;
                                break;
                            case WFTipoAcao.Devolver:
                                retorno = TipoSituacaoProcessoEnum.DevolvidoParaSeguradora;
                                break;
                        }
                        break;


                    case TipoSituacaoProcessoEnum.EnviadoParaVistoria:
                        switch (tipoAcao)
                        {
                            case WFTipoAcao.Apropriar:
                                retorno = TipoSituacaoProcessoEnum.ApropriadoVistoriador;
                                break;
                            case WFTipoAcao.Devolver:
                                retorno = TipoSituacaoProcessoEnum.DevolvidoParaGerencia;
                                break;
                        }


                        break;
                    case TipoSituacaoProcessoEnum.ApropriadoVistoriador:
                        switch (tipoAcao)
                        {
                            case WFTipoAcao.Enviar:
                                retorno = TipoSituacaoProcessoEnum.EnviadoParaAnalise;
                                break;
                            case WFTipoAcao.Devolver:
                                retorno = TipoSituacaoProcessoEnum.DevolvidoParaGerencia;
                                break;
                        }

                        if (tipoAcao == WFTipoAcao.Enviar)
                            retorno = TipoSituacaoProcessoEnum.EnviadoParaAnalise;
                        else if (tipoAcao == WFTipoAcao.Devolver)
                            retorno = TipoSituacaoProcessoEnum.DevolvidoParaGerencia;


                        break;
                    case TipoSituacaoProcessoEnum.EnviadoParaAnalise:
                        if (tipoAcao == WFTipoAcao.Apropriar)
                            retorno = TipoSituacaoProcessoEnum.ApropriadoPelaAnalise;


                        break;
                    case TipoSituacaoProcessoEnum.ApropriadoPelaAnalise:
                        switch (tipoAcao)
                        {
                            case WFTipoAcao.Enviar:
                                retorno = TipoSituacaoProcessoEnum.EnviadoParaFinanceiro;
                                break;
                            //case WFTipoAcao.Devolver:
                            //    retorno = TipoSituacaoProcessoEnum.DevolvidoParaVistoriador;
                            //  break;
                            case WFTipoAcao.Concluir:
                                retorno = TipoSituacaoProcessoEnum.Concluido;
                                break;
                            case WFTipoAcao.Apropriar:
                                retorno = TipoSituacaoProcessoEnum.ApropriadoPelaAnalise;
                                break;
                        }
                        break;
                    //case TipoSituacaoProcessoEnum.DevolvidoParaVistoriador:
                    //    if (tipoAcao == WFTipoAcao.Apropriar)
                    //        retorno = TipoSituacaoProcessoEnum.ApropriadoVistoriador;
                    //    break;

                    case TipoSituacaoProcessoEnum.ApropriadoPeloFinanceiro:
                    case TipoSituacaoProcessoEnum.EnviadoParaFinanceiro:
                        if (tipoAcao == WFTipoAcao.Apropriar)
                            retorno = TipoSituacaoProcessoEnum.ApropriadoPeloFinanceiro;
                        break;

                }

            }



            if (retorno == null)
                throw new ValidationException("A ação não pode ser realizada pois o fluxo não permite {0} quando a situação é {1}".Formata(tipoAcao.ToString(), situacaoAtual.ToString()));
            else
                return (int)retorno;
        }

        private IWorkFlowMovimentacaoProcesso CriarMovimentacaoProcesso(int IdProcesso, string textoMovimentacao)
        {

            var movimento = Activator.CreateInstance<M>();
            movimento.DthMovimentacao = DateTime.Now;
            movimento.IdOperadorOrigem = _usuario.Id;
            movimento.TipoSituacaoMovimento = TipoSituacaoMovimentoEnum.Nova;
            movimento.IdSolicitacao = IdProcesso;
            movimento.TextoMovimentacao = textoMovimentacao;

            return movimento;
        }


        private void ConcluirUltimaMovimentacao(IWorkFlowInstanciaProcesso instanciaProcesso)
        {
            var ultimoMovimento = instanciaProcesso.WorkFlowInstanciaProcesso.LastOrDefault();

            if (ultimoMovimento != null)
            {
                ultimoMovimento.TipoSituacaoMovimento = TipoSituacaoMovimentoEnum.Concluido;
                ultimoMovimento.DthConclusao = DateTime.Now;
            }
        }

        public void Criar(IWorkFlowInstanciaProcesso instanciaProcesso, TipoSituacaoProcessoEnum situacaoInicio = TipoSituacaoProcessoEnum.EmElaboracao)
        {
            TryCatch(() =>
            {
                IWorkFlowMovimentacaoProcesso novaMovimentacao = CriarMovimentacaoProcesso(instanciaProcesso.Id, string.Empty);
                novaMovimentacao.InstanciaProcesso = instanciaProcesso;
                novaMovimentacao.IdOperadorDestino = novaMovimentacao.IdOperadorOrigem;
                novaMovimentacao.TipoSituacaoProcesso = (TipoSituacaoProcessoEnum)WorkFlowMapMovimentar((int)situacaoInicio, WFTipoAcao.Criar);
                _movimentacaoProcessoService.Inserir(novaMovimentacao);

               // instanciaProcesso.WorkFlowInstanciaProcesso.Add(novaMovimentacao);
                instanciaProcesso.TpSituacao = novaMovimentacao.TipoSituacaoProcesso;
                //Apropriação
                instanciaProcesso.IdOperadorApropriado = _usuario.Id;

               var atividades = _atividadeProcessoService.Criar(instanciaProcesso);
                instanciaProcesso.AtividadeProcesso = atividades;

            });
        }

        public void Apropriar(IWorkFlowInstanciaProcesso instanciaProcesso)
        {
            TryCatch(() =>
            {
                ConcluirUltimaMovimentacao(instanciaProcesso);

                if (instanciaProcesso.WorkFlowInstanciaProcesso.Any())
                    instanciaProcesso.WorkFlowInstanciaProcesso.Last().IdOperadorDestino = _usuario.Id;


                IWorkFlowMovimentacaoProcesso novaMovimentacao = CriarMovimentacaoProcesso(instanciaProcesso.Id, string.Empty);
                novaMovimentacao.TipoSituacaoProcesso = (TipoSituacaoProcessoEnum)WorkFlowMapMovimentar((int)instanciaProcesso.TpSituacao, WFTipoAcao.Apropriar);
                _movimentacaoProcessoService.Inserir(novaMovimentacao);

                instanciaProcesso.TpSituacao = novaMovimentacao.TipoSituacaoProcesso;

                instanciaProcesso.IdOperadorApropriado = _usuario.Id;  //Apropriação
            });
        }

        public int Enviar(IWorkFlowInstanciaProcesso instanciaProcesso, string textoMovimentacao)
        {
            return TryCatch(() =>
             {
                 ConcluirUltimaMovimentacao(instanciaProcesso);

                 IWorkFlowMovimentacaoProcesso novaMovimentacao = CriarMovimentacaoProcesso(instanciaProcesso.Id, textoMovimentacao);
                 novaMovimentacao.TipoSituacaoMovimento = TipoSituacaoMovimentoEnum.Aberto;
                 novaMovimentacao.TipoSituacaoProcesso = (TipoSituacaoProcessoEnum)WorkFlowMapMovimentar((int)instanciaProcesso.TpSituacao, WFTipoAcao.Enviar);
                 _movimentacaoProcessoService.Inserir(novaMovimentacao);

                 instanciaProcesso.TpSituacao = novaMovimentacao.TipoSituacaoProcesso;
                 instanciaProcesso.IdOperadorApropriado = null;
                 return (int)novaMovimentacao.TipoSituacaoProcesso;
             });
        }

        //public int Enviar(IWorkFlowInstanciaProcesso instanciaProcesso, TipoSituacaoProcessoEnum tipoTipoSituacaoProcessoEnum, string textoMovimentacao)
        //{

        //    throw new NotImplementedException();
        //}

        public int Devolver(IWorkFlowInstanciaProcesso instanciaProcesso, string textoMovimentacao)
        {
           return TryCatch(() =>
            {
                ConcluirUltimaMovimentacao(instanciaProcesso);

                IWorkFlowMovimentacaoProcesso novaMovimentacao = CriarMovimentacaoProcesso(instanciaProcesso.Id, textoMovimentacao);

                novaMovimentacao.TipoSituacaoMovimento = TipoSituacaoMovimentoEnum.Aberto;
                novaMovimentacao.TipoSituacaoProcesso = (TipoSituacaoProcessoEnum)WorkFlowMapMovimentar((int)instanciaProcesso.TpSituacao, WFTipoAcao.Devolver);
                _movimentacaoProcessoService.Inserir(novaMovimentacao);

                instanciaProcesso.TpSituacao = novaMovimentacao.TipoSituacaoProcesso;
                instanciaProcesso.IdOperadorApropriado = null;

                return (int)novaMovimentacao.TipoSituacaoProcesso;
            });
        }

        public void Cancelar(IWorkFlowInstanciaProcesso instanciaProcesso, string textoMovimentacao)
        {
            TryCatch(() =>
            {
                ConcluirUltimaMovimentacao(instanciaProcesso);

                IWorkFlowMovimentacaoProcesso novaMovimentacao = CriarMovimentacaoProcesso(instanciaProcesso.Id, textoMovimentacao);
                novaMovimentacao.TipoSituacaoMovimento = TipoSituacaoMovimentoEnum.Concluido;
                novaMovimentacao.TipoSituacaoProcesso = (TipoSituacaoProcessoEnum)WorkFlowMapMovimentar((int)instanciaProcesso.TpSituacao, WFTipoAcao.Cancelar);
                _movimentacaoProcessoService.Inserir(novaMovimentacao);

                instanciaProcesso.TpSituacao = novaMovimentacao.TipoSituacaoProcesso;
                instanciaProcesso.IdOperadorApropriado = null;

            });
        }

        public void Concluir(IWorkFlowInstanciaProcesso instanciaProcesso, string textoMovimentacao)
        {
            TryCatch(() =>
            {
                ConcluirUltimaMovimentacao(instanciaProcesso);

                IWorkFlowMovimentacaoProcesso novaMovimentacao = CriarMovimentacaoProcesso(instanciaProcesso.Id, textoMovimentacao);
                novaMovimentacao.TipoSituacaoMovimento = TipoSituacaoMovimentoEnum.Concluido;
                novaMovimentacao.TipoSituacaoProcesso = (TipoSituacaoProcessoEnum)WorkFlowMapMovimentar((int)instanciaProcesso.TpSituacao, WFTipoAcao.Concluir);
                _movimentacaoProcessoService.Inserir(novaMovimentacao);

                instanciaProcesso.TpSituacao = novaMovimentacao.TipoSituacaoProcesso;
                instanciaProcesso.IdOperadorApropriado = null;

            });
        }

        public List<WFTipoAcao> AcoesDisponiveis(IWorkFlowInstanciaProcesso instanciaProcesso, int IdOperadorLogado, int[] tipoPapel)
        {
            var lstAcoesDisponivel = new List<WFTipoAcao>();

            if (instanciaProcesso != null && instanciaProcesso.Id > 0) // Processo Já existe
            {
                var attributeEnum = instanciaProcesso.TpSituacao.Value.GetAttributeOfType<SituacaoProcessoAttribute>();

                if (instanciaProcesso.IdOperadorApropriado == IdOperadorLogado && attributeEnum.WFTipoAcaoOperadorApropriado != null)
                {
                    lstAcoesDisponivel = attributeEnum.WFTipoAcaoOperadorApropriado.ToList();
                }
                else if (tipoPapel.Contains(attributeEnum.WFPapelAtuante) && attributeEnum.WFTipoAcaoPapelAtuante != null)
                {
                    lstAcoesDisponivel = attributeEnum.WFTipoAcaoPapelAtuante.ToList();
                }
            }
            else // processo em criação/inserir
            {
                //TODO: mudar e remover esse valor fixo


                if (tipoPapel.Contains((int)TipoPapelEnum.Gerente) || tipoPapel.Contains((int)TipoPapelEnum.Solicitante))
                    lstAcoesDisponivel.Add(WFTipoAcao.Gravar);
            }


            return lstAcoesDisponivel;
        }

        public List<int> ProximoMovimento<T>(T situacaoAtual, WFTipoAcao tipoAcao)
        {
            List<int> retorno = new List<int>();
            if (situacaoAtual != null)
                retorno.Add(WorkFlowMapMovimentar(Convert.ToInt32(situacaoAtual), tipoAcao));
            else
                retorno.Add(WorkFlowMapMovimentar(null, tipoAcao));

            return retorno;
        }

        public int[] MotivoDisponiveis(TipoSituacaoProcessoEnum tipoSituacaoProcessoEnum)
        {
            var attributeEnum = tipoSituacaoProcessoEnum.GetAttributeOfType<SituacaoProcessoAttribute>();
            if (attributeEnum.WFMotivoAcao != null)
                return attributeEnum.WFMotivoAcao;
            else
                return new int[] { };

        }

        public List<TipoAtividadeEnum> AtividadesDisponiveis(IWorkFlowInstanciaProcesso instanciaProcesso, int IdOperadorLogado, int[] tipoPapel)
        {
            var lstDisponivel = new List<TipoAtividadeEnum>();
            // Processo Já existe - Editar
            if (instanciaProcesso != null && instanciaProcesso.Id > 0)
            { 
                var Apropriado = instanciaProcesso.IdOperadorApropriado == IdOperadorLogado;
                var Situacao = instanciaProcesso.TpSituacao.Value; 

                if (tipoPapel.Contains((int)TipoPapelEnum.Gerente))
                {
                    if (Apropriado && (Situacao == TipoSituacaoProcessoEnum.EmElaboracao || Situacao == TipoSituacaoProcessoEnum.ApropriadoGerente))
                    {
                        lstDisponivel.Add(TipoAtividadeEnum.DefinirVistoriador);
                        lstDisponivel.Add(TipoAtividadeEnum.DefinirAnalista);
                    }
                    else if (!Apropriado && (Situacao == TipoSituacaoProcessoEnum.EnviadoParaVistoria || Situacao == TipoSituacaoProcessoEnum.ApropriadoVistoriador))
                    {
                        lstDisponivel.Add(TipoAtividadeEnum.Agendamento);
                        lstDisponivel.Add(TipoAtividadeEnum.RealizarVistoria);
                        lstDisponivel.Add(TipoAtividadeEnum.ElaborarCroquiVistoriador);
                        lstDisponivel.Add(TipoAtividadeEnum.PrestacaoContaKm);
                    }

                    if (Situacao != TipoSituacaoProcessoEnum.EmElaboracao && Situacao != TipoSituacaoProcessoEnum.Concluido)
                    {
                        lstDisponivel.Add(TipoAtividadeEnum.InformarAgendamento);
                    }

                }

                if (tipoPapel.Contains((int)TipoPapelEnum.Vistoriador))
                {
                    if (Apropriado && Situacao == TipoSituacaoProcessoEnum.ApropriadoVistoriador)
                    {
                        lstDisponivel.Add(TipoAtividadeEnum.Agendamento);
                        //lstDisponivel.Add(TipoAtividadeEnum.ChecklistDados);
                        lstDisponivel.Add(TipoAtividadeEnum.RealizarVistoria);
                        lstDisponivel.Add(TipoAtividadeEnum.ElaborarCroquiVistoriador);
                        lstDisponivel.Add(TipoAtividadeEnum.PrestacaoContaKm);
                    }
                    else if(Apropriado == false && (Situacao == TipoSituacaoProcessoEnum.EnviadoParaAnalise || Situacao == TipoSituacaoProcessoEnum.ApropriadoPelaAnalise))
                    {
                        lstDisponivel.Add(TipoAtividadeEnum.PrestacaoContaKm);
                    }
                }

                if (tipoPapel.Contains((int)TipoPapelEnum.Assessor))
                {

                    //Realiza atividades do analista do vistoriador
                    if (Situacao == TipoSituacaoProcessoEnum.EnviadoParaVistoria || Situacao == TipoSituacaoProcessoEnum.ApropriadoVistoriador)
                    {
                        lstDisponivel.Add(TipoAtividadeEnum.Agendamento);
                        //lstDisponivel.Add(TipoAtividadeEnum.ChecklistDados);
                        lstDisponivel.Add(TipoAtividadeEnum.RealizarVistoria);
                        lstDisponivel.Add(TipoAtividadeEnum.ElaborarCroquiVistoriador);
                        lstDisponivel.Add(TipoAtividadeEnum.PrestacaoContaKm);
                    } //Realiza atividades do analista do vistoriador
                    else if (Situacao == TipoSituacaoProcessoEnum.EnviadoParaAnalise || Situacao == TipoSituacaoProcessoEnum.ApropriadoPelaAnalise)
                    {
                        lstDisponivel.Add(TipoAtividadeEnum.PrestacaoContaKm);
                    }

                     
                    //Realiza atividades do analista
                    if (Situacao == TipoSituacaoProcessoEnum.ApropriadoPelaAnalise)
                    {
                        lstDisponivel.Add(TipoAtividadeEnum.ElaborarCroquiAnalise);
                        lstDisponivel.Add(TipoAtividadeEnum.ElaborarQuadro);
                    }

                    //Realiza atividades do gerente
                    if (Situacao != TipoSituacaoProcessoEnum.EmElaboracao && Situacao != TipoSituacaoProcessoEnum.Concluido)
                    {
                        lstDisponivel.Add(TipoAtividadeEnum.InformarAgendamento);
                    }

                }

                if (tipoPapel.Contains((int)TipoPapelEnum.Analista))
                {
                    if (Apropriado && Situacao == TipoSituacaoProcessoEnum.ApropriadoPelaAnalise)
                    {
                        lstDisponivel.Add(TipoAtividadeEnum.ElaborarEnviarLaudo);
                        lstDisponivel.Add(TipoAtividadeEnum.ElaborarCroquiAnalise);
                        lstDisponivel.Add(TipoAtividadeEnum.ElaborarQuadro);
                    }

                }

                if (tipoPapel.Contains((int)TipoPapelEnum.Financeiro))
                {
                    if (Apropriado && Situacao == TipoSituacaoProcessoEnum.ApropriadoPeloFinanceiro)
                    {
                        lstDisponivel.Add(TipoAtividadeEnum.RegistrarLancamentoFinanceiro);                    
                    }
                }
            }
            return lstDisponivel;
        }
    }
}

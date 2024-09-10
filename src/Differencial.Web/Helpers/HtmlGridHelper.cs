using Differencial.Domain.Util.ExtensionMethods;
using System.Collections;
using Differencial.Domain.Entities;
using Differencial.Domain;
using System.Collections.Generic;
using System.Linq;
using System;
using Differencial.Domain.Annotation;
using Microsoft.AspNetCore.Html;

namespace Differencial.Web.Helpers
{
    public static class HtmlGridHelper
    {
        public static HtmlString Indicador(bool valorPropriedade, bool exibirTexto = false, bool exibirIcone = true)
        {
            var shtml = string.Empty;

            if (exibirIcone)
                shtml = string.Format("<i class=\"fa {0}\" aria-hidden=\"true\"></i>", valorPropriedade ? "fa-check text-success" : "fa-minus text-danger");

            shtml += string.Format("&nbsp;<span style=\"{0}\">{1}</span>", exibirTexto ? string.Empty : "display:none", valorPropriedade.ToSimNao());

            return new HtmlString(shtml);

        }

        public static HtmlString TextoSubTexto(string texto, string subTextoPequeno)
        {
            var shtml = string.Empty;
            shtml = texto;
            if (!subTextoPequeno.IsNullOrEmpty())
                shtml += string.Format("<br /><small style=\"text-nowrap\">({0})</small>", subTextoPequeno);

            return new HtmlString(shtml);
        }

        public static HtmlString SituacaoAgenda(IEnumerable<Agendamento> Agendamento, DateTime? dthEnviadoParaVistoria = null)
        {
            var shtml = string.Empty;
            var classSitAgenda = "";
            var tpSitAgendamento = "";
            var dthAgendamento = "";

            if (Agendamento.Any(w => w.TipoAgendamento == TipoAgendamentoEnum.Agendar || w.TipoAgendamento == TipoAgendamentoEnum.Reagendar))
            {
                var ultimoAgendamento = Agendamento.LastOrDefault(w => w.TipoAgendamento == TipoAgendamentoEnum.Agendar || w.TipoAgendamento == TipoAgendamentoEnum.Reagendar);
                dthAgendamento = ultimoAgendamento.DthAgendamento.FormatoDataHora();
                if (ultimoAgendamento != null)
                {
                    if ((DateTime.Now - ultimoAgendamento.DthAgendamento.Value).TotalHours >= 24)
                    {
                        tpSitAgendamento = "Laudo Atrasado";
                        classSitAgenda = "label-danger";
                    }
                    else
                    {
                        tpSitAgendamento = ultimoAgendamento.TipoAgendamento == TipoAgendamentoEnum.Agendar ? "Agendado" : "Re-Agendado";
                        classSitAgenda = "label-primary";
                    }

                }
            }
            else
            {
                if (dthEnviadoParaVistoria != null && (dthEnviadoParaVistoria.Value - DateTime.Now).TotalHours <= 24)
                    tpSitAgendamento = "Agenda Pendente";
                classSitAgenda = "label-warning";
            }


            // shtml = string.Format("{0}",tpSitAgendamento);
            shtml = string.Format("<span class=\"label {0}\">{1}</span><br />", classSitAgenda, tpSitAgendamento);
            shtml += string.Format("<small class=\"text-nowrap\">{0}</small>", dthAgendamento);

            return new HtmlString(shtml);

        }

        public static HtmlString SituacaoAgendaVistoriador(IEnumerable<Agendamento> Agendamento, DateTime? dthEnviadoParaVistoria = null)
        {
            var shtml = string.Empty;
            var classSitAgenda = "";
            var tpSitAgendamento = "";
            var dthAgendamento = "";

            if (!Agendamento.Any())
            {
                tpSitAgendamento = "Pendente";
                classSitAgenda = "label-warning";
            }
            else
            {
                Agendamento agendaRealizada = Agendamento.LastOrDefault(w => w.IndCancelado == false && w.TipoAgendamento != TipoAgendamentoEnum.Comunicar);
                if (agendaRealizada != null)
                {
                    dthAgendamento = agendaRealizada.DthAgendamento.FormatoDataHora();
                    if (agendaRealizada.TipoAgendamento == TipoAgendamentoEnum.Reagendar)
                    {
                        tpSitAgendamento = "Re-Agendado";
                        classSitAgenda = "label-primary";
                    }
                    else
                    {
                        tpSitAgendamento = "Agendado";
                        classSitAgenda = "label-primary";
                    }
                }
            }
            // shtml = string.Format("{0}",tpSitAgendamento);
            shtml = string.Format("<span class=\"label {0}\">{1}</span><br />", classSitAgenda, tpSitAgendamento);
            shtml += string.Format("<small class=\"text-nowrap\">{0}</small>", dthAgendamento);

            return new HtmlString(shtml);

        }


        public static HtmlString SituacaoProcesso(TipoSituacaoProcessoEnum? situacaoProcessoEnum)
        {
            var shtml = situacaoProcessoEnum.HasValue ? situacaoProcessoEnum.Value.GetAttributeOfType<SituacaoProcessoAttribute>().Name : string.Empty;
            return new HtmlString(shtml);
        }

        public static HtmlString TempoDecorrido(DateTime data)
        {
            return TempoDecorrido(data, DateTime.Now);
        }
        public static HtmlString TempoDecorrido(DateTime data, DateTime dataComparacao)
        {
            TimeSpan duracaoDecorrida = dataComparacao - data;
            var shtml = string.Empty;
            if (duracaoDecorrida.Hours > 0)
                shtml = duracaoDecorrida.Days.ToString() + "d "
                        + duracaoDecorrida.Hours.ToString() + "h ";
            else
                shtml = duracaoDecorrida.Minutes.ToString() + "m";

            return new HtmlString(shtml);

        }

        public static HtmlString Endereco(Endereco endereco, bool exibirLocalidade = false)
        {
            var shtml = string.Empty;

            if (endereco.Numero.HasValue)
                shtml = "{0}, {1} - {2}".Formata(endereco.Logradouro, endereco.Numero.Value.ToString(), endereco.Bairro);
            else
                shtml = "{0} - {1}".Formata(endereco.Logradouro, endereco.Bairro);

            if (exibirLocalidade)
                shtml += "<br /><small style=\"text-nowrap\">({0} - {1})</small>".Formata(endereco.NomeMunicipio, endereco.SiglaUf);

            return new HtmlString(shtml);

        }

        public static HtmlString DthUltimoMovimentoSituacao(IEnumerable<MovimentacaoProcesso> movimentacaoProcesso, TipoSituacaoProcessoEnum tipoSituacaoProcessoEnum)
        {
            var shtml = string.Empty;

            var movimento = movimentacaoProcesso.LastOrDefault(m => m.TipoSituacaoProcesso == tipoSituacaoProcessoEnum);

            if (movimento != null)
                shtml = movimento.DthMovimentacao.FormatoDataHora();

            return new HtmlString(shtml);

        }

        public static HtmlString SituacaoRotaRealizada(Solicitacao solicitacao)
        {
            var shtml = string.Empty;
            var classSitAgenda = "";
            var tpSitAgendamento = "";
            var dthAgendamento = "";

            if (solicitacao.IndCidadeBaseVistoriador.HasValue)
            {
                if (solicitacao.IndCustoVistoriaAcordado || solicitacao.IndCidadeBaseVistoriador.Value)
                {
                    tpSitAgendamento = "Não Aplicável";
                    classSitAgenda = "label-default";
                    dthAgendamento = solicitacao.IndCustoVistoriaAcordado ? "Pré-Acordado" : "Cidade Base";
                }
                else if (solicitacao.IndCustoVistoriaAcordado == false)
                {
                    if (solicitacao.CustoDeslocamentoRealizado.HasValue)
                    {
                        tpSitAgendamento = "Informado";
                        classSitAgenda = "label-primary";
                    }
                    else
                    {
                        tpSitAgendamento = "Pendente";
                        classSitAgenda = "label-warning";
                    }
                }
            }
            else
            {
                tpSitAgendamento = "";
                classSitAgenda = "";
                dthAgendamento = "";
            }


            // shtml = string.Format("{0}",tpSitAgendamento);
            shtml = string.Format("<span class=\"label {0}\">{1}</span><br />", classSitAgenda, tpSitAgendamento);
            shtml += string.Format("<small class=\"text-nowrap\">{0}</small>", dthAgendamento);

            return new HtmlString(shtml);

        }
        public static HtmlString SituacaoAtividade(IEnumerable<AtividadeProcesso> atividadeProcesso, TipoAtividadeEnum tipoAtividade)
        {
            string classSpan, tpSituacao, dthConclusao, shtml = "";

            if (atividadeProcesso != null && atividadeProcesso.Any())
            { 
                var atividade = atividadeProcesso.FirstOrDefault(i => i.TipoAtividade == tipoAtividade);

                if (atividade != null)
                {

                    if (atividade.TipoSituacaoAtividade == TipoSituacaoAtividadeEnum.Concluida)
                    {
                        tpSituacao = "Concluído";
                        classSpan = "label-primary";
                    }
                    else
                    {
                        classSpan = "label-warning";
                        tpSituacao = "Pendente";
                    }

                    dthConclusao = atividade.DthConcluida.FormatoDataHora();
                    shtml = string.Format("<span class=\"label {0}\">{1}</span><br />", classSpan, tpSituacao);
                    shtml += string.Format("<small class=\"text-nowrap\">{0}</small>", dthConclusao);

                }
            }

            return new HtmlString(shtml);
        }
    }
}
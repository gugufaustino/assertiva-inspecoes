using Differencial.Domain;
using Differencial.Domain.Annotation;
using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Enums.WorkFlow;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Web.Generico;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using Differencial.Domain.Contracts.Services;
using Microsoft.AspNetCore.Http;

namespace Differencial.Web.Helpers
{

    public static class HtmlWorkFlowHelper
    {
        private static IWorkFlowService _iworkFlowService()
        {
#pragma warning disable HttpContextCurrent // Type or member is obsolete
            return (IWorkFlowService)HttpContextHelper.Current.RequestServices.GetService(typeof(IWorkFlowService));
#pragma warning restore HttpContextCurrent // Type or member is obsolete
        }

        public static List<WFTipoAcao> AcoesDisponiveis(IWorkFlowInstanciaProcesso instanciaProcesso)
        {
            var workflow = _iworkFlowService();

            return workflow.AcoesDisponiveis(instanciaProcesso, UtilWeb.UsuarioLogado.Id, UtilWeb.UsuarioLogado.TipoPapel.Cast<int>().ToArray());
        }

        public static List<TipoSituacaoProcessoEnum> ProximoMovimento(IWorkFlowInstanciaProcesso instanciaProcesso, WFTipoAcao tipoAcao)
        {
            var workflow = _iworkFlowService();
            var lstRet = workflow.ProximoMovimento(instanciaProcesso.TpSituacao.Value, tipoAcao);
            return lstRet.Cast<TipoSituacaoProcessoEnum>().ToList();
        }

        public static List<TipoMotivoEnum> MotivoAcao(TipoSituacaoProcessoEnum tipoSituacaoProcessoEnum)
        {
            var workflow = _iworkFlowService();
            var lstRet = workflow.MotivoDisponiveis(tipoSituacaoProcessoEnum);
            return lstRet.ToList().Cast<TipoMotivoEnum>().ToList();
        }

        public static HtmlString FwDisplaySituacaoProcessoEnum(this TipoSituacaoProcessoEnum enumVal, bool shortDisplayName = false)
        {
            var metadata = enumVal.GetAttributeOfType<SituacaoProcessoAttribute>();
            var displayName = string.Empty;
            if (shortDisplayName)
                displayName = metadata.ShortName;
            else
                displayName = metadata.Name;
            return new HtmlString(displayName);
        }

        public static HtmlString FwDisplaySituacaoAcaoEnum(this TipoSituacaoProcessoEnum enumVal)
        {
            var metadata = enumVal.GetAttributeOfType<SituacaoProcessoAttribute>();
            var displayName = string.Empty;
            if (String.IsNullOrEmpty(metadata.NomeAcao))
                return new HtmlString(displayName);
            else
                return new HtmlString(metadata.NomeAcao);
        }
    }
}
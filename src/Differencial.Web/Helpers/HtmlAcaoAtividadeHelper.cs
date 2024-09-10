using Differencial.Domain;
using Differencial.Domain.Annotation;
using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Enums.WorkFlow;
using Differencial.Web.Generico;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Differencial.Web.Helpers
{
    public static class HtmlAtividadeHelper
    {
        #region Barra Atividade
        private class BarraDisposableContainer : IDisposable
        {
            private readonly TextWriter _writer;
            private readonly List<string> _lstHtmlDispose;

            public BarraDisposableContainer(TextWriter writer, List<string> lstHtmlDispose)
            {
                _writer = writer;
                _lstHtmlDispose = lstHtmlDispose;
            }
            public void Dispose()
            {
                foreach (var html in _lstHtmlDispose)
                {
                    _writer.Write(html);
                }
            }
        }


        public static IDisposable BarraAtividade(this IHtmlHelper htmlHelper)
        {
            var _htmlDispose = new List<string>();
            var writer = htmlHelper.ViewContext.Writer;

            writer.WriteLine("<div class=\" \">");
            writer.WriteLine("<div class=\"barra-atividade\" style=\"\">");
            writer.WriteLine("<div class=\"barra-btn-grupo pull-right\">");

            _htmlDispose.Add("</div>");
            _htmlDispose.Add("</div>");
            _htmlDispose.Add("</div>");
            return new BarraDisposableContainer(writer, _htmlDispose);
        }

        public static List<TipoAtividadeEnum> AtividadesDisponiveis(IWorkFlowInstanciaProcesso instanciaProcesso)
        {
#pragma warning disable HttpContextCurrent // Type or member is obsolete
            IWorkFlowService workflow =(IWorkFlowService)HttpContextHelper.Current.RequestServices.GetService(typeof(IWorkFlowService));
#pragma warning restore HttpContextCurrent // Type or member is obsolete

            return workflow.AtividadesDisponiveis(instanciaProcesso, UtilWeb.UsuarioLogado.Id, UtilWeb.UsuarioLogado.TipoPapel.Cast<int>().ToArray());
        }
        #endregion

        //Somente botões para pbarra de atividade dentro de cada aba
        public static HtmlString BotaoModalAtividade(string label, string id, bool indRederizar, BtnCorEnum btnTemaCor, IconeEnum btnIconeEnum, TamanhoEnum modalTamanho, string datahref)
        {
            var shtml = string.Empty;
            if (indRederizar)
            {
                shtml = string.Format(" <button type='button' class='btn {0} btn-sm' id='{1}' fw-click='modal' fw-modal='{2}' data-href='{3}'>", HtmlAcaoHelper.htmlBtnTemaEnum(btnTemaCor), id, HtmlAcaoHelper.htmlModalSizeEnum(modalTamanho), datahref);
                shtml += string.Format("{0} &nbsp;<span class='{1}'></span>", label, HtmlAcaoHelper.htmlBtnIconeEnum(btnIconeEnum));
                shtml += "</button> ";
            }
            return new HtmlString(shtml);
        }
        /// <summary>
        /// Esse botão não tem recusso de callback, ou seja, a interface não será atualizada em nada, apenas será apresentado a mensagem de retorno da contoller.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="id"></param>
        /// <param name="indRederizar"></param>
        /// <param name="mensagemConfirma"></param>
        /// <param name="datahref"></param>
        /// <returns></returns>
        public static HtmlString BotaoConcluirAtividade(string label, string id, bool indRederizar, string mensagemConfirma, string datahref)
        {
            var shtml = string.Empty;
            if (indRederizar)
            {
                if (mensagemConfirma.IndexOf("\"") > -1)
                    mensagemConfirma = mensagemConfirma.Replace("\"", "'");

                shtml = string.Format(" <button type='button' class='btn {0} btn-sm' id='{1}' fw-click='confirmaPost' data-href='{2}' data-val-mensagem=\"{3}\" >", HtmlAcaoHelper.htmlBtnTemaEnum(BtnCorEnum.success), id, datahref, mensagemConfirma);
                shtml += string.Format("{0} &nbsp;<span class='{1}'></span>", label, HtmlAcaoHelper.htmlBtnIconeEnum(IconeEnum.pencil));
                shtml += "</button> ";
            }
            return new HtmlString(shtml);
        }

    }

    public static class HtmlAcaoHelper
    {
        public static HtmlString BotaoModal(string label, string id, bool indRederizar, BtnCorEnum btnTema, string datahref, IconeEnum btnIcone = IconeEnum.SemIcone, TamanhoEnum modalTamanho = TamanhoEnum.ModalPadrao)
        {
            var shtml = string.Empty;
            if (indRederizar)
            {
                shtml = string.Format(" <button type='button' class='btn {0} btn-sm' id='{1}' fw-click='modal' fw-modal='{2}' data-href='{3}'>", htmlBtnTemaEnum(btnTema), id, htmlModalSizeEnum(modalTamanho), datahref);
                shtml += string.Format("{0} &nbsp;<span class='{1}'></span>", label, htmlBtnIconeEnum(btnIcone));
                shtml += "</button> ";
            }
            return new HtmlString(shtml);
        }

        public static HtmlString Botao(string label, string id, bool indRederizar, BtnCorEnum btnTemaCor, IconeEnum btnIconeEnum, BtnTamanhoEnum btnTamanho = BtnTamanhoEnum.Padrao)
        {
            var shtml = string.Empty;
            if (indRederizar)
            {
                shtml = string.Format(" <button type='button' class='btn {0} {2}' id='{1}' >", htmlBtnTemaEnum(btnTemaCor), id, htmlBtnTamanhoEnum(btnTamanho));
                shtml += string.Format("{0} &nbsp;<span class='{1}'></span>", label, htmlBtnIconeEnum(btnIconeEnum));
                shtml += "</button> ";
            }
            return new HtmlString(shtml);
        }
        


        public static HtmlString BotaoConfirm(string label, string id, bool indRederizar, string mensagemConfirma, string datahref, BtnCorEnum btnTemaCor, IconeEnum btnIconeEnum, BtnTamanhoEnum btnTamanho = BtnTamanhoEnum.Padrao)
        {
            var shtml = string.Empty;
            if (indRederizar)
            {
                if (mensagemConfirma.IndexOf("\"") > -1)
                    mensagemConfirma = mensagemConfirma.Replace("\"", "'");

                shtml = string.Format(" <button type='button' class='btn {0} {4}' id='{1}' fw-click='confirmaPost' data-href='{2}' data-val-mensagem=\"{3}\" >",
                    htmlBtnTemaEnum(btnTemaCor), id, datahref, mensagemConfirma, htmlBtnTamanhoEnum(btnTamanho));

                shtml += string.Format("{0} &nbsp;<span class='{1}'></span>", label, htmlBtnIconeEnum(btnIconeEnum));
                shtml += "</button> ";
            }
            return new HtmlString(shtml);
        }

        #region Metodos Auxiliares

        public static string htmlBtnTemaEnum(BtnCorEnum btnTema)
        {
            var sbtnTema = string.Empty;
            switch (btnTema)
            {
                case BtnCorEnum.Default:
                    sbtnTema = "btn-white";
                    break;
                case BtnCorEnum.primary:
                    sbtnTema = "btn-primary";
                    break;
                case BtnCorEnum.success:
                    sbtnTema = "btn-success";
                    break;
                case BtnCorEnum.white:
                    sbtnTema = "btn-white";
                    break;

            }
            return sbtnTema;
        }

        public static string htmlModalSizeEnum(TamanhoEnum modalSize)
        {
            var sModalSize = string.Empty;
            switch (modalSize)
            {
                case TamanhoEnum.ModalPequena:
                    sModalSize = "modal-sm";
                    break;
                case TamanhoEnum.ModalPadrao:
                    sModalSize = "";
                    break;
                case TamanhoEnum.ModalLarga:
                    sModalSize = "modal-lg";
                    break;

            }
            return sModalSize;
        }
        public static string htmlBtnTamanhoEnum(BtnTamanhoEnum modalSize)
        {
            var strRetorno = string.Empty;
            switch (modalSize)
            {
                case BtnTamanhoEnum.Pequena:
                    strRetorno = "btn-sm";
                    break;
                case BtnTamanhoEnum.Padrao:
                    strRetorno = "";
                    break;
                case BtnTamanhoEnum.Grande:
                    strRetorno = "btn-lg";
                    break;

            }
            return strRetorno;
        }
        public static string htmlBtnIconeEnum(IconeEnum btnIconeEnum)
        {
            var sRetorno = string.Empty;
            switch (btnIconeEnum)
            {
                case IconeEnum.pencil:
                    return "fa fa-pencil";

                case IconeEnum.check:
                    return "fa fa-check";

                case IconeEnum.floppySave:
                    return "fa fa-floppy-o";

                default:
                    return "";

            }

        }
        #endregion
    }

    public static class HtmlBotaoHelper
    {
        public static HtmlString Botao(string label, string id, bool indRederizar = true, BtnTamanhoEnum btnTamanho = BtnTamanhoEnum.Padrao, string btnClass = "", string spanClass = "")
        {
            var shtml = string.Empty;
            if (indRederizar)
            {
                shtml = string.Format(" <button type='button' class='btn {0} {1}' id='{2}' >", btnClass, HtmlAcaoHelper.htmlBtnTamanhoEnum(btnTamanho), id);
                shtml += string.Format("{0} &nbsp;<span class='{1}'></span>", label, spanClass);
                shtml += "</button> ";
            }
            return new HtmlString(shtml);
        }
      
    }
}
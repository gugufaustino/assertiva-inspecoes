using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Differencial.Web.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static string IsSelected(this IHtmlHelper html, string controller = null, string action = null, string cssClass = null)
        {

            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction ?
                cssClass : String.Empty;
        }
        public static string IsSelected(this IHtmlHelper html, string[] controller = null, string cssClass = null)
        {

            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (controller.Contains(currentController))
                return IsSelected(html, currentController, null, cssClass);
            else
                return String.Empty;
        }
        public static string PageClass(this IHtmlHelper html)
        {
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

        private class PageDisposableContainer : IDisposable
        {
            private readonly TextWriter _writer;
            private readonly List<string> _lstHtmlDispose;


            public PageDisposableContainer(TextWriter writer, List<string> lstHtmlDispose)
            {
                _writer = writer;
                _lstHtmlDispose = lstHtmlDispose;

            }

            public void Dispose()
            {
                foreach (var html in _lstHtmlDispose)
                    _writer.Write(html);
            }
        }

        #region Page Heading


        private static string PageHeading(this IHtmlHelper htmlHelper, string tituloLinkAtivoMigalha, string cssClassColMigalha, string cssClassColActionsBtns, params IHtmlContent[] lstActionLinksMigalha)
        {
            string strHtml;
            strHtml = PrintBeginPageHeading(htmlHelper, tituloLinkAtivoMigalha, cssClassColMigalha, cssClassColActionsBtns, lstActionLinksMigalha);
            strHtml += string.Concat(PrintEndPageHeading());
            return strHtml;
        }

        public static HtmlString PageHeading(this IHtmlHelper htmlHelper, string tituloLinkAtivoMigalha, params IHtmlContent[] lstActionLinksMigalha)
        {
            string cssClassColMigalha = "col-sm-6";
            string cssClassColActionsBtns = "col-sm-6";

            return new HtmlString(PageHeading(htmlHelper, tituloLinkAtivoMigalha, cssClassColMigalha, cssClassColActionsBtns, lstActionLinksMigalha));
        }


        public static IDisposable BeginPageHeading(this IHtmlHelper htmlHelper, string tituloLinkAtivoMigalha, params IHtmlContent[] lstActionLinksMigalha)
        {
            string cssClassColMigalha = "col-sm-6";
            string cssClassColActionsBtns = "col-sm-6";

            var writer = htmlHelper.ViewContext.Writer;
            writer.Write(PrintBeginPageHeading(htmlHelper, tituloLinkAtivoMigalha, cssClassColMigalha, cssClassColActionsBtns, lstActionLinksMigalha));

            // fechamento das div será no dispose 
            return new PageDisposableContainer(writer, PrintEndPageHeading());
        }

        private static string PrintBeginPageHeading(this IHtmlHelper htmlHelper, string tituloLinkAtivoMigalha, string cssClassColMigalha = "col-sm-4", string cssClassColActionsBtns = "col-sm-8", params IHtmlContent[] lstActionLinksMigalha)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine("<div class=\"row wrapper border-bottom white-bg page-heading\">");
            strBuilder.AppendFormat("  <div class=\"{0}\">", cssClassColMigalha);
            strBuilder.AppendFormat("     <h2>{0}</h2>", (htmlHelper.ViewBag.Title != null ? htmlHelper.ViewBag.Title : string.Empty));
            strBuilder.AppendFormat("     <ol class=\"breadcrumb\">");

            foreach (var actionlink in lstActionLinksMigalha)
            {
                var action = GetString(actionlink);
                
                strBuilder.AppendFormat($"<li>{action}</li>");
            }
            strBuilder.AppendFormat("     <li class=\"active\"> <strong>{0}</strong> </li>", tituloLinkAtivoMigalha);
            strBuilder.AppendFormat("     </ol>");
            strBuilder.AppendFormat("  </div>");
            strBuilder.AppendFormat("  <div class=\"{0}\" style=\"text-align:right\">", cssClassColActionsBtns);
            strBuilder.AppendFormat("      <div class=\"title-action\">");
            // fechamento das div será no dispose 
            return strBuilder.ToString();
        }

        public static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
            return writer.ToString();
        }

        private static List<string> PrintEndPageHeading()
        {
            var _lstHtmlDispose = new List<string>();
            _lstHtmlDispose.Add("      </div>");
            _lstHtmlDispose.Add("  </div>");
            _lstHtmlDispose.Add("</div>");
            return _lstHtmlDispose;
        }

        #endregion Page Heading


        public static HtmlString Tooltip(this IHtmlHelper helper, string descricao, string icone = "fa-question-circle")
        {
            var htmlInformation = string.Format("<i class=\"fa {1} text-muted\" data-toggle=\"tooltip\" data-placement=\"auto\" title=\"{0}\"></i>", descricao.Replace("\"","'"), icone);

            return new HtmlString(htmlInformation);
        }


    }
}

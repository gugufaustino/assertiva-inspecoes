using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Differencial.Web.Helpers
{
    public static class HtmlBoxHelper
    {
        private class BoxDisposableContainer : IDisposable
        {
            private readonly TextWriter _writer;
            private readonly List<string> _lstHtmlDispose;
            public BoxDisposableContainer(TextWriter writer, List<string> lstHtmlDispose)
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

        public static IDisposable Box(this IHtmlHelper htmlHelper, string titulo, string tituloMenor, bool? minimizavel = null, bool? maximizavel = null, bool? removivel = null, object htmlAttributes = null, List<HtmlString> lstActionLinksConfig = null, HtmlString actionsLeft = null)
        {

            var urlfullscreen = Controllers.BaseController.UrlFullScreen();


            var writer = htmlHelper.ViewContext.Writer;
            writer.WriteLine("<div class=\"ibox float-e-margins {0} \" >", (urlfullscreen.HasValue && urlfullscreen.Value == true) ? "fullscreen" : "");
            writer.WriteLine("<div class=\"ibox-title\" >");

            tituloMenor = String.IsNullOrEmpty(tituloMenor) ? string.Empty : "<small class=\"m-l-sm\">" + tituloMenor + "</small>";

            writer.WriteLine("<h5>{0}{1}</h5>", titulo, tituloMenor);
            writer.WriteLine("<div class=\"ibox-tools\">");
            if (lstActionLinksConfig != null && lstActionLinksConfig.Any())
            {
                writer.WriteLine("<a class=\"dropdown-toggle\" data-toggle=\"dropdown\" href=\"#\"><i class=\"fa fa-wrench\"></i></a>");
                writer.WriteLine("<ul class=\"dropdown-menu dropdown-user\">");

                lstActionLinksConfig.ForEach(delegate (HtmlString actionlink)
                {
                    writer.WriteLine("<li>{0}</li>", actionlink);
                });
                writer.WriteLine("</ul>");
            }
            if (minimizavel.HasValue && minimizavel.Value)
            {
                writer.WriteLine("<a class=\"collapse-link\"><i class=\"fa fa-chevron-up\"></i></a>");
            }
            if (maximizavel.HasValue && maximizavel.Value)
            {
                writer.WriteLine("<a class=\"fullscreen-link\"><i class=\"fa {0}\"></i></a>", (urlfullscreen.HasValue && urlfullscreen.Value == true) ? "fa-compress" : "fa-expand");
            }
            if (removivel.HasValue && removivel.Value)
            {
                writer.WriteLine("<a class=\"close-link\"><i class=\"fa fa-times\"></i></a>");
            }
            writer.Write("</div>");
            writer.Write("</div>");

            var _lstHtmlDispose = new List<string>();
            _lstHtmlDispose.Add("</div>");

            return new BoxDisposableContainer(writer, _lstHtmlDispose);
        }

        public static IDisposable BeginBox(this IHtmlHelper htmlHelper, string titulo = "", string tituloMenor = "", bool? minimizavel = false, bool? maximizavel = true, bool? removivel = false, object htmlAttributes = null, IHtmlContent[] actionsLeft = null, HtmlString[] actionsRight = null, params HtmlString[] lstActionLinksConfig)
        {

            var urlfullscreen = Controllers.BaseController.UrlFullScreen();


            var writer = htmlHelper.ViewContext.Writer;
            writer.WriteLine("<div class=\"ibox float-e-margins {0} \" >", (urlfullscreen.HasValue && urlfullscreen.Value == true) ? "fullscreen" : "");
            writer.WriteLine("<div class=\"ibox-title\" >");

            #region "pull-left"
            writer.WriteLine("<div class=\"pull-left\" >");
            #region "h5"
            tituloMenor = String.IsNullOrEmpty(tituloMenor) ? string.Empty : "<small class=\"m-l-sm\">" + tituloMenor + "</small>";
            writer.WriteLine("<h5 style='margin-top: 8px; margin-right:10px; margin-left: 5px; '>{0}{1}</h5>", titulo, tituloMenor);
            if (actionsLeft != null && actionsLeft.Any())
                foreach (var item in actionsLeft)
                    writer.WriteLine(item);
            #endregion

            writer.Write("</div>");
            #endregion

            #region "pull-right"
            writer.WriteLine("<div class=\"pull-right\" >");

            if (actionsRight != null && actionsRight.Any())
                foreach (var item in actionsRight)
                    writer.WriteLine(item);

            #region "div-tools"
            writer.WriteLine("<div style=\"float: unset; display:inline-block\"  class=\"ibox-tools\">");

            if (lstActionLinksConfig != null && lstActionLinksConfig.Any())
            {
                writer.WriteLine("<a class=\"dropdown-toggle\" data-toggle=\"dropdown\" href=\"#\"><i class=\"fa fa-wrench\"></i></a>");
                writer.WriteLine("<ul class=\"dropdown-menu dropdown-user\">");

                lstActionLinksConfig.ToList().ForEach(delegate (HtmlString actionlink)
                {
                    writer.WriteLine("<li>{0}</li>", actionlink);
                });
                writer.WriteLine("</ul>");
            }
            if (minimizavel.HasValue && minimizavel.Value)
            {
                writer.WriteLine("<a class=\"collapse-link\"><i class=\"fa fa-chevron-up\"></i></a>");
            }
            if (maximizavel.HasValue && maximizavel.Value)
            {
                writer.WriteLine("<a class=\"fullscreen-link\"><i class=\"fa {0}\"></i></a>", (urlfullscreen.HasValue && urlfullscreen.Value == true) ? "fa-compress" : "fa-expand");
            }
            if (removivel.HasValue && removivel.Value)
            {
                writer.WriteLine("<a class=\"close-link\"><i class=\"fa fa-times\"></i></a>");
            }
            writer.Write("</div>");
            #endregion

            writer.Write("</div>");
            #endregion
            

         

            writer.Write("</div>");
            writer.Write("<div class=\"ibox-content\">");

            var _lstHtmlDispose = new List<string>();
            _lstHtmlDispose.Add("</div>");
            _lstHtmlDispose.Add("</div>");

            return new BoxDisposableContainer(writer, _lstHtmlDispose);
        }


    }
}

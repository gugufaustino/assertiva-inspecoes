using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Differencial.Web.Helpers
{
    public class Tab
    {
        public Tab(string identificacao, string tituloAba, bool indAtiva = false, bool indRendereizar = true)
        {
            IndAtiva = indAtiva;
            IndRederizar = indRendereizar;
            TituloAba = tituloAba;
            Identificacao = identificacao;
        }

        /// <summary>
        /// Indica se a aba deve ser renderizada como html
        /// </summary>
        public bool IndRederizar { get; set; }

        /// <summary>
        /// Indica se a aba está selecionada
        /// </summary>
        public bool IndAtiva { get; set; }

        /// <summary>
        /// Titulo que aparece na aba
        /// </summary>
        public string TituloAba { get; set; }

        /// <summary>
        /// Href e Id usado para seleção quando clicado;
        /// </summary>
        public string Identificacao { get; set; }

        public HtmlString Partial { get; set; }



    }

    public enum TabsOrientacaoEnum
    {
        Top = 0,
        Left = 1
    }
    public static class HtmlTabsHelper
    {

        private class TabsDisposableContainer : IDisposable
        {
            private readonly TextWriter _writer;
            private readonly List<string> _lstHtmlDispose;

            public TabsDisposableContainer(TextWriter writer, List<string> lstHtmlDispose)
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


        public static IDisposable TabPane(this IHtmlHelper htmlHelper, Tab tab)
        {
            var _htmlDispose = new List<string>();
            var writer = htmlHelper.ViewContext.Writer;
            writer.WriteLine("<div class=\"tab-pane {0}\" id=\"{1}\">", tab.IndAtiva ? "active" : string.Empty, tab.Identificacao);
            writer.WriteLine("<div class=\"panel-body\" >");

            if (tab.IndRederizar == false)
            {
                writer.WriteLine("<!-- ");
                _htmlDispose.Add(" -->");
            }


            _htmlDispose.Add("</div>");
            _htmlDispose.Add("</div>");
            return new TabsDisposableContainer(writer, _htmlDispose);
        }


        public static IDisposable TabContainer(this IHtmlHelper htmlHelper, TabsOrientacaoEnum tabsOrientacao ,  params Tab[] tabs)
        {
            var writer = htmlHelper.ViewContext.Writer;
            writer.WriteLine("<div class=\"tabs-container\" >");
            if(tabsOrientacao == TabsOrientacaoEnum.Left)
                writer.WriteLine("<div class=\"tabs-left\" >");

            writer.WriteLine("<ul class=\"nav nav-tabs\" >");

            if (tabs.All(i => i.IndAtiva == false))           
                tabs.First().IndAtiva = true;             

            tabs.Where(t => t.IndRederizar).ToList().ForEach(delegate (Tab tab)
            {
                writer.WriteLine("<li class=\"{0}\">", tab.IndAtiva ? "active" : string.Empty);
                writer.WriteLine("<a data-toggle=\"tab\" href=\"#{0}\">{1}</a>", tab.Identificacao, tab.TituloAba);
                writer.WriteLine("</li>");
            });
            writer.WriteLine("</ul>");


            writer.WriteLine("  <div class=\"tab-content\" >");

            var _htmlDispose = new List<string>();
            _htmlDispose.Add("   </div>");

            if (tabsOrientacao == TabsOrientacaoEnum.Left)
                _htmlDispose.Add("</div>");

            _htmlDispose.Add("</div>");
            return new TabsDisposableContainer(writer, _htmlDispose);
        }

    }
}

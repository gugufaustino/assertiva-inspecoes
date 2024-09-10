using System.Collections;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace Differencial.Web.Filters
{
    public class GenericoVoltarFilter : ActionFilterAttribute, IActionFilter
    {
        private Stack stack;
        public bool _tirarDaPilha { get; set; }

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_tirarDaPilha)
            {
                var stackExistente = (Stack)filterContext.HttpContext.Session.Contents["Voltar"];

                stackExistente.Pop();
                var clone = (Stack)stackExistente.Clone();

                if (stackExistente.Count > 0)
                    stackExistente.Pop();

                filterContext.HttpContext.Session.Add("Voltar", stackExistente);
                filterContext.HttpContext.Session.Add("VoltarAux", clone); 

            }
            else
            {
                var url = HttpContextHelper.Current.Request.RawUrl;
                var stackExistente = filterContext.HttpContext.Session.Contents["Voltar"];

                var urlVoltar = "";

                if (stackExistente == null)
                {
                    this.stack = new Stack();
                    urlVoltar = url;
                }
                else
                {
                    this.stack = (Stack)stackExistente;
                    if (filterContext.HttpContext.Session["_hash_pagina_"] != null)
                    {
                        string hashPagina = filterContext.HttpContext.Session["_hash_pagina_"].ToString();
                        if (!string.IsNullOrWhiteSpace(hashPagina) &&
                            stack.Count > 0 && filterContext.HttpContext.Request.UrlReferrer != null)
                        {
                            var urlStack = (string) stack.Pop();
                            //Checa se estamos indo para uma pagina 
                            if (urlStack.Split('#')[0] ==
                                filterContext.HttpContext.Request.UrlReferrer.AbsolutePath +
                                filterContext.HttpContext.Request.UrlReferrer.Query)
                                urlStack += hashPagina;
                            stack.Push(urlStack);

                            filterContext.HttpContext.Session["_hash_pagina_"] = "";
                        }
                        urlVoltar = stack.Count > 0 ? stack.Peek().ToString() : "";
                    }
                }

                if (!(stackExistente != null && urlVoltar == url))
                    this.stack.Push(url);

                filterContext.HttpContext.Session.Add("Voltar", this.stack);
            }
        }
    }
}
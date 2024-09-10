using Differencial.Domain.Exceptions;
using Differencial.Infra;
using Differencial.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;

namespace Differencial.Web.Filters
{
    public class ValidationExceptionFilter : ExceptionFilterAttribute, IExceptionFilter, IActionFilter
    {
        private Microsoft.AspNetCore.Mvc.Controller mycontroller;

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
#pragma warning disable UA0002 // Types should be upgraded
            mycontroller = (Controller)context.Controller;
#pragma warning restore UA0002 // Types should be upgraded
        }

        public override void OnException(ExceptionContext filterContext)
        {

            base.OnException(filterContext);

            var baseController = new Controllers.BaseController();
            
            ILog logger = filterContext.HttpContext.RequestServices.GetService<ILog>();

            var validationResult = new ValidationExceptionResult(logger, baseController.Url);
            Exception exception = filterContext.Exception;

            if (exception.GetType() == typeof(ValidationException))
            {
                if (Controllers.HttpRequestExtensions.IsAjaxRequest(filterContext.HttpContext.Request))
                {
                    filterContext.ExceptionHandled = true;
                    filterContext.Result = baseController.Json(validationResult.ResponseResultException(exception));

                }
                else
                {
                    if (filterContext.Result is ViewResult)
                    {
                        ((ViewResult)filterContext.Result).ViewData["ResponseResult"] = validationResult.ResponseResultException(exception);

                    }
                    else
                    {
                        var request = filterContext.HttpContext.Request;
                        // Se pagina é a mesma

                        var UrlReferrer = request.GetTypedHeaders().Referer;

                        //var uri = new Uri(request.QueryString.ToUriComponent());

                        //if (UrlReferrer == null || (UrlReferrer != null && UrlReferrer.GetLeftPart(UriPartial.Path) == uri.GetLeftPart(UriPartial.Path)))
                        //{
                        //    filterContext.Result = baseController.View();
                        //}
                        //else
                        {
                            string controller = filterContext.RouteData.Values["controller"].ToString();
                            string action = filterContext.RouteData.Values["action"].ToString();
                            string id = filterContext.RouteData.Values.ContainsKey("id") ? filterContext.RouteData.Values["id"].ToString() : string.Empty;



                            var url = UrlReferrer;

                            var strControllerActionUrl = url.AbsolutePath.Split('/');
                            var route = new Microsoft.AspNetCore.Routing.RouteValueDictionary();


                            if (!String.IsNullOrEmpty(id))
                                route.Add("id", id);

                            if (!String.IsNullOrEmpty(url.Query))
                            {
                                var query = url.Query.Substring(1).Split('&');
                                foreach (var param in query)
                                {
                                    route.Add(param.Split('=')[0], param.Substring(param.IndexOf("=") + 1));
                                }
                            }


                            filterContext.Result = baseController.RedirectToAction(strControllerActionUrl[2], strControllerActionUrl[1], route);
                            ((Differencial.Web.Controllers.BaseController)mycontroller).TempData["ResponseResult"] = JsonSerializer.Serialize(validationResult.ResponseResultException(exception));

                        }
                    }
                }
            }

        }
    }
}
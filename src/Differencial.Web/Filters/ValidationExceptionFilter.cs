using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Exceptions;
using Differencial.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;
using System;
using System.Text.Json;

namespace Differencial.Web.Filters
{
    public class ValidationExceptionFilter : ExceptionFilterAttribute, IExceptionFilter, IActionFilter
    {
        private Microsoft.AspNetCore.Mvc.Controller mycontroller;
        private IActionResult r;
        private IActionResult r2;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            r2 = context.Result;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
#pragma warning disable UA0002 // Types should be upgraded
            mycontroller = (Controller)filterContext.Controller;
#pragma warning restore UA0002 // Types should be upgraded
              r = filterContext.Result;
         

        }

        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            var baseController = new Controllers.BaseController();
            ILog logger = filterContext.HttpContext.RequestServices.GetService<ILog>();
            var validationResult = new ValidationExceptionResult(logger, baseController.Url);
            Exception exception = filterContext.Exception;

            if (exception is ValidationException)
            {
                filterContext.ExceptionHandled = true;
                // Se for uma requisição Ajax
                if (Controllers.HttpRequestExtensions.IsAjaxRequest(filterContext.HttpContext.Request))
                {

                    filterContext.Result = baseController.Json(validationResult.ResponseResultException(exception));
                }
                else
                {
                    if (filterContext.Result is ViewResult viewResult) // quando já houve return na action mas deu erro na view;
                    {
                        // Preserve o Model atual
                        var model = viewResult.Model;

                        // Retorne a mesma view com a model original
                        baseController.ViewData.ModelState.Merge(filterContext.ModelState);
                        filterContext.Result = baseController.View(viewResult.ViewName, model);
                        // Adicione a mensagem de erro ao ViewData para exibir na View
                        viewResult.ViewData["ResponseResult"] = validationResult.ResponseResultException(exception);
                    }
                    else if (filterContext.Result is null)
                    {

                        baseController.ViewData.ModelState.Merge(filterContext.ModelState);

                        var actionName = filterContext.RouteData.Values["action"].ToString();
                        //var controllerName = filterContext.RouteData.Values["controller"].ToString();

                        filterContext.Result = baseController.View($"{actionName}");
                        ((ViewResult)filterContext.Result).ViewData["ResponseResult"] = validationResult.ResponseResultException(exception);
                    }
                    else
                        throw new NotImplementedException("Fluxo de validação não tratado;");
                }
            }
        }

    }
}
using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Exceptions;
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

			if (exception is ValidationException)
			{
				// Se for uma requisição Ajax
				if (Controllers.HttpRequestExtensions.IsAjaxRequest(filterContext.HttpContext.Request))
				{
					filterContext.ExceptionHandled = true;
					filterContext.Result = baseController.Json(validationResult.ResponseResultException(exception));

				}
				else
				{
					// Verifica se a Result atual é uma ViewResult
					if (filterContext.Result is ViewResult viewResult)
					{
						viewResult.ViewData["ResponseResult"] = validationResult.ResponseResultException(exception);

					}
					else
					{
						var request = filterContext.HttpContext.Request;
						var refererUrl = request.GetTypedHeaders().Referer;

						if (refererUrl != null && refererUrl.AbsolutePath.Split('/').Length >=3)
						{
							string controller = filterContext.RouteData.Values["controller"].ToString();
							string action = filterContext.RouteData.Values["action"].ToString();
							string id = filterContext.RouteData.Values.ContainsKey("id") ? filterContext.RouteData.Values["id"].ToString() : string.Empty;


							var strControllerActionUrl = refererUrl.AbsolutePath.Split('/');
							var route = new Microsoft.AspNetCore.Routing.RouteValueDictionary();

							if (!string.IsNullOrEmpty(id))
								route.Add("id", id);

							// Adiciona os parâmetros da QueryString, se houver
							if (!string.IsNullOrEmpty(refererUrl.Query))
							{
								var queryParams = refererUrl.Query.Substring(1).Split('&');
								foreach (var param in queryParams)
								{
									var keyValue = param.Split('=');
									route.Add(keyValue[0], keyValue[1]);
								}
							}

							// Redireciona para a página anterior
							
							filterContext.Result = baseController.RedirectToAction(strControllerActionUrl[2], strControllerActionUrl[1], route);
							((Differencial.Web.Controllers.BaseController)mycontroller).TempData["ResponseResult"] = JsonSerializer.Serialize(validationResult.ResponseResultException(exception));
						}
						else
						{   // Caso não tenha Referer, retorna para uma View padrão

							filterContext.Result = baseController.View();
							((Differencial.Web.Controllers.BaseController)mycontroller).TempData["ResponseResult"] = JsonSerializer.Serialize(validationResult.ResponseResultException(exception));
						
						}
					}
				}

			}
		}
	}
}
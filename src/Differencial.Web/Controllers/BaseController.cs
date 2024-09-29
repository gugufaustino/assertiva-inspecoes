using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Exceptions;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Web.DTO;
using Differencial.Web.Filters;
using Differencial.Web.Generico;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Differencial.Web.Controllers
{
	[ValidationExceptionFilter]
	public class BaseController : Controller
	{

		protected ILog Log => this.HttpContext.RequestServices.GetService(typeof(ILog)) as ILog;




		#region Methods that Save Operations

		protected void AppSaveChanges()
		{
			this.UnitOfWorkSaveChanges(UtilWeb.UsuarioLogado.Id);
		}

		protected void AppSaveChanges(int IdOperador)
		{
			this.UnitOfWorkSaveChanges(IdOperador);
		}

		private void UnitOfWorkSaveChanges(int usuarioaplicacao)
		{
			Domain.UOW.IUnitOfWork unitOfWork = (Domain.UOW.IUnitOfWork)HttpContext.RequestServices.GetService(typeof(Domain.UOW.IUnitOfWork));

			unitOfWork.AppSaveChanges(usuarioaplicacao);

		}


		protected async Task AppSaveChangesAsync( )
		{
			await this.UnitOfWorkSaveChangesAsync(UtilWeb.UsuarioLogado.Id);
		}

		private async Task UnitOfWorkSaveChangesAsync(int usuarioaplicacao)
		{
			Domain.UOW.IUnitOfWork unitOfWork = (Domain.UOW.IUnitOfWork)HttpContext.RequestServices.GetService(typeof(Domain.UOW.IUnitOfWork));

			await unitOfWork.AppSaveChangesAsync(usuarioaplicacao);

		}

		#endregion


		/// <summary>
		/// Cria retorno padrão para o request feito pela view
		/// </summary>
		/// <param name="success">True or False</param>
		/// <param name="content">Conteúdo de retorno (caso se aplique)</param>
		/// <param name="url">Url de retorno (caso se aplique)</param>
		/// <param name="message">Mensagem de erro (caso se aplique)</param>        
		/// 
		/// <returns>JSON de resposta</returns>
		protected ContentResult ResponseResultObject(bool success, object content = null, string url = null, string message = null)
		{
			return JsonObject(CreateResponseDTO(success, content, url, message));
		}


		protected JsonResult ResponseResult(bool success, object content = null, string url = null, string message = null)
		{
			return base.Json(CreateResponseDTO(success, content, url, message));
		}
		private static ResponseResultDTO CreateResponseDTO(bool success, object content, string url, string message)
		{
			return new ResponseResultDTO()
			{
				TipoResponseResult = success ? TipoResponseResultEnum.Sucesso : TipoResponseResultEnum.Erro,
				success = success,
				url = url,
				content = content,
				showMessage = !message.IsNullOrEmpty(),
				message = !message.IsNullOrEmpty() ? message : string.Empty
			};
		}

		/// <summary>
		/// Cria retorno para ser usado quando é necessário o retorno de muitos dados na tela, 
		/// por exemplo, conteudo de arquivos em base64
		/// </summary>
		/// <param name="success">True or False</param>
		/// <param name="showMessage">Deve exibir a mensagem ao retornar para a tela</param>
		/// <param name="content">Conteúdo de retorno (caso se aplique)</param>
		/// <param name="url">Url de retorno (caso se aplique)</param>        
		/// <param name="message">Mensagem de erro (caso se aplique)</param>
		/// <returns>JSON de resposta</returns>
		protected JsonResult ResponseResultRetornaConteudoArquivo(bool success, bool showMessage = true, object content = null, string url = null, string messageError = null)
		{
			JsonResult jsonResult = base.Json(new ResponseResultDTO()
			{
				TipoResponseResult = success ? TipoResponseResultEnum.Sucesso : TipoResponseResultEnum.Erro,
				success = success,
				url = url,
				content = content,
				showMessage = showMessage,
				message = (success && showMessage && messageError.IsNullOrEmpty())
						? Domain.Resources.MensagensSucesso.SucessoSalvar
						: messageError
			});

			//jsonResult.MaxJsonLength = int.MaxValue;

			return jsonResult;
		}



		#region "Sobreescrita do JsonResult.Json"

		//TODO: METODO PRECISA SER REFATORADO/RE IMPLEMENTADO
		/// <summary>
		/// Foi implementado no ResultJson uma forma de tratar o globalization de campos como Data, Decimal e Double. Por padrão o Json retornava como CultureInfo.Invariant.
		/// Agora é possivel cutomizar esse retorno como CultureInfo pt-BR, fazendo com que o json fique no mesmo formato que os ModelBinder do asp.net mvc.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		protected new JsonResult Json(object data)
		{
			return base.Json(data);
		}
		protected ContentResult JsonObject(object data)
		{
			var options = new JsonSerializerOptions()
			{
				PropertyNameCaseInsensitive = true,
			};
			options.Converters.Add(new DecimalJsonConverter());
			options.Converters.Add(new DoubleJsonConverter());

			var result = new ContentResult
			{
				ContentType = "application/json",
				Content = JsonSerializer.Serialize(data, options)
			};

			return result;

		}


		#endregion "Sobreescrita do JsonResult.Json"




		/// <summary>
		/// Metodo captura erro na controller e monta o retorno:
		/// Em caso de requisição AJAX monta JsonResult com informações sobre o erro;
		/// Em caso de POST NORMAL retorna para a mesma pagina com erro no (TempData["ResponseResult"]
		/// </summary>
		/// <param name="filterContext"></param>
		private void OnException(ExceptionContext filterContext)
		{
			//Tratamento generico para exceções, para não precisar fazer try catch na controller;
			//TODO Ver melhor implementação no FW 

			Exception exception = filterContext.Exception;
			filterContext.ExceptionHandled = true;



			if (filterContext.HttpContext.Request.IsAjaxRequest())
			{
				// filterContext.Result = base.Json(ResponseResultException(exception));
			}
			else
			{

				if (exception.GetType() == typeof(ExcecaoSessaoExpirada))
				{
					filterContext.Result = Redirect("Home/Login");
					return;
				}
				var request = filterContext.HttpContext.Request;
				// Se pagina é a mesma

				var UrlReferrer = request.GetTypedHeaders().Referer;

				var uri = new Uri(request.QueryString.ToUriComponent());

				if (UrlReferrer == null || (UrlReferrer != null && UrlReferrer.GetLeftPart(UriPartial.Path) == uri.GetLeftPart(UriPartial.Path)))
				{
					filterContext.Result = View();
				}
				else
				{
					string controller = filterContext.RouteData.Values["controller"].ToString();
					string action = filterContext.RouteData.Values["action"].ToString();
					string id = filterContext.RouteData.Values.ContainsKey("id") ? filterContext.RouteData.Values["id"].ToString() : string.Empty;



					var url = UrlReferrer;

					var strControllerActionUrl = url.AbsolutePath.Split('/');
					var route = new RouteValueDictionary();


					if (!id.IsNullOrEmpty())
						route.Add("id", id);

					if (!url.Query.IsNullOrEmpty())
					{
						var query = url.Query.Substring(1).Split('&');
						foreach (var param in query)
						{
							route.Add(param.Split('=')[0], param.Substring(param.IndexOf("=") + 1));
						}
					}


					filterContext.Result = RedirectToAction(strControllerActionUrl[2], strControllerActionUrl[1], route);
				}

				//  base.TempData["ResponseResult"] = ResponseResultException(exception);
			}
		}



		//protected override void OnAuthorization(AuthorizationContext filterContext)
		//{
		//    //base.OnAuthorization(filterContext);
		//    var authorize = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AuthorizeAttribute), true);
		//    if (authorize.Count() > 0)
		//    {
		//        var allowAnonymous = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true);

		//        if (authorize.Count() > 0 && allowAnonymous.Count() > 0)
		//            return;

		//        var autenticado = UtilWeb.UsuarioLogado.Autenticado;
		//    }
		//    base.OnAuthorization(filterContext);
		//}

		//protected virtual bool AuthorizeCore(System.Web.HttpContextBase httpContext)
		//{
		//    if (httpContext == null)
		//    {
		//        throw new ArgumentNullException("httpContext");
		//    }

		//    System.Security.Principal.IPrincipal user = httpContext.User;
		//    if (!user.Identity.IsAuthenticated)
		//    {
		//        return false;
		//    }

		//    return true;
		//}


		protected ActionResult RetornoSalvar(RetornoSalvarEnum retornosalvar, int Id = 0, string mensagemSucesso = "")
		{
			RouteValueDictionary htmlRouteValues = new RouteValueDictionary();
			var controller = this.ControllerContext.ActionDescriptor.ControllerName;

			ViewData["ResponseResult"] = new ResponseResultDTO()
			{
				showMessage = true,
				TipoResponseResult = TipoResponseResultEnum.Sucesso,
				message = String.IsNullOrEmpty(mensagemSucesso) ? Domain.Resources.MensagensSucesso.SucessoSalvar : mensagemSucesso
			};



			var fullscreen = UrlFullScreen().HasValue ? UrlFullScreen().ToString().ToLower() : "";
			if (retornosalvar != RetornoSalvarEnum.Listar && UrlFullScreen().HasValue)
				htmlRouteValues.Add("fullscreen", fullscreen);

			switch (retornosalvar)
			{
				case RetornoSalvarEnum.Listar:
					return RedirectToAction("Listar", htmlRouteValues);

				case RetornoSalvarEnum.Editar:

					htmlRouteValues.Add("Id", Id);
					return RedirectToAction("Editar", htmlRouteValues);
				case RetornoSalvarEnum.Inserir:

					htmlRouteValues.Add("Id", "");
					return RedirectToAction("Editar", controller, htmlRouteValues);
				default:
					throw new NotImplementedException();
			}
		}

		/// <summary>
		/// Metodo captura a ValidationException e suprime a exceção ou lança como Alerta de Infromação para a interface;
		/// </summary>
		/// <param name="action"></param>
		/// <param name="showAsIformation"></param>
		/// <returns>Retorna true se houve total processamento ou false se capturou alguma ValidationException</returns>
		protected bool TryValidation(Action action, bool showAsIformation = false)
		{
			try
			{
				action();
				return true;
			}
			catch (ValidationException validEx)
			{
				if (showAsIformation)
				{
					var message = string.Empty;
					var exception = validEx;
					foreach (var item in exception.ValidationResults)
					{
						message += Environment.NewLine + item.ErrorMessage;
					}
					TempData["ResponseResult"] = new ResponseResultDTO()
					{
						showMessage = true,
						TipoResponseResult = TipoResponseResultEnum.Informacao,
						message = exception.Message + message
					};
				}

				return false;
			}
		}


		public static bool? UrlFullScreen()
		{
			var request = new HttpContextAccessor().HttpContext.Request;
			var query = "";
			if (request.Method == "GET")
				query = request.QueryString.ToUriComponent();
			else
				query = request.GetTypedHeaders().Referer.Query;

			if (query.Contains("fullscreen=true"))
				return true;
			else if (query.Contains("fullscreen=false"))
				return false;


			return null;
		}


		protected List<string> GetModelValidation(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
		{
			List<string> errorsReturn = new List<string>();
			if (!modelState.IsValid)
			{
				var errors = modelState.Values.SelectMany(x => x.Errors);
				foreach (var error in errors)
				{
					errorsReturn.Add(error.Exception == null ? error.ErrorMessage : error.Exception.Message);
				}
			}
			return errorsReturn;
		}

		protected void CatchValidationExceptionToViewData(ValidationException vEx)
		{
			ViewData["ResponseResult"] = new ValidationExceptionResult(this.Log, this.Url).ResponseResultException(vEx);
		}

	}


	public static class HttpRequestExtensions
	{
		private const string RequestedWithHeader = "X-Requested-With";
		private const string XmlHttpRequest = "XMLHttpRequest";

		public static bool IsAjaxRequest(this HttpRequest request)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}

			if (request.Headers != null)
			{
				return request.Headers[RequestedWithHeader] == XmlHttpRequest;
			}

			return false;
		}
	}


	public class DecimalJsonConverter : JsonConverter<Decimal>
	{
		public override Decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
				Decimal.Parse(reader.GetString()!, System.Globalization.CultureInfo.InvariantCulture);

		public override void Write(Utf8JsonWriter writer, Decimal dateTimeValue, JsonSerializerOptions options) =>
				writer.WriteStringValue(dateTimeValue.ToString(System.Globalization.CultureInfo.CurrentCulture));
	}

	public class DoubleJsonConverter : JsonConverter<Double>
	{
		public override Double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
				Double.Parse(reader.GetString()!, System.Globalization.CultureInfo.InvariantCulture);

		public override void Write(Utf8JsonWriter writer, Double dateTimeValue, JsonSerializerOptions options) =>
				writer.WriteStringValue(dateTimeValue.ToString(System.Globalization.CultureInfo.CurrentCulture));
	}



}
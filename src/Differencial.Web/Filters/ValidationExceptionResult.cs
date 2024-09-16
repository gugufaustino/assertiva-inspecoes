using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Exceptions;
using Differencial.Web.DTO;
using Differencial.Web.Generico;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Differencial.Web.Controllers
{
    public class ValidationExceptionResult
    {
        private readonly ILog Log;
        private readonly IUrlHelper Url;

        public ValidationExceptionResult(ILog log, IUrlHelper url)
        {
            Log = log;
            Url = url;
        }

        public ResponseResultDTO ResponseResultException(Exception ex)
        {
            var responseResultDTO = new ResponseResultDTO()
            {
                message = ex.Message + Environment.NewLine + "", //ex.StackTrace,
                showMessage = true,
                success = false,
            };

            if (ex.GetType() == typeof(ValidationException)) // bloco comentado por que as Exceptions são pegas manuais no ation-post e o model-state-valid é pego pelo validation-sumary
            {
                responseResultDTO.TipoResponseResult = TipoResponseResultEnum.Atencao;
                responseResultDTO.title = "Validação";
                //responseResultDTO.message = string.Empty;
                //var exception = (Domain.Exceptions.ValidationException)ex;

                //foreach (var item in exception.ValidationResults)
                //{
                //    responseResultDTO.message += Environment.NewLine + item.ErrorMessage;
                //}

                Log.Registrar(ex, responseResultDTO.message, TipoLogEnum.RegraNegocio);

            }
            else if (ex.GetType() == typeof(ExcecaoSessaoExpirada))
            {
                responseResultDTO.TipoResponseResult = TipoResponseResultEnum.Erro;
                responseResultDTO.title = "Erro";
                responseResultDTO.url =  Url.Action("Login", "Home");

                Log.Registrar(ex, TipoLogEnum.Erro);
            }
            else
            {
                responseResultDTO.TipoResponseResult = TipoResponseResultEnum.Erro;
                responseResultDTO.title = "Erro";
                // responseResultDTO.message = ex.ToString();

                Log.Registrar(ex, TipoLogEnum.Erro);
            }
            return responseResultDTO;
        }
    }



}
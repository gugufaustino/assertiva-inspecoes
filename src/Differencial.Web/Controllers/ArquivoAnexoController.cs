using System.Linq;
using Differencial.Web.Controllers;
using Differencial.Domain.Contracts.Services;
using Differencial.Service.Services;
using Differencial.Domain.DTO;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Web.DTO;
using Differencial.Web.Filters;
using System;
using System.IO;
using Differencial.Web.Generico;
using Differencial.Domain;
using Differencial.Web.Models;
using System.Collections.Generic;
using System.IO.Compression;
using Microsoft.AspNetCore.Mvc;

namespace WEB.Controllers
{
    public class ArquivoAnexoController : BaseController
    {
        IArquivoAnexoService _arquivoAnexoService;
        ILaudoFotoService _laudoFotoService;
        IDocXService _wordService;
        public ArquivoAnexoController(IArquivoAnexoService fotoService,
             ILaudoFotoService laudoFotoService,
             IDocXService wordService)
        {
            _arquivoAnexoService = fotoService;
            _laudoFotoService = laudoFotoService;
            _wordService = wordService;
        }

        public FileStreamResult BaixarLaudoAnalise(int Id)
        {

            var lstArquivo = _arquivoAnexoService.ListarArquivoSolicitacao(Id, TipoArquivoAnexoEnum.LaudoAnalise);

            if (lstArquivo.Any())
            {
                var arquivo = lstArquivo.First();
                FileStreamResult fileResult;
                var memory = _arquivoAnexoService.BaixarArquivoAnexo(arquivo);
                fileResult = new FileStreamResult(memory, "application/zip");
                fileResult.FileDownloadName = arquivo.ArquivoNome + arquivo.ArquivoExtencao;

                return fileResult;
            }
            return null;
        }

        #region Metodos Auxiliares


        #endregion Metodos Auxiliares
    }

}





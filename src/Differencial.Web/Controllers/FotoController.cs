using Differencial.Domain;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Contracts.Util;
using Differencial.Domain.Entities;
using Differencial.Domain.UOW;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Web.Controllers;
using Differencial.Web.Filters;
using Differencial.Web.Generico;
using Differencial.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WEB.Controllers
{
    public class FotoController : BaseController
    {
        private readonly IConfiguracaoAplicativo _configuracaoAplicativo;
        readonly IArquivoAnexoService _arquivoAnexoService;
        readonly ILaudoFotoService _laudoFotoService;
        readonly IDocXService _wordService;
        public FotoController(IArquivoAnexoService fotoService, IConfiguracaoAplicativo configuracaoAplicativo,
             ILaudoFotoService laudoFotoService,
             IDocXService wordService)
        {
            _configuracaoAplicativo = configuracaoAplicativo;
            _arquivoAnexoService = fotoService;
            _laudoFotoService = laudoFotoService;
            _wordService = wordService;
        }

        #region Inspecao
        [HttpPost]        
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult Enviar(IFormFile arquivo, int idSolicitacao, DateTime dataModificacao, int index)
        {


            var laudoFoto = new LaudoFoto { QuadroFotosPosicao = index };
            _laudoFotoService.Salvar(laudoFoto);

            var foto = new ArquivoAnexo()
            {
                IdSolicitacao = idSolicitacao,
                ArquivoDataModificacao = dataModificacao,
                ArquivoNome = Path.GetFileNameWithoutExtension(arquivo.FileName),
                ArquivoExtencao = Path.GetExtension(arquivo.FileName),
                ArquivoTamanhoBytes = arquivo.Length,
                TipoArquivoAnexo = TipoArquivoAnexoEnum.QuadroFotos,
                ArquivoAnexoPosicao = index,
                LaudoFoto = laudoFoto

            };
            foto.GuidArquivo = _arquivoAnexoService.EnviarArquivoSolicitacao(idSolicitacao, TipoArquivoAnexoEnum.QuadroFotos, arquivo, dataModificacao, index);
            AppSaveChanges();
            return ResponseResult(true, content: MontarArquivoAnexo(foto, false));

        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult Excluir(int idSolicitacao, string guidFoto)
        {
             
            _laudoFotoService.ExcluirFoto(idSolicitacao, new Guid(guidFoto));
            AppSaveChanges();

            return ResponseResult(true);
        }

        [HttpPost]
        public ActionResult FotosInspecao(int idSolicitacao)
        {
            var lstFotos = _arquivoAnexoService.ListarArquivoSolicitacao(idSolicitacao, TipoArquivoAnexoEnum.QuadroFotos);
            var lstResult = lstFotos.OrderBy(o => o.ArquivoAnexoPosicao).Select(foto => MontarArquivoAnexo(foto)).ToList();

            return ResponseResult(true, content: lstResult);
        }

        public FileStreamResult BaixarFotosZip(int Id)
        {
            FileStreamResult fileZip;
            var memory = _arquivoAnexoService.BaixarInspecaoFotosZip(Id);
            fileZip = new FileStreamResult(memory, "application/zip");
            fileZip.FileDownloadName = "Solicitacao_" + Id.ToString() + "_FotosInspecao.zip";
            return fileZip;
        }
        #endregion

        #region Quadro de Fotos

        public ActionResult QuadroFotos(int idSolicitacao)
        {
            var lstFotos = _arquivoAnexoService.ListarArquivoSolicitacao(idSolicitacao, TipoArquivoAnexoEnum.QuadroFotos);

            var lstFotosTemp = lstFotos.Where(i => i.LaudoFoto.IndQuadroFoto == false && i.IndExcluida != true).OrderBy(o => o.ArquivoAnexoPosicao)
                            .Select(foto => MontarArquivoAnexo(foto)).ToList();

            var lstQuadroFotos = _arquivoAnexoService.ListarQuadroFotosSolicitacao(idSolicitacao)
                                    .Select(foto => MontarArquivoAnexo(foto)).ToList();

            return ResponseResult(true, content: new { lstQuadroFotos, lstFotosTemp });
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult SalvarQuadroFotosPosicao(int id, List<QuadroFotoModel> quadroFotoModel)
        {
            if (quadroFotoModel != null)
            {
                Dictionary<int, int> dicQuadroFotoPosicao = quadroFotoModel.ToDictionary(i => i.Id, e => e.QuadroFotosPosicao);
                _laudoFotoService.SalvarQuadroFotoPosicao(id, dicQuadroFotoPosicao);
                AppSaveChanges();
            }

            return ResponseResult(true);
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult SalvarQuadroFotosNome(int id, List<QuadroFotoModel> quadroFotoModel)
        {
            Dictionary<int, string> dicArquivosNomes = quadroFotoModel.ToDictionary(i => i.Id, e => e.ArquivoNome);

            var lstFotos = _arquivoAnexoService.SalvarArquivoSolicitacaoRenomear(id, dicArquivosNomes);
            AppSaveChanges();

            var lstResult = lstFotos.Select(foto => MontarArquivoAnexo(foto)).ToList();

            return ResponseResult(true, content: lstResult);

        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public ActionResult SalvarQuadroFotosRemover(int Id, int IdSolicitacao)
        {
            _laudoFotoService.SalvarArquivoSolicitacaoQuadroFotosRemover(Id, IdSolicitacao);
            AppSaveChanges();
            return ResponseResult(true);
        }

        public FileStreamResult BaixarFotosQuadroZip(int Id)
        {
            FileStreamResult fileZip;
            var memory = _arquivoAnexoService.BaixarQuadroFotosZip(Id);
            fileZip = new FileStreamResult(memory, "application/zip");
            fileZip.FileDownloadName = "Solicitacao_" + Id.ToString() + "_QuadroFotos.zip";
            return fileZip;
        }

        public FileStreamResult BaixarFotosQuadroDoc(int Id, bool chkQuadroNumSeqLegenda)
        {

            var lstFotos = _arquivoAnexoService.ListarQuadroFotosSolicitacao(Id);
            var caminoArquivo = _wordService.MontarQuadro(lstFotos.ToList(), chkQuadroNumSeqLegenda);

            FileStreamResult fileZip;
            var memory = _arquivoAnexoService.BaixarMemoryStream(caminoArquivo);
            fileZip = new FileStreamResult(memory, "application/zip");
            fileZip.FileDownloadName = "Solicitacao_" + Id.ToString() + "_QuadroFotos.docx";

            return fileZip;
        }
        #endregion

        #region Metodos Auxiliares
        private dynamic MontarArquivoAnexo(ArquivoAnexo arquivoAnexo, bool imgBase64 = true)
        {
            var nfoto = new QuadroFotoModel
            {
                Id = arquivoAnexo.Id,
                IdSolicitacao = arquivoAnexo.IdSolicitacao,
                ArquivoDataModificacao = arquivoAnexo.ArquivoDataModificacao,
                ArquivoNome = arquivoAnexo.ArquivoNome,
                ArquivoExtencao = arquivoAnexo.ArquivoExtencao,
                ArquivoTamanhoBytes = arquivoAnexo.ArquivoTamanhoBytes,
                GuidFoto = arquivoAnexo.GuidArquivo,
                ImgData = imgBase64 ? UtilWeb.ImagemBase64(_configuracaoAplicativo.RepositorioSolicitacao + "//" + arquivoAnexo.IdSolicitacao + "//_thumbnail//" + arquivoAnexo.GuidArquivo) : string.Empty,
                QuadroFotosPosicao = arquivoAnexo.ArquivoAnexoPosicao
            };

            int ivalor;
            if (int.TryParse(arquivoAnexo.ArquivoNome.RemoveNonNumbers(), out ivalor))
                nfoto.fotoAnsNumero = ivalor;
            else
                nfoto.fotoAnsNumero = -1;


            return nfoto;
        }


        #endregion Metodos Auxiliares
    }

}





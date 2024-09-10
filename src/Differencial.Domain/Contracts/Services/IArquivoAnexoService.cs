using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;

namespace Differencial.Domain.Contracts.Services
{
    public interface IArquivoAnexoService
    {
        Guid EnviarArquivoSolicitacao(int IdSolicitacao, TipoArquivoAnexoEnum tipoArquivoAnexo, IFormFile arquivo, DateTime? arquivoDataModificacao = null, int? ArquivoAnexoPosicao = null, LaudoFoto laudoFoto = null);
        IEnumerable<ArquivoAnexo> Listar(ArquivoAnexoFilter fotoFilter);
        IEnumerable<ArquivoAnexo> ListarArquivoSolicitacao(int idSolicitacao, TipoArquivoAnexoEnum tipoArquivoAnexoEnum);
        IEnumerable<ArquivoAnexo> ListarQuadroFotosSolicitacao(int idSolicitacao);
        IEnumerable<ArquivoAnexo> SalvarArquivoSolicitacaoRenomear(int idSolicitacao, Dictionary<int, string> dicArquivosNomes);
        MemoryStream BaixarInspecaoFotosZip(int idSolicitacao);
        MemoryStream BaixarQuadroFotosZip(int id);
        MemoryStream BaixarMemoryStream(string caminhoarquivo);
        MemoryStream BaixarArquivoAnexo(ArquivoAnexo arquivoAnexo);
        //void GerarTodosThumbnail(int IdSolicitacao);
        void Excluir(ICollection<ArquivoAnexo> foto);
        void Excluir(ArquivoAnexo arquivoAnexo);
    }
}
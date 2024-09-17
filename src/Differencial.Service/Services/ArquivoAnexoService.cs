using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Collections.Generic;
using System;
using Differencial.Domain.Contracts.Util;
using System.IO;
using Differencial.Service.Util;
using Differencial.Domain;
using Differencial.Domain.Exceptions;
using System.Linq;
using System.IO.Compression;
using Differencial.Domain.EntitiesDTO;
using Differencial.Domain.Contracts.Infra;
using Microsoft.AspNetCore.Http;

namespace Differencial.Service.Services
{
	public class ArquivoAnexoService : Service, IArquivoAnexoService
	{
		IArquivoAnexoRepository _arquivoAnexoRepositorio;
		IConfiguracaoAplicativo _configuracaoAplicativo;
		IUsuarioService _usuarioService;

		public ArquivoAnexoService(IUnitOfWork uow,
									IConfiguracaoAplicativo configuracaoAplicativo,
									IArquivoAnexoRepository fotosRepositorio,
									IUsuarioService usuarioService)
			: base(uow)
		{
			_arquivoAnexoRepositorio = fotosRepositorio;
			_configuracaoAplicativo = configuracaoAplicativo;
			_usuarioService = usuarioService;
		}
		public IEnumerable<ArquivoAnexo> Listar(ArquivoAnexoFilter filtro)
		{
			return TryCatch(() =>
			{
				return _arquivoAnexoRepositorio.Where(filtro);
			});
		}
		public ArquivoAnexo Buscar(int id)
		{
			return TryCatch(() =>
			{
				return _arquivoAnexoRepositorio.Find(id);
			});
		}
		public ArquivoAnexo Buscar(Guid guidFoto)
		{
			return TryCatch(() =>
			{
				return _arquivoAnexoRepositorio.FirstOrDefault(i => i.GuidArquivo == guidFoto);
			});
		}
		public Guid EnviarArquivoAnexo(ArquivoAnexo entidade, Stream arquivo)
		{
			return TryCatch(() =>
			 {
				 entidade.Validate();

				 if (entidade.Id == 0)
				 {
					 entidade.GuidArquivo = Guid.NewGuid();
					 _arquivoAnexoRepositorio.Add(entidade);
				 }
				 else
					 throw new InvalidOperationException();

				 string path = _configuracaoAplicativo.RepositorioAnexos + "//";
				 ArquivoService.Salvar(arquivo, path, entidade.GuidArquivo.ToString());

				 return entidade.GuidArquivo;
			 });
		}
		private void Salvar(ArquivoAnexo entidade)
		{
			TryCatch(() =>
			{

				if (entidade.Id > 0)
				{
					_arquivoAnexoRepositorio.Update(entidade);
				}
				else
					throw new InvalidOperationException();

			});
		}

		#region Solicitacao

		public IEnumerable<ArquivoAnexo> ListarArquivoSolicitacao(int idSolicitacao, TipoArquivoAnexoEnum tipoArquivoAnexoEnum)
		{
			return TryCatch(() =>
			{
				return _arquivoAnexoRepositorio.ListarArquivoSolicitacao(idSolicitacao, tipoArquivoAnexoEnum);

			});
		}

		public IEnumerable<ArquivoAnexo> ListarQuadroFotosSolicitacao(int idSolicitacao)
		{
			return TryCatch(() =>
			{
				return _arquivoAnexoRepositorio.Where(i => i.IdSolicitacao == idSolicitacao
												&& i.TipoArquivoAnexo == TipoArquivoAnexoEnum.QuadroFotos
												&& i.LaudoFoto.IndQuadroFoto == true).OrderBy(o => o.LaudoFoto.QuadroFotosPosicao);
			});
		}

		public Guid EnviarArquivoSolicitacao(int idSolicitacao, TipoArquivoAnexoEnum tipoArquivoAnexo, IFormFile arquivo, DateTime? arquivoDataModificacao = null, int? arquivoAnexoPosicao = null, LaudoFoto laudoFoto = null)
		{
			return TryCatch(() =>
			{
				if (arquivo == null)
					throw new ValidationException("Arquivo não enviado");


				var entidade = new ArquivoAnexo()
				{
					IdSolicitacao = idSolicitacao,
					GuidArquivo = Guid.NewGuid(),

					ArquivoDataModificacao = arquivoDataModificacao == null ? DateTime.Now : arquivoDataModificacao.Value,
					ArquivoNome = Path.GetFileNameWithoutExtension(arquivo.FileName),
					ArquivoExtencao = Path.GetExtension(arquivo.FileName),
					ArquivoTamanhoBytes = arquivo.Length,
					TipoArquivoAnexo = tipoArquivoAnexo,
					ArquivoAnexoPosicao = arquivoAnexoPosicao ?? 0,
					LaudoFoto = laudoFoto
				};

				if (tipoArquivoAnexo == TipoArquivoAnexoEnum.QuadroFotos)
				{
					entidade.LaudoFoto = laudoFoto;
				}


				_arquivoAnexoRepositorio.Add(entidade);

				string path = _configuracaoAplicativo.RepositorioSolicitacao + "//" + entidade.IdSolicitacao + "//";

				if (tipoArquivoAnexo == TipoArquivoAnexoEnum.QuadroFotos)
				{
					// var imagem = new Imagem(arquivo.InputStream);
					var str = arquivo.OpenReadStream();
					var newInputStream = str;// ImageHelper.ReduzirTamanho(str, 1024);
					ArquivoService.Salvar(newInputStream, path, entidade.GuidArquivo.ToString());

					var thumbStream = ImageHelper.MontarThumbnailImage(newInputStream);
					ArquivoService.Salvar(thumbStream, path + "_thumbnail//", entidade.GuidArquivo.ToString());
				}
				else
				{
					//Salva Arquivo Original
					ArquivoService.Salvar(arquivo.OpenReadStream(), path, entidade.GuidArquivo.ToString());
				}

				return entidade.GuidArquivo;
			});
		}

		//public void GerarTodosThumbnail(int id)
		//{
		//    TryCatch(() =>
		//    {
		//        var lstFotos = ListarArquivoSolicitacao(id, TipoArquivoAnexoEnum.QuadroFotos);

		//        foreach (var entidade in lstFotos)
		//        {
		//            string path = _configuracaoAplicativo.RepositorioSolicitacao + "//" + entidade.IdSolicitacao + "//";


		//            var arquivo = File.OpenRead(path + entidade.GuidArquivo.ToString());
		//            arquivo.Seek(0, SeekOrigin.Begin);

		//            var thumbStream = ImagemService.MontarThumbnailImage(arquivo, entidade.ArquivoExtencao);
		//            ArquivoService.Salvar(thumbStream, path + "_thumbnail//", entidade.GuidArquivo.ToString());

		//        }

		//    });
		//}


		public IEnumerable<ArquivoAnexo> SalvarArquivoSolicitacaoRenomear(int idSolicitacao, Dictionary<int, string> dicArquivosNomes)
		{
			return TryCatch(() =>
			{
				var lstId = dicArquivosNomes.Select(i => i.Key).ToList();
				var lstArquivosAnexos = _arquivoAnexoRepositorio.Where(w => w.IdSolicitacao == idSolicitacao && lstId.Contains(w.Id)).ToList();

				foreach (var arquivo in lstArquivosAnexos)
				{
					arquivo.ArquivoNome = dicArquivosNomes[arquivo.Id];
					_arquivoAnexoRepositorio.Update(arquivo);
				}
				return lstArquivosAnexos;
			});
		}


		public MemoryStream BaixarInspecaoFotosZip(int idSolicitacao)
		{
			return TryCatch(() =>
			{
				var lstFotos = ListarArquivoSolicitacao(idSolicitacao, TipoArquivoAnexoEnum.QuadroFotos);
				var msReturn = new MemoryStream();
				using (var ms = new MemoryStream())
				{
					using (var zipArchive = new ZipArchive(ms, ZipArchiveMode.Create, true))
					{

						foreach (var arquivoAnexo in lstFotos)
						{
							string caminhoarquivo = _configuracaoAplicativo.RepositorioSolicitacao + "//" + arquivoAnexo.IdSolicitacao + "//" + arquivoAnexo.GuidArquivo;
							var nomeArqZip = arquivoAnexo.ArquivoNome + arquivoAnexo.ArquivoExtencao;
							var entry = zipArchive.CreateEntry(nomeArqZip, CompressionLevel.NoCompression);
							using (var entryStream = entry.Open())
							{

								if (!File.Exists(caminhoarquivo))
									throw new FileNotFoundException(string.Format("Arquivo não Encontrado '{0}'", arquivoAnexo.ArquivoNome));

								var file = File.OpenRead(caminhoarquivo);

								//entryStream.Read(bfile, 0, bfile.Length);


								file.Seek(0, SeekOrigin.Begin);
								file.CopyTo(entryStream);

							}


						}
					}

					ms.Seek(0, SeekOrigin.Begin);
					ms.CopyTo(msReturn);
				}
				msReturn.Seek(0, SeekOrigin.Begin);
				return msReturn;
			});

		}

		public MemoryStream BaixarQuadroFotosZip(int idSolicitacao)
		{
			return TryCatch(() =>
			{
				var lstFotos = ListarQuadroFotosSolicitacao(idSolicitacao);
				var msReturn = new MemoryStream();
				using (var ms = new MemoryStream())
				{
					using (var zipArchive = new ZipArchive(ms, ZipArchiveMode.Create, true))
					{
						foreach (var arquivoAnexo in lstFotos)
						{
							string caminhoarquivo = _configuracaoAplicativo.RepositorioSolicitacao + "//" + arquivoAnexo.IdSolicitacao + "//" + arquivoAnexo.GuidArquivo;
							var nomeArqZip = arquivoAnexo.ArquivoNome + arquivoAnexo.ArquivoExtencao;
							var entry = zipArchive.CreateEntry(nomeArqZip, CompressionLevel.NoCompression);
							using (var entryStream = entry.Open())
							{
								if (!File.Exists(caminhoarquivo))
									throw new FileNotFoundException("Arquivo não Encontrado");

								var file = File.OpenRead(caminhoarquivo);

								file.Seek(0, SeekOrigin.Begin);
								file.CopyTo(entryStream);
							}

						}
					}
					ms.Seek(0, SeekOrigin.Begin);
					ms.CopyTo(msReturn);
				}
				msReturn.Seek(0, SeekOrigin.Begin);
				return msReturn;
			});

		}

		public MemoryStream BaixarMemoryStream(string caminhoarquivo)
		{
			return TryCatch(() =>
			{
				var msReturn = new MemoryStream();
				using (var ms = new MemoryStream())
				{
					if (!File.Exists(caminhoarquivo))
						throw new FileNotFoundException("Arquivo não Encontrado");

					var file = File.OpenRead(caminhoarquivo);
					file.Seek(0, SeekOrigin.Begin);
					file.CopyTo(ms);

					ms.Seek(0, SeekOrigin.Begin);
					ms.CopyTo(msReturn);
				}
				msReturn.Seek(0, SeekOrigin.Begin);
				return msReturn;
			});

		}

		public MemoryStream BaixarArquivoAnexo(ArquivoAnexo arquivoAnexo)
		{
			return TryCatch(() =>
			{
				string caminhoarquivo = _configuracaoAplicativo.RepositorioSolicitacao + "//" + arquivoAnexo.IdSolicitacao + "//" + arquivoAnexo.GuidArquivo;
				return BaixarMemoryStream(caminhoarquivo);
			});

		}


		public void Excluir(ArquivoAnexo arquivoAnexo)
		{

			_arquivoAnexoRepositorio.Delete(arquivoAnexo);
			SaveChange(_usuarioService.Id);

			if (arquivoAnexo.TipoArquivoAnexo == TipoArquivoAnexoEnum.QuadroFotos)
			{
				string guidFoto = arquivoAnexo.GuidArquivo.ToString();

				string caminhoarquivo = _configuracaoAplicativo.RepositorioSolicitacao + "//" + arquivoAnexo.IdSolicitacao + "//";
				if (File.Exists(caminhoarquivo + guidFoto))
				{
					ArquivoService.Excluir(caminhoarquivo + guidFoto);
					ArquivoService.Excluir(caminhoarquivo + "_thumbnail//" + guidFoto);
				}
			}
		}

		public void Excluir(ICollection<ArquivoAnexo> lstArquivoAnexo)
		{
			TryCatch(() =>
			 {
				 foreach (var arquivoAnexo in lstArquivoAnexo.ToList())
				 {
					 Excluir(arquivoAnexo);
				 }
			 });
		}

		#endregion

	}
}
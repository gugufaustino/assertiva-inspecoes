using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Differencial.Service.Services
{
	public class LaudoFotoService : Service, ILaudoFotoService
	{
		ILaudoFotoRepository _laudoFotoRepositorio;
		IArquivoAnexoRepository _arquivoAnexoRepositorio;
		 
		IArquivoAnexoService _arquivoAnexoService;

        public LaudoFotoService(IUnitOfWork uow,
                                ILaudoFotoRepository laudoFotoRepositorio,
                                IArquivoAnexoRepository arquivoAnexoRepositorio, 
								IArquivoAnexoService arquivoAnexoService)
            : base(uow)
        {
            _laudoFotoRepositorio = laudoFotoRepositorio;
            _arquivoAnexoRepositorio = arquivoAnexoRepositorio;          
            _arquivoAnexoService = arquivoAnexoService;
        }

        public IEnumerable<LaudoFoto> Listar(LaudoFotoFilter filtro)
		{
			return TryCatch(() =>
			{
				return _laudoFotoRepositorio.Where(filtro);
			});
		}

		public void Salvar( LaudoFoto entidade)
		{
			TryCatch(() =>
			{ 
				if (entidade.Id == 0)
					_laudoFotoRepositorio.Add(entidade);
				else
					_laudoFotoRepositorio.Update(entidade);
			});
		}

		public void Excluir(  int id)
		{
			TryCatch(() =>
			{
				_laudoFotoRepositorio.Delete(id);
			});
		}

        public void SalvarQuadroFotoPosicao(int idSolicitacao, Dictionary<int, int> dicQuadroFotoPosicao)
        {
            TryCatch(() =>
            {
                var lstId = dicQuadroFotoPosicao.Select(i => i.Key).ToList();
                var lstLaudoFoto = _laudoFotoRepositorio.Where(w => w.ArquivoAnexo.IdSolicitacao == idSolicitacao && lstId.Contains(w.Id)).ToList();

                foreach (var item in lstLaudoFoto)
                {
                    item.QuadroFotosPosicao = dicQuadroFotoPosicao[item.Id];
                    item.IndQuadroFoto = true;
                    _laudoFotoRepositorio.Update(item);
                }
            });
        }


        public void SalvarArquivoSolicitacaoQuadroFotosRemover(int id, int idSolicitacao)
        {
            TryCatch(() =>
            {
                var foto = _laudoFotoRepositorio.Find(id);
                foto.IndQuadroFoto = false;
                Salvar(foto);
            });
        }

        public void ExcluirFoto(int idSolicitacao, Guid guid)
        {
			TryCatch(() =>
			{
				var arquivoAnexo = _arquivoAnexoRepositorio
									.Include(i=> i.LaudoFoto)
									.FirstOrDefault(i => i.IdSolicitacao == idSolicitacao && i.GuidArquivo == guid);

				if (arquivoAnexo.LaudoFoto != null)
					Excluir(arquivoAnexo.LaudoFoto.Id);

				_arquivoAnexoService.Excluir(arquivoAnexo);					
			
			});

			

		}
    }
}
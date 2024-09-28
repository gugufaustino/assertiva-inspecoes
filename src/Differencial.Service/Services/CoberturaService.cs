using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using Differencial.Domain.Util.ExtensionMethods;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Differencial.Service.Services
{
	public class CoberturaService : Service, ICoberturaService
	{
		ICoberturaRepository _coberturaRepositorio;

		public CoberturaService(IUnitOfWork uow, ICoberturaRepository coberturaRepositorio)
			: base(uow)
		{
			_coberturaRepositorio = coberturaRepositorio;
		}

		public IEnumerable<Cobertura> Listar(CoberturaFilter filtro)
		{
			return TryCatch(() =>
			{
				return _coberturaRepositorio.Where(filtro);
			});
		}

		private Cobertura Buscar(int id)
		{
			return TryCatch(() =>
			{
				return _coberturaRepositorio.Find(id);
			});
		}

		private void Salvar(Cobertura entidade)
		{
			TryCatch(() =>
			{
				if (entidade.Id == 0)
					_coberturaRepositorio.Add(entidade);
				else
				{
					var oldEntidade = Buscar(entidade.Id);
					oldEntidade.NomeCobertura = entidade.NomeCobertura;
					oldEntidade.VlrCobertura = entidade.VlrCobertura;
					_coberturaRepositorio.Update(oldEntidade);
				}
			});
		}

		public void Salvar(IEnumerable<Cobertura> entidade, int IdSolicitacao)
		{
			TryCatch(() =>
			{
				// Remover outros registros fora do conjunto
				var lstIds = entidade.Select(i => i.Id).ToList();
				var lstItensExcluir = _coberturaRepositorio.Where(w => w.IdSolicitacao == IdSolicitacao && !lstIds.Contains(w.Id)).ToList();
				if (lstItensExcluir.Any())
					Excluir(lstItensExcluir.Select(i => i.Id).ToArray());

				foreach (var item in entidade)
				{
					if (!item.NomeCobertura.IsNullOrEmpty())
					{
						item.IdSolicitacao = IdSolicitacao;
						Salvar(item);
					}

				}
			});
		}

		public void Excluir(ICollection<Cobertura> cobertura)
		{
			foreach (var item in cobertura)
			{
				_coberturaRepositorio.Delete(item);

			}

		}

		public void Excluir(int[] ids)
		{

			_coberturaRepositorio.Delete(ids);

		}

	}
}
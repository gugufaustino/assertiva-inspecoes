using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Collections.Generic;

namespace Differencial.Service.Services
{
    public class TipoAssuntoService : Service, ITipoAssuntoService
    {
        ITipoAssuntoRepository _tipoAssuntoRepositorio;

        public TipoAssuntoService(IUnitOfWork uow, ITipoAssuntoRepository tipoAssuntoRepositorio)
            : base(uow)
        {
            _tipoAssuntoRepositorio = tipoAssuntoRepositorio;
        }

        public IEnumerable<TipoAssunto> Listar(TipoAssuntoFilter filtro)
        {
           
            return TryCatch(() =>
            {
                return _tipoAssuntoRepositorio.Where(filtro);
            });
        }

        public void Salvar(TipoAssunto entidade)
        {
            TryCatch(() =>
            {
                entidade.Validate();

                if (entidade.Id == 0)
                    _tipoAssuntoRepositorio.Add(entidade);
                else
                    _tipoAssuntoRepositorio.Update(entidade);
            });
        }

        public void Excluir(int[] ids)
        {
            TryCatch(() =>
            {
                foreach (var id in ids)
                {
                    Excluir(id);
                }
               
            });
        }
        public void Excluir(int id)
        {
            TryCatch(() =>
            {
                _tipoAssuntoRepositorio.Delete(id);
            });
        }

        public TipoAssunto Buscar(int id)
        {
            return TryCatch(() =>
            {
                return _tipoAssuntoRepositorio.Find(id);
            });
        }
    }
}
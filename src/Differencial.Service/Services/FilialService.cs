using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Collections.Generic;
using System;
using System.Linq;
using Differencial.Domain.Util.ExtensionMethods;

namespace Differencial.Service.Services
{
    public class FilialService : Service, IFilialService
    {
        IFilialRepository _filialRepositorio;

        public FilialService(IUnitOfWork uow, IFilialRepository filialRepositorio)
            : base(uow)
        {
            _filialRepositorio = filialRepositorio;
        }

        public IEnumerable<Filial> Listar(FilialFilter filtro)
        {
            return TryCatch(() =>
            {
                return _filialRepositorio.Where(filtro);
            });
        }

        public void Salvar(Filial entidade)
        {
            TryCatch(() =>
            {
                entidade.Validate();

                if (entidade.Id == 0)
                    _filialRepositorio.Add(entidade);
                else
                    _filialRepositorio.Update(entidade);
            });
        }

        public void Excluir(int id)
        {
            TryCatch(() =>
            {
                _filialRepositorio.Delete(id);
            });
        }

        public Filial Buscar(int id)
        {
           return TryCatch(() =>
            {
               return _filialRepositorio.Find(id);
            });
        }

        public void Excluir(int[] ids)
        {    
            ids.ForEach(Excluir);
        }
    }
}
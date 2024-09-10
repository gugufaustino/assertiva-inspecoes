using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Collections.Generic;

namespace Differencial.Service.Services
{
    public class TipoInspecaoService : Service, ITipoInspecaoService
    {
        ITipoInspecaoRepository _serviceRepositorio;

        public TipoInspecaoService(IUnitOfWork uow, ITipoInspecaoRepository tipoInspecaoRepositorio)
            : base(uow)
        {
            _serviceRepositorio = tipoInspecaoRepositorio;
        }

        public IEnumerable<TipoInspecao> Listar(TipoInspecaoFilter filtro)
        {
            return TryCatch(() =>
            {
                return _serviceRepositorio.Where(filtro);
            });
        }

        public void Salvar(TipoInspecao entidade)
        {
            TryCatch(() =>
            {
                entidade.Validate();
                if (entidade.Id == 0)
                {
                    entidade.IndAtivo = true;
                    _serviceRepositorio.Add(entidade);
                }
                else
                {
                    TipoInspecao oldEntidade = Buscar(entidade.Id);
                    //Alteração de interface
                    // Se houver outro case de alteração dessa entidade então implementar outro metodo 
                    oldEntidade.IndAtivo = true;
                    oldEntidade.NomeTipoInspecao = entidade.NomeTipoInspecao;
                    oldEntidade.DescricaoTipoInspecao = entidade.DescricaoTipoInspecao;


                    _serviceRepositorio.Update(oldEntidade);
                }

            });
        }

        public void Excluir( int id)
        {
            TryCatch(() =>
            {
                _serviceRepositorio.Delete(id);
            });
        }


        public void Excluir(int[] ids)
        {
            TryCatch(() =>
            {
                _serviceRepositorio.Delete(ids);
            });
        }
        public TipoInspecao Buscar(int id)
        {
            return TryCatch(() =>
            {
                return _serviceRepositorio.Find(id);
            });
        }
    }
}
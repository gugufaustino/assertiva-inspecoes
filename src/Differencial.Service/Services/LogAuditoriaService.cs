using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Collections.Generic;
using System.Linq;
using System;
using Differencial.Domain.Queries;

namespace Differencial.Service.Services
{
    public class LogAuditoriaService : Service, ILogAuditoriaService
    {
        ILogAuditoriaRepository _logAuditoriaRepositorio;
        ILogAuditoriaQueries _dpLogAuditoriaRepositorio;

        public LogAuditoriaService(IUnitOfWork uow,
            ILogAuditoriaRepository logAuditoriaRepositorio,
            ILogAuditoriaQueries logAuditoriaDapperRepositorio)
            : base(uow)
        {
            _logAuditoriaRepositorio = logAuditoriaRepositorio;
            _dpLogAuditoriaRepositorio = logAuditoriaDapperRepositorio;
        }

        public IEnumerable<LogAuditoria> Listar(int IdTabela, IEntity entidadeenum)
        {
            return TryCatch(() =>
            {
                var filtro = new LogAuditoriaFilter()
                {
                    IdTabela = IdTabela,
                    Tabela = entidadeenum.GetType().Name,                    
                }.OrderByField(CampoOrdenacaoLogAuditoria.DataAcao);

                 return _logAuditoriaRepositorio.Where(filtro); 
            });
        }

        private IEnumerable<LogAuditoria> ListarDapper(int IdTabela, IEntity entidadeenum)
        {
            return TryCatch(() =>
            {
                var filtro = new LogAuditoriaFilter()
                {
                    IdTabela = IdTabela,
                    Tabela = entidadeenum.GetType().BaseType.Name,                                        
                }.OrderByField(CampoOrdenacaoLogAuditoria.DataAcao);

                return _dpLogAuditoriaRepositorio.Where(filtro);
                
            });
        }

        public IEnumerable<LogAuditoria> Listar(List<IEntity> entidades)
        {
            return TryCatch(() =>
            {
                var lstEntidades = new List<LogAuditoria>();

                foreach (var entidade in entidades)
                {
                    lstEntidades.AddRange(Listar(entidade.Id, entidade).ToList());
                }

                return lstEntidades.OrderBy(o => o.DataAcao).ToList();
            }); 
        }

        public IEnumerable<LogAuditoria> ListarDapper(List<IEntity> entidades) {

            return TryCatch(() =>
            {
                var lstEntidades = new List<LogAuditoria>();

                foreach (var entidade in entidades)
                {
                    lstEntidades.AddRange(ListarDapper(entidade.Id, entidade).ToList());
                }

                return lstEntidades.OrderBy(o => o.DataAcao).ToList();
            });

        }

        public void Salvar(int codigoUsuarioLogado, LogAuditoria entidade)
        {
            TryCatch(() =>
            {
                entidade.Validate();

                if (entidade.Id == 0)
                    _logAuditoriaRepositorio.Add(entidade);
                else
                    _logAuditoriaRepositorio.Update(entidade);
            });
        }

        public void Excluir(int codigoUsuarioLogado, int id)
        {
            TryCatch(() =>
            {
                _logAuditoriaRepositorio.Delete(id);
            });
        }
        public List<LogAuditoria> ListarTodosEF()
        {
            var obj = _logAuditoriaRepositorio.All();
            return (List<LogAuditoria>)obj;
        }
        public List<LogAuditoria> ListarTodosDP()
        {
            var obj = _dpLogAuditoriaRepositorio.All();
            return (List<LogAuditoria>)obj;
        }

        public IEnumerable<LogAuditoria> Listar(LogAuditoriaFilter filtro)
        {
           return TryCatch(() =>
            {
                return _logAuditoriaRepositorio.Where(filtro);
            });
        }

        public IEnumerable<LogAuditoria> ListarDP(LogAuditoriaFilter filtro)
        {
            return TryCatch(() =>
            {
                return _dpLogAuditoriaRepositorio.Where(filtro);
            });
        }
    }
}
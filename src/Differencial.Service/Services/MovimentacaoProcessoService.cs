using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Collections.Generic;
using System;
using Differencial.Domain.Contracts.Entities;

namespace Differencial.Service.Services
{
    public class MovimentacaoProcessoService : Service, IMovimentacaoProcessoService
    {
        IMovimentacaoProcessoRepository _movimentacaoProcessoRepositorio;

        public MovimentacaoProcessoService(IUnitOfWork uow, IMovimentacaoProcessoRepository movimentacaoProcessoRepositorio)
            : base(uow)
        {
            _movimentacaoProcessoRepositorio = movimentacaoProcessoRepositorio;
        }

        public IEnumerable<MovimentacaoProcesso> Listar(MovimentacaoProcessoFilter filtro)
        {
            return TryCatch(() =>
            {
                return _movimentacaoProcessoRepositorio.Where(filtro);
            });
        }

        public void Inserir(IWorkFlowMovimentacaoProcesso wfMovimentoProcesso)
        {
            TryCatch(() =>
            {
                var entidade = (MovimentacaoProcesso)wfMovimentoProcesso;
                entidade.Validate();
                _movimentacaoProcessoRepositorio.Add(entidade);


            });
        }

        public void Excluir( int id)
        {
            TryCatch(() =>
            {
                _movimentacaoProcessoRepositorio.Delete(id);
            });
        }

        public void Editar(MovimentacaoProcesso entidade)
        {
            TryCatch(() =>
            {
                entidade.Validate();
                _movimentacaoProcessoRepositorio.Update(entidade);
            });
        }

        public void Excluir(int[] ids)
        {
            TryCatch(() =>
            {               
                _movimentacaoProcessoRepositorio.Delete(ids);
            });

             
        }
    }
}
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Differencial.Service.Services
{
    public class ContratoLancamentoService : Service, IContratoLancamentoService
    {
        IContratoLancamentoRepository _contratoLancamentoRepositorio;
        IContratoLancamentoValorService _contratoLancamentoValorService;


        public ContratoLancamentoService(IUnitOfWork uow,
            IContratoLancamentoRepository contratoLancamentoRepositorio,
            IContratoLancamentoValorService contratoLancamentoValorService)
            : base(uow)
        {
            _contratoLancamentoRepositorio = contratoLancamentoRepositorio;
            _contratoLancamentoValorService = contratoLancamentoValorService;
        }

        public IEnumerable<ContratoLancamento> Listar(ContratoLancamentoFilter filtro)
        {
            return TryCatch(() =>
            {
                return _contratoLancamentoRepositorio.Where(filtro);
            });
        }

        public void Salvar(ContratoLancamento entidade)
        {
            TryCatch(() =>
            {
                entidade.Validate();

                // Remove registros "Excuidos" na interface
                _contratoLancamentoValorService.ExcluirNaoContidos(entidade.Id, entidade.ContratoLancamentoValor); 


                _contratoLancamentoValorService.Salvar(entidade.ContratoLancamentoValor, entidade.Id);

                if (entidade.Id == 0)
                    _contratoLancamentoRepositorio.Add(entidade);
                else
                { 
                    var oldEntidade = Buscar(entidade.Id);
                    //oldEntidade.IndParametroQuantitativoVariavel = entidade.IndParametroQuantitativoVariavel;
                    oldEntidade.TipoContratoLancamento = entidade.TipoContratoLancamento;
                    oldEntidade.TipoParametroQuantitativoVariavel = entidade.TipoParametroQuantitativoVariavel;

                    _contratoLancamentoRepositorio.Update(oldEntidade);
                }

            });
        }
 
        private ContratoLancamento Buscar(int id)
        {
            return TryCatch(() =>
            {
                return _contratoLancamentoRepositorio.Find(id);
            });
        }

        public void Excluir(int id)
        {
            TryCatch(() =>
            {
                _contratoLancamentoRepositorio.Delete(id);
            });
        }

        public void Salvar(IEnumerable<ContratoLancamento> lstEntidade)
        {
            TryCatch(() =>
            {
                foreach (var item in lstEntidade)
                {
                    Salvar(item);
                }
            });
        }
    }
}
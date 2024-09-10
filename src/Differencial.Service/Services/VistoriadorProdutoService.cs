using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Collections.Generic;
using Differencial.Domain.Exceptions;
using Differencial.Domain.Resources;
using Differencial.Domain.DTO;

namespace Differencial.Service.Services
{
    public class VistoriadorProdutoService : Service, IVistoriadorProdutoService
    {
        IVistoriadorProdutoRepository _vistoriadorProdutoRepositorio;

        public VistoriadorProdutoService(IUnitOfWork uow, IVistoriadorProdutoRepository vistoriadorProdutoRepositorio)
            : base(uow)
        {
            _vistoriadorProdutoRepositorio = vistoriadorProdutoRepositorio;
        }

        public IEnumerable<VistoriadorProduto> Listar(VistoriadorProdutoFilter filtro)
        {
            return TryCatch(() =>
            {
                return _vistoriadorProdutoRepositorio.Where(filtro);
            });
        }

        public void Salvar(int codigoUsuarioLogado, VistoriadorProduto entidade)
        {
            TryCatch(() =>
            {
                entidade.Validate();

                if (entidade.Id == 0)
                    _vistoriadorProdutoRepositorio.Add(entidade);
                else
                    _vistoriadorProdutoRepositorio.Update(entidade);
            });
        }

        public void Excluir(int codigoUsuarioLogado, int id)
        {
            TryCatch(() =>
            {
                _vistoriadorProdutoRepositorio.Delete(id);
            });
        }

        public void Ativar(int[] Ids)
        {
            TryCatch(() =>
            {
                foreach (var id in Ids)
                {
                    if (id < 1)
                        throw new ValidationException(MensagensValidacaoServicos.VistoriadorProdutoRnAtivarSemValor);

                    _vistoriadorProdutoRepositorio.Ativar(id);
                }

            });
        }

        public void Desativar(int[] Ids)
        {
            TryCatch(() =>
            {
                foreach (var id in Ids)
                {
                    if (id < 1)
                        throw new ValidationException(MensagensValidacaoServicos.VistoriadorProdutoRnDesativarSemValor);
                    _vistoriadorProdutoRepositorio.Desativar(id);
                }
            });
        }

        public void SalvarValoresVistoriadorProduto(int idVistoriador, KeyVistoriadorProdutoLancamentoDTO[] arrVistoriadorProduto, decimal vlrQuilometroRodado, decimal vlrPagamentoVistoria)
        {
            TryCatch(() =>
            {
                foreach (var vistoriadorProdutoLancamento in arrVistoriadorProduto)
                {
                    if (vistoriadorProdutoLancamento.IdVistoriadorProduto > 0)
                    {
                        var entidade = _vistoriadorProdutoRepositorio.FirstOrDefault(vp => vp.Id == vistoriadorProdutoLancamento.IdVistoriadorProduto);
                        entidade.VlrPagamentoVistoria = vlrPagamentoVistoria;
                        entidade.VlrQuilometroRodado = vlrQuilometroRodado;

                        entidade.IdContratoLancamento = vistoriadorProdutoLancamento.IdContratoLancamento;
                        entidade.IdContratoLancamentoValor = vistoriadorProdutoLancamento.IdContratoLancamentoValor;

                        Salvar(1, entidade);
                    }
                    else //Insere
                    {

                        var novaEntidade = new VistoriadorProduto()
                        {
                            IdProduto = vistoriadorProdutoLancamento.IdProduto,
                            IdContratoLancamento = vistoriadorProdutoLancamento.IdContratoLancamento,
                            IdContratoLancamentoValor = vistoriadorProdutoLancamento.IdContratoLancamentoValor,
                            IdVistoriador = idVistoriador,
                            IndAtivo = true,
                            VlrPagamentoVistoria = vlrPagamentoVistoria,
                            VlrQuilometroRodado = vlrQuilometroRodado
                        };
                        Salvar(1, novaEntidade);
                    }
                }
            });
        }
    }
}
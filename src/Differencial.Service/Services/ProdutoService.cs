using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Collections.Generic;
using Differencial.Domain.DTO;
using System.Linq;
using System.Threading.Tasks;
namespace Differencial.Service.Services
{
    public class ProdutoService : Service, IProdutoService
	{
		IProdutoRepository _produtoRepositorio;
        IContratoService _contratoService;

		public ProdutoService(IUnitOfWork uow,
                IProdutoRepository produtoRepositorio,
                IContratoService contratoService)
			: base(uow)
		{
			_produtoRepositorio = produtoRepositorio;
            _contratoService = contratoService;
        }

		public IEnumerable<Produto> Listar(ProdutoFilter filtro)
		{
			return TryCatch(() =>
			{
				return _produtoRepositorio.Where(filtro);
			});
		}

		public void Salvar(Produto entidade)
        {
			TryCatch(() =>
			{
				entidade.Validate();

                if (entidade.Id == 0)
                {
                    entidade.IndAtivo = true;
                    _contratoService.Salvar(entidade.Contrato);
                    _produtoRepositorio.Add(entidade);
                   
                }
                else
                {
                    var oldEntidade = Buscar(entidade.Id);
                    oldEntidade.IdSeguradora = entidade.IdSeguradora;
                    oldEntidade.CodProdutoSeguradora = entidade.CodProdutoSeguradora;
                    oldEntidade.NomeProdutoSeguradora = entidade.NomeProdutoSeguradora;
                    oldEntidade.IdTipoInspecao = entidade.IdTipoInspecao;
                    oldEntidade.NomeProduto = entidade.NomeProduto;
                    oldEntidade.VlrDespesa = entidade.VlrDespesa;
                    oldEntidade.VlrReceber = entidade.VlrReceber;
                    oldEntidade.VlrQuilometragem = entidade.VlrQuilometragem;
                    oldEntidade.IndFranquiaQuilometragem = entidade.IndFranquiaQuilometragem;
                    oldEntidade.QtdFranquiaQuilometragem = entidade.QtdFranquiaQuilometragem;
                    oldEntidade.IndBlocoExtra = entidade.IndBlocoExtra;
                    oldEntidade.VlrBlocoExtr = entidade.VlrBlocoExtr;           

                    entidade.Contrato.Produto = oldEntidade;

                    _contratoService.Salvar(entidade.Contrato);
                    _produtoRepositorio.Update(oldEntidade);
                  
                }

            });
		}

		public void Excluir( int id)
		{
			TryCatch(() =>
			{
				_produtoRepositorio.Delete(id);
			});
		}
        public void Excluir( int[] ids)
        {
            TryCatch(() =>
            {
                _produtoRepositorio.Delete(ids);
            });
        }
        public Produto Buscar(int id)
        {
            return TryCatch(() =>
            {
                return _produtoRepositorio.Find(id);
            });
        }
        public Task<Produto> BuscarParaEditar(int id)
        {
            return TryCatch(() =>
            {
                return _produtoRepositorio.BuscarParaEditarView(id);
            });
        }

        public IEnumerable<VistoriadorProdutoValorDTO> ListarDiponivelParaVistoriador(int idVistoriador)
        {
            return TryCatch(() =>
            {
                return _produtoRepositorio.ListarDiponivelParaVistoriador(idVistoriador);
            });
        }

        public Produto BuscarPorCodProdutoSeguradora(string codProdutoSeguradora)
        {
            return TryCatch(() =>
            {
                return _produtoRepositorio.FirstOrDefault(i => i.CodProdutoSeguradora == codProdutoSeguradora && i.CodProdutoSeguradora != null);
            });
        }

        public bool ExisteDadosFinanceiros(int idProduto)
        {
            return _produtoRepositorio.ExisteDadosFinanceiros(idProduto);

        }
    }
}
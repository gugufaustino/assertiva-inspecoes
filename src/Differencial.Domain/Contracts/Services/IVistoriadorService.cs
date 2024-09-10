using Differencial.Domain.DTO;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
	public interface IVistoriadorService
	{
		IEnumerable<Vistoriador> Listar(VistoriadorFilter filtro); 
		void Salvar(int codigoUsuarioLogado, Vistoriador entidade); 
		void Excluir(int codigoUsuarioLogado, int id);
        IEnumerable<Vistoriador> ListarVistoriadorPorProduto(int idProduto);
        Vistoriador Buscar(int id);
        List<OperadorDistancia> ListarOperadorDistancia(double latitude, double longitude, int idProduto, int? IdContratoLancamentoValor);
    }
}
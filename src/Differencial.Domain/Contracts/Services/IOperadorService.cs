using Differencial.Domain.DTO;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Differencial.Domain.Contracts.Services
{
    public interface IOperadorService : IBaseService<Operador, OperadorFilter>
    {
        IEnumerable<OperadorDistancia> ListarOperadorDistanciaPorProximidadeGeo(string siglaUf, string municipio, int idProduto, int idSolicitacao, int? IdContratoLancamentoValor, double latitude, double longitude);

		Task Salvar(Operador entidade, IFormFile inputFoto); 
        
        OperadorDistancia BuscarOperadorDistanciaSolicitacao(int idVistoriador, int idSolicitacao, string siglaUf, string municipio, double latDestino, double longDestino, int idProduto, int? IdContratoLancamentoValor);

        Operador BuscarPorToken(string tokenTransacao);

        OperadorDistancia MontarOperadorDistanciaRota(Vistoriador Vistoriador, Endereco EnderecoRotaParida, string siglaUf, string municipio, double latDestino, double longDestino, int idSolicitacao, int idProduto);

        void SalvarGerarAcesso(int idOperador);

        void GerarNovoAcesso(string usuario);

        void SalvarMudarSenha(int idOperador, string senhaAnterior, string senhaNova);

        Operador BuscarLogon(string usuario, string senha);
        Operador BuscarPorUsuario(string usuario);

        void SalvarSolicitanteSemAcesso(Operador entidade, int idSeguradora);
        void ExcluirSolicitante(int[] id);

        Task<Operador> BuscarParaEditar(int id);

        new IEnumerable<Operador> Listar(OperadorFilter filtro);


        IEnumerable<Operador> ListarOperadorCadastro(OperadorFilter filtro);
    }
}
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Differencial.Domain.Contracts.Services
{
    public interface IWorkFlowSolicitacaoService : IBaseService<Solicitacao, SolicitacaoFilter>
    {
        Task Enviar(int Id, string txtMensagemMovimento, IFormFile arquivo); 
        void Apropriar(int Id);
        void Cancelar(int id, string textoMovimentacao);
        void Devolver(int id, string textoMovimentacao, TipoMotivoEnum? tipoMotivo);
        void Concluir(int id);
         
    }
}
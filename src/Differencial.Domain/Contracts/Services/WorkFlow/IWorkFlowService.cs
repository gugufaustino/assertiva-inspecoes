using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Entities;
using Differencial.Domain.Enums.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differencial.Domain.Contracts.Services
{
    public interface IWorkFlowService 
    {
        void Criar(IWorkFlowInstanciaProcesso instanciaProcesso, TipoSituacaoProcessoEnum situacaoInicio = TipoSituacaoProcessoEnum.EmElaboracao);
        int Enviar(IWorkFlowInstanciaProcesso instanciaProcesso, string textoMovimentacao);
        //int Enviar(IWorkFlowInstanciaProcesso instanciaProcesso, TipoSituacaoProcessoEnum tipoTipoSituacaoProcessoEnum, string textoMovimentacao);
        int Devolver(IWorkFlowInstanciaProcesso instanciaProcesso, string textoMovimentacao);
        void Cancelar(IWorkFlowInstanciaProcesso instanciaProcesso, string textoMovimentacao);
        void Concluir(IWorkFlowInstanciaProcesso instanciaProcesso, string textoMovimentacao);
        void Apropriar(IWorkFlowInstanciaProcesso instanciaProcesso);
        List<WFTipoAcao> AcoesDisponiveis(IWorkFlowInstanciaProcesso instanciaProcesso, int IdOperadorLogado, int[] tipoPapel);
        List<int> ProximoMovimento<T>(T situacaoAtual, WFTipoAcao tipoAcao);

        int[] MotivoDisponiveis (TipoSituacaoProcessoEnum tipoSituacaoProcessoEnum);

        List<TipoAtividadeEnum> AtividadesDisponiveis(IWorkFlowInstanciaProcesso instanciaProcesso, int IdOperadorLogado, int[] tipoPapel);
    }
}

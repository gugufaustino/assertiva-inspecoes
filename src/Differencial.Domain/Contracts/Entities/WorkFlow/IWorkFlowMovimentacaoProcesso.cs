using Differencial.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Entities
{
    public interface IWorkFlowMovimentacaoProcesso
    {
        string TextoMovimentacao { get; set; }

        TipoSituacaoProcessoEnum TipoSituacaoProcesso { get; set; }

        int IdOperadorOrigem { get; set; }

        int? IdOperadorDestino { get; set; }

        DateTime DthMovimentacao { get; set; }

        DateTime? DthApropriacao { get; set; }

        DateTime? DthConclusao { get; set; }

        TipoSituacaoMovimentoEnum TipoSituacaoMovimento { get; set; }

        int IdSolicitacao { get; set; }
        IWorkFlowInstanciaProcesso InstanciaProcesso { get; set; }
    }
}
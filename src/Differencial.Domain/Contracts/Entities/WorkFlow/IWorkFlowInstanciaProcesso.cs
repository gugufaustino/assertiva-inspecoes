using Differencial.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Entities
{
    public interface IWorkFlowInstanciaProcesso  
    {
        int Id { get; set; }
        TipoSituacaoProcessoEnum? TpSituacao { get; set; }
        ICollection<IWorkFlowMovimentacaoProcesso> WorkFlowInstanciaProcesso { get;   }
        int? IdOperadorApropriado { get; set; }
        ICollection<AtividadeProcesso> AtividadeProcesso { get; set; }
    }
}
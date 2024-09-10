using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differencial.Domain.Contracts.Entities
{
    public interface INotificacaoProcesso
    {
        int Id { get; set; }
        TipoSituacaoProcessoEnum? TpSituacao { get; set; }
    }
}

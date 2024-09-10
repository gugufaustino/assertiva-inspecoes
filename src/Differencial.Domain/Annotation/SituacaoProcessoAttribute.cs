using Differencial.Domain.Enums.WorkFlow;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differencial.Domain.Annotation
{
    
    public  class SituacaoProcessoAttribute : Attribute
    {
        //
        // Resumo:
        //     Gets or sets a value that is used for display in the UI.
        //
        // Devoluções:
        //     A value that is used for display in the UI.
        public string Name { get; set; }
        //
        // Resumo:
        //     Gets or sets a value that is used for the grid column label.
        //
        // Devoluções:
        //     A value that is for the grid column label.
        public string ShortName { get; set; }

        public bool IndGerarAtividade { get; set; }

        public bool IndMotivocao { get; set; }

        /// <summary>
        /// Define ações para o papel corrente(apropriado)
        /// </summary>
        public WFTipoAcao[] WFTipoAcaoOperadorApropriado { get; set; }

        /// <summary>
        /// Define ações para o papel definido em WFMesmoPapelAtuante
        /// </summary>
        public WFTipoAcao[] WFTipoAcaoPapelAtuante { get; set; }

        /// <summary>
        /// Possibilita definir qual papél pode atuar em ações definidas no atributo WFTipoAcaoPapelAtuante
        /// </summary>
        public int WFPapelAtuante { get; set; }
        
        public string NomeAcao { get; set; }
        public int[] WFMotivoAcao { get; set; }
    }
}

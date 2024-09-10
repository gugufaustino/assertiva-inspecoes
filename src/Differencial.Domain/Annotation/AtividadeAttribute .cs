using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differencial.Domain.Annotation
{
    
    public  class AtividadeAttribute : Attribute
    { //
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

        public bool IndAtividadeOpicional { get; set; }

        public int IdSituacaoProcessoOrigem { get; set; }

    }
}

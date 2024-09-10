using Differencial.Domain.Enums.WorkFlow;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differencial.Domain.Annotation
{

    public class MotivoAcaoAttribute : Attribute
    {
        public WFTipoAcao WFTipoAcao { get; set; }
         
        public int[] WFMotivoAcao { get; set; }

 
    }
}

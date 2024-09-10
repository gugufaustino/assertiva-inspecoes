using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differencial.Domain.Annotation
{
    
    public  class CoberturaAttribute : Attribute
    {  
        public string Name { get; set; }       
        public bool IndPadraoInclusao { get; set; }
 
    }
}

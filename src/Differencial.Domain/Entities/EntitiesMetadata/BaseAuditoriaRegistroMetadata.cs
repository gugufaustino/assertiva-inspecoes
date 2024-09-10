using System;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class BaseAuditoriaRegistroMetadata
    {


        [Display(Name = "Data Cadastro", ShortName = "Cadastro")]
        public DateTime DataCadastro { get; set; }

        [Display(Name = "Data Modificacao", ShortName = "Modificacao")]
        public DateTime DataModificacao { get; set; }

        [Display(Name = "Código Operador de Cadastro")]
        public int IdOperadorCadastro { get; set; }
 
        [Display(Name = "Código Operador de Modificação")]
        public int IdOperadorModificacao { get; set; } 
        
    }
}
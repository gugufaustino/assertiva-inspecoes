using System;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class CoberturaMetadata : BaseAuditoriaRegistroMetadata
    {
        [Display(Name = "C�digo Cobertura")] 
        public int Id { get; set; }


        [Display(Name = "Nome Cobertura")]
        [Required]
        [MaxLength(250)]
        public string NomeCobertura { get; set; }

    }
}
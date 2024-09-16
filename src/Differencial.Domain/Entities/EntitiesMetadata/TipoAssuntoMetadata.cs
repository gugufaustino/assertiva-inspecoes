using Differencial.Domain.Contracts.Entities;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class TipoAssuntoMetadata : BaseAuditoriaRegistroMetadata 
    {

        [Display(Name = "C�digo")]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Nome Assunto", ShortName = "Nome Assunto")]
        public string NomeAssunto { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Texto Padr�o")]
        public string TextoPadrao { get; set; }

      
        
    }
}
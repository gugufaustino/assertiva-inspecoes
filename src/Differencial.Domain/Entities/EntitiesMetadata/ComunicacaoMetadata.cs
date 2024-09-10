using System;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class ComunicacaoMetadata  : BaseAuditoriaRegistroMetadata
    {

        [Display(Name = "C�digo Comunica��o")]
        public int Id { get; set; }

        
        [Display(Name = "Tipo Assunto")]
        public int? IdTipoAssunto { get; set; }

        [Required]
        [Display(Name = "Tipo de Comunica��o")]
        public int TipoComunicacao { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Assunto")]
        public string Assunto { get; set; }

         
        [MaxLength(1000)]
        [Display(Name = "Texto")]
        public string TextoComunicacao { get; set; }

        [Required]
        [Display(Name = "Solicita��o")]
        public int IdSolicitacao { get; set; }
 
    }
}
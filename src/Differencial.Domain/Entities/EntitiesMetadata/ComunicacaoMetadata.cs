using System;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class ComunicacaoMetadata  : BaseAuditoriaRegistroMetadata
    {

        [Display(Name = "Código Comunicação")]
        public int Id { get; set; }

        
        [Display(Name = "Tipo Assunto")]
        public int? IdTipoAssunto { get; set; }

        [Required]
        [Display(Name = "Tipo de Comunicação")]
        public int TipoComunicacao { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Assunto")]
        public string Assunto { get; set; }

         
        [MaxLength(1000)]
        [Display(Name = "Texto")]
        public string TextoComunicacao { get; set; }

        [Required]
        [Display(Name = "Solicitação")]
        public int IdSolicitacao { get; set; }
 
    }
}
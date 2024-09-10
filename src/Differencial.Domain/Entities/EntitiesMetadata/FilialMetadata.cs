using Differencial.Domain.Contracts.Entities;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class FilialMetadata : BaseAuditoriaRegistroMetadata 
    {

        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Nome Filial", ShortName = "Nome Filial")]
        public string NomeFilial { get; set; }
         
        
    }
}
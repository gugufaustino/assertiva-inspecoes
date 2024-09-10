using Differencial.Domain.Contracts.Entities;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class TipoInspecaoMetadata : BaseAuditoriaRegistroMetadata, IAtivavel
    {

        [Display(Name = "Código")]
        public int Id { get; set; }

        [MaxLength(250)]
        [Display(Name = "Nome Tipo Produto", ShortName = "Tipo Produto")]
        public string NomeTipoInspecao { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Descrição")]
        public string DescricaoTipoInspecao { get; set; }

        [Display(Name = "Situação")] 
        public bool IndAtivo { get; set; }
        
        
    }
}
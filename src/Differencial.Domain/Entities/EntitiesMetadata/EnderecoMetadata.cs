using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class EnderecoMetadata :BaseAuditoriaRegistroMetadata
    {
        [Display(Name ="Código Endereço D")]
        public int Id { get; set; }

        [Display(Name = "CEP")]
        [MaxLength(9)]
        public string Cep { get; set; }

        [Required]
        [Display(Name = "Logradouro")]
        [MaxLength(250)]
        public string Logradouro { get; set; }

        [Display(Name = "Número")]
        public int? Numero { get; set; }

        [Display(Name = "Complemento")]
        [MaxLength(80)]
        public string Complemento { get; set; }

        [Required]
        [Display(Name = "Bairro")]
        [MaxLength(80)]
        public string Bairro { get; set; }

        [Required]
        [Display(Name = "Nome Município")]
        [MaxLength(80)]
        public string NomeMunicipio { get; set; }

        [Required]
        [Display(Name = "Sigla UF")]
        [MaxLength(2)]
        public string SiglaUf { get; set; }

        [Display(Name ="Latitude")]
        public double? Latitude { get; set; }

        [Display(Name = "Longitude")]
        public double? Longitude { get; set; }
    }
}

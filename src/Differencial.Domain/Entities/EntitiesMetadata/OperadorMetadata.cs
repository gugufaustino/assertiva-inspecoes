using Differencial.Domain.Contracts.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class OperadorMetadata : BaseAuditoriaRegistroMetadata, IAtivavel
    {

        [Display(Name = "Código")]
        public int Id { get; set; }

        [Display(Name = "Data de Nascimento", ShortName = "Nascimento")]        
        public DateTime DataNascimento { get; set; }

        [Display(Name = "Acesso ao Sistema")]
        public bool IndAcessoSistema { get; set; }

        [Display(Name = "Situação")]
        public bool IndAtivo { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Nome Operador")]
        public string NomeOperador { get; set; }

        [MaxLength(13)]
        [Display(Name = "Telefone")]
        public string Telefone { get; set; }

        [MaxLength(50)]
        [Display(Name = "Foto")]
        public string UrlFoto { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [MaxLength(11)]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "RG")]
        public string Rg { get; set; }
 
    }
}
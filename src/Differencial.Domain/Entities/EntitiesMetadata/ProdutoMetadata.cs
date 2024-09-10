using Differencial.Domain.Contracts.Entities;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class ProdutoMetadata : BaseAuditoriaRegistroMetadata , IAtivavel
    {
        [Display(Name = "Código Produto", ShortName = "Código")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Seguradora")]
        public int IdSeguradora { get; set; }

        [Required]
        [Display(Name = "Tipo Inspeção")]
        public int IdTipoInspecao { get; set; }

        [Required]
        [Display(Name = "Nome Produto")]
        public string NomeProduto { get; set; }


        [Display(Name = "Possui Franquia em Km")]
        public bool? IndFranquiaQuilometragem { get; set; }

        [Display(Name = "Franquia de Km")]
        public int? QtdFranquiaQuilometragem { get; set; }



        [Display(Name = "Valor Despesa")]
        public decimal? VlrDespesa { get; set; }

        [Display(Name = "Valor a Receber")]
        public decimal? VlrReceber { get; set; }

        [Display(Name = "Valor Pago por Km")]
        public decimal? VlrQuilometragem { get; set; }

        [Display(Name = "Possui Bloco Extra")]
        public bool? IndBlocoExtra { get; set; }

        [Display(Name = "Valor Pago por Bloco Extra", ShortName = "Valor Receber")]
        public decimal? VlrBlocoExtr { get; set; }



        [Display(Name = "Possui Quilometragem Variavél")]
        public bool? IndQuilometragemVariavel { get; set; }

        [Display(Name = "Quilometragem Inicial", ShortName =  "Km Inicial")]
        public int? QtdQuilometragemFinal { get; set; }

        [Display(Name = "Quilometragem Final", ShortName  = "Km Final")]
        public int? QtdQuilometragemInicial { get; set; }

        [Display(Name = "Valor a Receber por Quilometragem Variavél", ShortName = "Valor Receber")]
        public decimal? VlrQuilometragemReceber { get; set; }



        [Display(Name = "Possui Metragem Variavél")]
        public bool? IndMetragemVariavel { get; set; }

        [Display(Name = "Metragem Inicial")]
        public decimal? QtdMetragemM2Inicial { get; set; }

        [Display(Name = "Metragem Final")]
        public decimal? QtdMetragemM2Final { get; set; }

        [Display(Name = "Valor a Receber sobre Metragem", ShortName ="Valor Receber")]
        public decimal? VlrMetragemReceber { get; set; }

        [Display(Name = "Situação")]
        public bool IndAtivo { get; set; }

        [Display(Name = "Código Seguradora", ShortName = "Produto Seguradora", Description = "Campo de equivalência do produto na Seguradora, utilizado também para fazer integração e relacionar registros entre sistemas diferentes")]
        public string CodProdutoSeguradora { get; set; }

        [Display(Name = "Nome Produto Seguradora", ShortName = "Produto Seguradora")]
        public string NomeProdutoSeguradora { get; set; }
    }
}

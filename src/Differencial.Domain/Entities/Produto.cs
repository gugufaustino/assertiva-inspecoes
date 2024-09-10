using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Resources;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Microsoft.AspNetCore.Mvc;
using Differencial.Domain.EntitiesMetadata;
using System.Collections.Generic;

namespace Differencial.Domain.Entities
{
    [ModelMetadataType(typeof(ProdutoMetadata))]
    public class Produto : IEntity, IAtivavel
    {
        public Produto()
        {
            VistoriadorProduto = new HashSet<VistoriadorProduto>();
            Solicitacao = new HashSet<Solicitacao>();
        }


        [Column("Id")]
        public int Id { get; set; }

        [Column("DtCadastro")]
        public DateTime DataCadastro { get; set; }

        [Column("IdOperadorCadastro")]
        public int IdOperadorCadastro { get; set; }

        [Column("DtModificacao")]
        public DateTime DataModificacao { get; set; }

        [Column("IdOperadorModificacao")]
        public int IdOperadorModificacao { get; set; }

        [Column("IndAtivo")]
        public bool IndAtivo { get; set; }

        [Column("IdSeguradora")]
        public int IdSeguradora { get; set; }

        [Column("IdTipoInspecao")]
        public int IdTipoInspecao { get; set; }

        [Column("NomeProduto")]
        public string NomeProduto { get; set; }

        [Column("IndFranquiaQuilometragem")]
        public bool IndFranquiaQuilometragem { get; set; }
        [Column("QtdFranquiaQuilometragem")]
        public int? QtdFranquiaQuilometragem { get; set; }


        [Column("VlrDespesa")]
        public decimal? VlrDespesa { get; set; }

        [Column("VlrReceber")]
        public decimal? VlrReceber { get; set; }

        [Column("VlrQuilometragem")]
        public decimal? VlrQuilometragem { get; set; }

        [Column("IndBlocoExtra")]
        public bool IndBlocoExtra { get; set; }

        [Column("VlrBlocoExtr")]
        public decimal? VlrBlocoExtr { get; set; }



        [Column("IndQuilometragemVariavel")]
        public bool IndQuilometragemVariavel { get; set; }

        [Column("QtdQuilometragemFinal")]
        public int? QtdQuilometragemFinal { get; set; }

        [Column("QtdQuilometragemInicial")]
        public int? QtdQuilometragemInicial { get; set; }

        [Column("VlrQuilometragemReceber")]
        public decimal? VlrQuilometragemReceber { get; set; }



        [Column("IndMetragemVariavel")]
        public bool IndMetragemVariavel { get; set; }

        [Column("QtdMetragemM2Inicial")]
        public decimal? QtdMetragemM2Inicial { get; set; }

        [Column("QtdMetragemM2Final")]
        public decimal? QtdMetragemM2Final { get; set; }

        [Column("VlrMetragemReceber")]
        public decimal? VlrMetragemReceber { get; set; }

        [Column("CodProdutoSeguradora")]
        public string CodProdutoSeguradora { get; set; }

        [Column("NomeProdutoSeguradora")]
        public string NomeProdutoSeguradora { get; set; }


        #region "Relacionamentos"
        [ForeignKey("IdSeguradora")]
        public virtual Seguradora Seguradora { get; set; }

        [ForeignKey("IdTipoInspecao")]
        public virtual TipoInspecao TipoInspecao { get; set; }

        public virtual ICollection<VistoriadorProduto> VistoriadorProduto { get; set; }
        public virtual ICollection<Solicitacao> Solicitacao { get; set; }

        public virtual Contrato Contrato { get; set; }

        #endregion "Relacionamentos"


        // Valida os dados da entidade
        public void Validate()
        {
            var validationResultsManager = new ValidationResultsManager();

            // Required
            if (IdSeguradora == 0)
                validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoRelacionamentoInvalido, "Seguradora", "Seguradora"));
            if (IdTipoInspecao == 0)
                validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoRelacionamentoInvalido, "Tipo Inspeção", "Tipo Inspeção"));
            if (NomeProduto.IsNullOrEmpty() || NomeProduto.Length > 250)
                validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoTamanhoMaximoInvalido, "Nome Produto", "250"));

            // Optional
            if (validationResultsManager.HasError)
                validationResultsManager.ThrowException();
        }

        // Clona os dados da entidade
        public object Clone()
        {
            var entidade = new Produto();
            entidade.IdSeguradora = this.IdSeguradora;
            entidade.IdTipoInspecao = this.IdTipoInspecao;
            entidade.IndBlocoExtra = this.IndBlocoExtra;
            entidade.IndFranquiaQuilometragem = this.IndFranquiaQuilometragem;
            entidade.IndQuilometragemVariavel = this.IndQuilometragemVariavel;
            entidade.NomeProduto = this.NomeProduto;
            entidade.QtdFranquiaQuilometragem = this.QtdFranquiaQuilometragem;
            entidade.QtdQuilometragemFinal = this.QtdQuilometragemFinal;
            entidade.QtdQuilometragemInicial = this.QtdQuilometragemInicial;
            entidade.VlrBlocoExtr = this.VlrBlocoExtr;
            entidade.VlrDespesa = this.VlrDespesa;
            entidade.VlrQuilometragem = this.VlrQuilometragem;
            entidade.VlrQuilometragemReceber = this.VlrQuilometragemReceber;
            entidade.VlrReceber = this.VlrReceber;
            entidade.IndMetragemVariavel = this.IndMetragemVariavel;
            entidade.QtdMetragemM2Inicial = this.QtdMetragemM2Inicial;
            entidade.QtdMetragemM2Final = this.QtdMetragemM2Final;
            entidade.VlrMetragemReceber = this.VlrMetragemReceber;
            return entidade;
        }
    }

    public enum CampoOrdenacaoProduto
    {
        Id,
        IdSeguradora,
        IdTipoInspecao,
        NomeProduto,
    }
}
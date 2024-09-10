using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Resources;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Differencial.Domain.EntitiesMetadata;
using Microsoft.AspNetCore.Mvc;

namespace Differencial.Domain.Entities
{
    [ModelMetadataType(typeof(VistoriadorProdutoMetadata))]
    public class VistoriadorProduto : IEntity, IAtivavel
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("DtCadastro")]
        public DateTime DataCadastro { get; set; }

        [Column("DtModificacao")]
        public DateTime DataModificacao { get; set; }

        [Column("IdOperadorCadastro")]
        public int IdOperadorCadastro { get; set; }

        [Column("IdOperadorModificacao")]
        public int IdOperadorModificacao { get; set; }

        [Column("IndAtivo")]
        public bool IndAtivo { get; set; }

        //[Index("UK_VistoriadorProduto", 1, IsUnique = true)]
        [Column("IdVistoriador")]
        public int IdVistoriador { get; set; }
        
        //[Index("UK_VistoriadorProduto", 2, IsUnique = true)]
        [Column("IdProduto")]
        public int IdProduto { get; set; }

        //[Index("UK_VistoriadorProduto", 3, IsUnique = true)]
        [Column("IdContratoLancamento")]
        public int IdContratoLancamento { get; set; }

        //[Index("UK_VistoriadorProduto", 4, IsUnique = true)]
        [Column("IdContratoLancamentoValor")]
        public int IdContratoLancamentoValor { get; set; }

        [Column("VlrPagamentoVistoria")]
        public decimal? VlrPagamentoVistoria { get; set; }

        [Column("VlrQuilometroRodado")]
        public decimal? VlrQuilometroRodado { get; set; }
                
        [ForeignKey("IdProduto")]
        public virtual Produto Produto { get; set; }

        [ForeignKey("IdVistoriador")]
        public virtual Vistoriador Vistoriador { get; set; }

        
        


        // Valida os dados da entidade
        public void Validate()
        {
            var validationResultsManager = new ValidationResultsManager();

            // Required
            if (IdVistoriador == 0)
                validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoRelacionamentoInvalido, "Vistoriador"));
            if (IdProduto == 0)
                validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoRelacionamentoInvalido, "Produto"));

            // Optional
            if (validationResultsManager.HasError)
                validationResultsManager.ThrowException();
        }

        // Clona os dados da entidade
        public object Clone()
        {
            var entidade = new VistoriadorProduto();
            entidade.IdVistoriador = this.IdVistoriador;
            entidade.IdProduto = this.IdProduto;
            entidade.VlrPagamentoVistoria = this.VlrPagamentoVistoria;
            entidade.VlrQuilometroRodado = this.VlrQuilometroRodado;
            return entidade;
        }
    }

    public enum CampoOrdenacaoVistoriadorProduto
    {
        Id,
        IdVistoriador,
        IdProduto,
        VlrPagamentoVistoria,
        VlrQuilometroRodado,
    }
}
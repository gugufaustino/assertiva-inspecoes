using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Resources;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Domain.Validation;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Differencial.Domain.Entities
{
	public partial class MovimentacaoProcesso : IWorkFlowMovimentacaoProcesso, IEntity
    {
        public MovimentacaoProcesso()
        {
            
        }

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

        [Column("TextoMovimentacao")]
        public string TextoMovimentacao { get; set; }

        [Column("TipoSituacaoProcesso")]
        public TipoSituacaoProcessoEnum TipoSituacaoProcesso { get; set; }

        [Column("IdOperadorOrigem")]
        public int IdOperadorOrigem { get; set; }

        [Column("IdOperadorDestino")]
        public int? IdOperadorDestino { get; set; }

        [Column("DthMovimentacao")]
        public DateTime DthMovimentacao { get; set; }

        [Column("DthApropriacao")]
        public DateTime? DthApropriacao { get; set; }

        [Column("DthConclusao")]
        public DateTime? DthConclusao { get; set; }

        [Column("TipoSituacaoMovimento")]
        public TipoSituacaoMovimentoEnum TipoSituacaoMovimento { get; set; }

        [Column("IdSolicitacao")]
        public int IdSolicitacao { get; set; } 

        #region "Relacionamentos"

        [ForeignKey("IdSolicitacao")]
        public virtual Solicitacao Solicitacao { get; set; }
 

        [ForeignKey("IdOperadorOrigem")]
        public virtual Operador OperadorOrigem { get; set; }

        [ForeignKey("IdOperadorDestino")]
        public virtual Operador OperadorDestino { get; set; }

        [NotMapped]
        public IWorkFlowInstanciaProcesso InstanciaProcesso
        {
            get { return this.Solicitacao; }

            set { this.Solicitacao = (Solicitacao)value; }
        }

        #endregion

        // Valida os dados da entidade
        public void Validate()
        {
            var validationResultsManager = new ValidationResultsManager();

            // Required
            if (!TextoMovimentacao.IsNullOrEmpty() && TextoMovimentacao.Length > 1000)
                validationResultsManager.AddValidationResultNotValid(String.Format(MensagensValidacao.CampoTamanhoMaximoInvalido, "TextoMovimentacao", "1000"));

            if (TipoSituacaoProcesso == 0)
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoRelacionamentoInvalido, "Tipo Situacao Processo");

            if (TipoSituacaoMovimento == 0)
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoRelacionamentoInvalido, "Tipo Situacao Movimento");

            if (IdOperadorOrigem == 0)
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoRelacionamentoInvalido, "Operador");
            if (DthMovimentacao.IsValid() == false)
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoRequeridoInvalido, "Data Movimentação");

            // Optional
            if (DthApropriacao.HasValue && DthApropriacao.Value.IsValid() == false)
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.DataInvalida, "Data Apropriação");
            if (DthConclusao.HasValue && DthConclusao.Value.IsValid() == false)
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.DataInvalida, "Data Conclusão");
            if (validationResultsManager.HasError)
                validationResultsManager.ThrowException();
        }

        // Clona os dados da entidade
        public object Clone()
        {
            var entidade = new MovimentacaoProcesso();
            entidade.TextoMovimentacao = this.TextoMovimentacao;
            entidade.TipoSituacaoProcesso = this.TipoSituacaoProcesso;
            entidade.IdOperadorOrigem = this.IdOperadorOrigem;
            entidade.IdOperadorDestino = this.IdOperadorDestino;
            entidade.DthMovimentacao = this.DthMovimentacao;
            entidade.DthApropriacao = this.DthApropriacao;
            entidade.DthConclusao = this.DthConclusao;
            return entidade;
        }
    }

    public enum CampoOrdenacaoMovimentacaoProcesso
    {

        TipoSituacaoProcesso,
        IdOperadorOrigem,
        IdOperadorDestino,
        DthMovimentacao,
        DthApropriacao,
        DthConclusao,
    }
}
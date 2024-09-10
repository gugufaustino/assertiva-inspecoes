using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Differencial.Domain;

namespace Differencial.Domain.Entities
{
    public class ArquivoAnexo : IEntity
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

        [Column("IdSolicitacao")]
		public int IdSolicitacao { get; set; }

        [Column("GuidArquivo")]
        public Guid GuidArquivo { get; set; }

        [Column("ArquivoDataModificacao")]
        public DateTime ArquivoDataModificacao { get; set; }

        [Column("ArquivoNome")]
        public string ArquivoNome { get; set; }

        [Column("ArquivoExtencao")]
        public string ArquivoExtencao { get; set; }

        [Column("ArquivoTamanho")]
        public long ArquivoTamanhoBytes { get; set; }

        [Column("Descricao")]
        public string Descricao { get; set; } 

        [Column("ArquivoAnexoPosicao")]
        public int ArquivoAnexoPosicao { get; set; }

        [Column("IndExcluida")]
        public bool IndExcluida { get; set; }

        [Column("TipoArquivoAnexo")]
        public TipoArquivoAnexoEnum TipoArquivoAnexo { get; set; }


        [ForeignKey("IdSolicitacao")]
        public virtual Solicitacao Solicitacao { get; set; }

        public virtual LaudoFoto LaudoFoto { get; set; }

        // Valida os dados da entidade
        public void Validate()
		{
			 
		}

		// Clona os dados da entidade
		public object Clone()
		{
			var entidade = new ArquivoAnexo();
			entidade.IdSolicitacao = this.IdSolicitacao;
			return entidade;
		}
	}

	public enum CampoOrdenacaoArquivoAnexo
	{
	}
}
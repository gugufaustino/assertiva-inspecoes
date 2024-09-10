using Differencial.Domain.Contracts.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Microsoft.AspNetCore.Mvc;
using Differencial.Domain.EntitiesMetadata;

namespace Differencial.Domain.Entities
{
    [ModelMetadataType(typeof(LogAuditoriaMetadata))]
    public class LogAuditoria : IEntity
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("Tabela")]
        public string Tabela { get; set; }

        [Column("IdTabela")]
        public int IdTabela { get; set; }

        [Column("XMLDadosAnterior", TypeName = "xml")]
        public string XMLDadosAnterior { get; set; }

        [Column("XMLDadosPosterior", TypeName = "xml")]
        public string XMLDadosPosterior { get; set; }

        [Column("UsuarioAplicacao")]
        public int UsuarioAplicacao { get; set; }

        [Column("Acao")]
        public string Acao { get; set; }

        [Column("DataAcao")]
        public DateTime DataAcao { get; set; }

        [Column("UsuarioBanco")]
        public string UsuarioBanco { get; set; }

        [NotMapped]
        public DateTime DataCadastro { get; set; }

        [NotMapped]
        public DateTime DataModificacao { get; set; }

        [NotMapped]
        public int IdOperadorCadastro { get; set; }

        [NotMapped]
        public int IdOperadorModificacao { get; set; }

        [ForeignKey("UsuarioAplicacao")]
        public virtual Operador Operador { get; set; }

        // Valida os dados da entidade
        public void Validate()
        {

        }

        // Clona os dados da entidade
        public object Clone()
        {
            var entidade = new LogAuditoria();
            entidade.Id = this.Id;
            entidade.Tabela = this.Tabela;
            entidade.XMLDadosAnterior = this.XMLDadosAnterior;
            entidade.XMLDadosPosterior = this.XMLDadosPosterior;
            entidade.UsuarioAplicacao = this.UsuarioAplicacao;
            entidade.Acao = this.Acao;
            entidade.DataAcao = this.DataAcao;
            entidade.UsuarioBanco = this.UsuarioBanco;
            return entidade;
        }
         
        public override bool Equals(object objs)
        {
            var b = (LogAuditoria)objs;
            return Id == b.Id &&
                    Tabela == b.Tabela &&
                    XMLDadosAnterior == b.XMLDadosAnterior &&
                    XMLDadosPosterior == b.XMLDadosPosterior &&
                    UsuarioAplicacao == b.UsuarioAplicacao &&
                    Acao == b.Acao &&
                    DataAcao == b.DataAcao &&
                    UsuarioBanco == b.UsuarioBanco;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public enum CampoOrdenacaoLogAuditoria
    {
        Id,
        Tabela,
        IdTabela,
        Acao,
        DataAcao
    }
}
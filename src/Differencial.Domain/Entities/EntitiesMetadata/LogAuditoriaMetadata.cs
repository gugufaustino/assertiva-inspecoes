using System;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class LogAuditoriaMetadata : BaseAuditoriaRegistroMetadata
    {
        [Display(Name = "Código Log Auditoria", ShortName = "Código")]
        public int  Id { get; set; }
 
        [Display(Name = "Nome da Entidade")]
        public string Tabela { get; set; }

        [Display(Name = "Código")]
        public int IdTabela { get; set; }

        [Display(Name = "Dados Anterior")]
        public string XMLDadosAnterior { get; set; } 

        [Display(Name = "Dados Posterior" )]
        public string XMLDadosPosterior { get; set; }

        [Display(Name = "Operador do Sistema")]
        public string UsuarioAplicacao { get; set; }

        [Display(Name = "Operação Realizada")]
        public string Acao { get; set; }

        [Display(Name = "Data da Operação")]
        public DateTime DataAcao { get; set; }

        [Display(Name = "Usuario de Banco de Dados")]
        public string UsuarioBanco { get; set; }
    }
}

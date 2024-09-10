using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Entities;
using System;

namespace Differencial.Domain.Filters
{
    public class LogAuditoriaFilter : Pagination, IPagination
    {
        public int? IdTabela { get; set; }

        public string Tabela { get; set; }

        public int? UsuarioAplicacao { get; set; }

        public string Acao { get; set; }

        public CampoOrdenacaoLogAuditoria CampoOrdenacao { get; private set; }

        public LogAuditoriaFilter OrderByField(CampoOrdenacaoLogAuditoria campoOrdenacao, Order? order = null)
        {
            OrderField = campoOrdenacao.ToString();
            this.Order = order ?? this.Order;
            this.CampoOrdenacao = campoOrdenacao;
            return this;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differencial.Domain.DTO
{
   public class MovimentoSinteticoDTO
    {
        public int Id { get; set; } 
        public int IdSolicitacao { get; set; }
        public int CodSistemaLegado { get; set; }
        public int TipoMovimentoSintetico { get; set; }         
        public decimal ValorMovimentoSintetico { get; set; } 
        public int Dia { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public string SituacaoInspecao { get; set; }
        public string DataCadastroInspecao { get; set; }
        public string NomeCliente { get; set; }
        public string NomeFavorecido { get; set; }
        public string CpfCnpjeFavorecido { get; set; }
        public string DataCobranca { get; set; }
        public int CodSituacaoInspecao { get; set; }
    }
}

using Differencial.Domain.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differencial.Domain.Queries.Dao
{
    public class SolicitacaoCobrancaVistoriaDao : SolicitacaoRootDao
    {
        public string Cep { get; set; }
        public string CodSeguradora { get; set; }
        public string Complemento { get; set; }
        public string EmailOperador { get; set; }
        public string  Logradouro { get; set; }
        public string NomeMunicipio { get; set; }
        public string NomeRazaoSocial { get; set; }
        public int? Numero { get; set; }
        public string SiglaUf { get; set; }
        public DateTime? ControleDthEmailCobrancaVistoria { get; set; }
        public string Bairro { get; set; }
    }
}

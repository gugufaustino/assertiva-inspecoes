using Differencial.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace Differencial.Domain.DTO
{
    public class UsuarioLogadoDTO 
    {
        public int Id { get; set; }
        public bool IndGerente { get;  set; }
        public bool IndVistoriador { get;  set; }
        public bool IndAnalista { get;  set; }
        public bool IndFinanceiro { get;  set; }
        public bool IndSolicitante { get;  set; }
        public bool IndAssessor { get;  set; }
        public List<TipoPapelEnum>  TipoPapel { get;  set; }
        public string NomeOperador { get; set; }
        public string UrlFoto { get;  set; }
        public string NomeSeguradoraSolicitante { get; set; }

        public string ConfigMenuColapso { get; set; }

        public bool IndUsuarioSistema { get; set; }

    }
}
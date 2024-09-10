using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differencial.Domain
{
    public enum TipoSolicitanteEnum
    {
        /// <summary>
        /// Esse tipo de solicitante fazer parte do cadastro mas não realiza logon como operador. Esse registro é suprimido da lista de operadores
        /// </summary>
        [Display(Name = "Cadastro (sem acesso ao sistema)")]
        Cadastro = 0,

        /// <summary>
        /// Solicitante cadastrado como operador do sistema
        /// </summary>
        [Display(Name = "Operador (com acesso ao sistema)")]
        AcessoAoSistema = 1
    }
}

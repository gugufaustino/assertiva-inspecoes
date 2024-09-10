using System;

namespace Differencial.Domain.DTO
{
    public class NotificacaoDTO
    { 
        public string mensagem { get; set; }
        public DateTime data { get; set; }
        public int? cod { get; set; }
        public bool indLido { get; set; }
       
    }
}

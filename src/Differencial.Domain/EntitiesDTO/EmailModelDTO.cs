using System;

namespace Differencial.Domain.DTO
{
    public class EmailModelDTO
    {
        public string id { get; set; }
        public string assunto { get; set; }
        public string remetente { get; set; }
        public string fragmento { get; set; }
        public DateTime data { get; set; }
        public string corpoHtml { get; set; }
        public string corpoTexto { get; set; }
        public byte[] corpoByte { get; set; }
    }
}

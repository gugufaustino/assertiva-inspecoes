using System;

namespace Differencial.Web.Models
{
    public class QuadroFotoModel
    {
        public int IdSolicitacao { get; set; }
        public int Id { get; set; }
        public DateTime ArquivoDataModificacao { get; set; }
        public string ArquivoNome { get; set; }
        public string ArquivoExtencao { get; set; }
        public long ArquivoTamanhoBytes { get; set; }
        public int fotoAnsNumero { get; set; }
        public Guid GuidFoto { get; set; }
        public string ImgData { get; set; }
        public int QuadroFotosPosicao { get; set; }
    }
}
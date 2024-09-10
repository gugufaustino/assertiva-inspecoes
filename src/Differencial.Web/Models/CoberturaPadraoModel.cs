using System.ComponentModel.DataAnnotations;

namespace Differencial
{
    public class CoberturaPadraoModel
    {
        public int? Id { get; set; }
        public string NomeCobertura { get; set; }
        public bool IndPadrao { get; set; }
        public decimal? VlrCobertura { get; set; }
    }
}
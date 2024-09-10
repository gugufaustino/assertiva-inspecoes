
namespace Differencial.Domain.Filters
{
    public class FiltroGrade
    {
        public string Campo { get; set; }
        public string Campo2 { get; set; }
        public string Valor { get; set; }
        public string Valor2 { get; set; }
        public FiltroTipo Tipo { get; set; }
        public FiltroTipo Tipo2 { get; set; }
    }
    
    public enum FiltroTipo
    {
        Igual = 0,
        MenorQue = 1,
        MaiorQue,
        Entre,
        MaiorIgualQue,
        MenorIgualQue
    }
}

namespace Differencial.Web.DTO
{
    public class ResponseResultDTO
    {
        public bool success { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public bool showMessage { get; set; }
        public string url { get; set; }
        public object content { get; set; }
        public TipoResponseResultEnum TipoResponseResult { get; set; }
    }

    public enum TipoResponseResultEnum
    {
        Sucesso,
        Erro,
        Informacao,
        Atencao
    }
}
namespace Differencial.Domain.Util.ExtensionMethods
{
    public static class IntegerExtension
    {
        /// <summary>
        /// Formata valor inteiro nullable vazio para Moeda
        /// </summary>
        /// <param name="valor">Valor</param>
        /// <returns>Formato 5 Ex:"5,00"</returns>
        public static string FormatoMoeda(this int? valor)
        {
            return valor == null ? "" : valor.Value.ToString("N");
        }

        /// <summary>
        /// Formata valor inteiro para moeda
        /// </summary>
        /// <param name="valor">Valor</param>
        /// <returns>Formato 5 Ex:"5,00"</returns>
        public static string FormataMoeda(this int valor)
        {
            return valor.ToString("N");
        }


        /// <summary>
        /// Formata o numero do processo
        /// </summary>
        /// <param name="valor">Valor</param>
        /// <returns>Formato xxxxx/xxxx</returns>
        public static string FormatarNumeroProcesso(this int valor)
        {
            return valor.ToString().Insert(5, "/");
        }

    }
}
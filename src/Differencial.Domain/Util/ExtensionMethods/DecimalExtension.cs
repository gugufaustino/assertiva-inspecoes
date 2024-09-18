namespace Differencial.Domain.Util.ExtensionMethods
{
    public static class DecimalExtension
    {
        /// <summary>
        /// Formata valor decimal nullable vazio para Moeda
        /// </summary>
        /// <param name="valor">Valor</param>
        /// <returns>Formato 5 Ex:"5,00"</returns>
        public static string FormatoMoeda(this decimal? valor)
        {
            return valor == null ? "" : valor.Value.ToString("N2");
        }

        /// <summary>
        /// Formata valor decimal para moeda
        /// </summary>
        /// <param name="valor">Valor</param>
        /// <returns>Formato 5 Ex:"5,00"</returns>
        public static string FormataMoeda(this decimal valor)
        {
            return valor.ToString("N2");
        }
    }
}
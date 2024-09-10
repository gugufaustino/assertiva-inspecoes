namespace Differencial.Domain.Util.ExtensionMethods
{
    public static class BooleanExtension
    {
        /// <summary>
        /// Retorna valor "Sim" para true e "Não" para false
        /// </summary>
        /// <param name="item">Valor booleano</param>
        /// <returns>String Sim ou Não</returns>
        public static string ToSimNao(this bool item)
        {
            return item ? "Sim" : "Não";
        }
    }
}
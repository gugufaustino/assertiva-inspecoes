using System;

namespace Differencial.Domain.Util.ExtensionMethods
{
    public static class TimeSpanExtension
    {
        /// <summary>
        /// Formata o TimeSpan nullable passado para Hora:Minutos
        /// </summary>
        /// <param name="time">Objeto</param>
        /// <returns>Formato Ex:"10:30" ou "" caso seja null</returns>
        public static string FormatoHora(this TimeSpan? time)
        {
            return time == null ? "" : time.Value.ToString("hh':'mm");
        }

        /// <summary>
        /// Formata o TimeSpan passado para Hora:Minutos
        /// </summary>
        /// <param name="time">Objeto</param>
        /// <returns>Formato Ex:"10:30"</returns>
        public static string FormatoHora(this TimeSpan time)
        {
            return time.ToString("hh':'mm");
        }
    }
}
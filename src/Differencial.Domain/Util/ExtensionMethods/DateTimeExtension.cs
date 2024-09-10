using System;

namespace Differencial.Domain.Util.ExtensionMethods
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// Verifica se a data está dentro do intervalo de outras duas datas
        /// </summary>
        /// <param name="dt">Objeto</param>
        /// <param name="rangeBeg">Data de início</param>
        /// <param name="rangeEnd">Data de fim</param>
        /// <returns>true caso esteja no intervalo</returns>
        public static bool Between(this DateTime dt, DateTime rangeBeg, DateTime rangeEnd)
        {
            return dt.Ticks >= rangeBeg.Ticks && dt.Ticks <= rangeEnd.Ticks;
        }

        /// <summary>
        /// Calcula a idade a partir da data atual
        /// </summary>
        /// <param name="dateTime">Objeto</param>
        /// <returns>Idade em anos</returns>
        public static int CalculateAge(this DateTime dateTime)
        {
            var age = DateTime.Now.Year - dateTime.Year;
            if (DateTime.Now < dateTime.AddYears(age))
                age--;

            return age;
        }

        /// <summary>
        /// Verifica se é dia trabalhado (Não contempla feriados)
        /// </summary>
        /// <param name="date">Objeto</param>
        /// <returns>true caso seja dia de semana</returns>
        public static bool WorkingDay(this DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
        }

        /// <summary>
        /// Verifica se é fim de semana
        /// </summary>
        /// <param name="date">Objeto</param>
        /// <returns>true caso seja sábado ou domingo</returns>
        public static bool IsWeekend(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Busca o próximo dia de semana
        /// </summary>
        /// <param name="date">Objeto</param>
        /// <returns>o próximo dia de semana</returns>
        public static DateTime NextWorkday(this DateTime date)
        {
            var nextDay = date;
            while (!nextDay.WorkingDay())
            {
                nextDay = nextDay.AddDays(1);
            }

            return nextDay;
        }

        /// <summary>
        /// Busca a próxima data do dia da semana passado
        /// </summary>
        /// <param name="current">Data atual</param>
        /// <param name="dayOfWeek">Dia da semana</param>
        /// <returns>próximo dia da semana</returns>
        public static DateTime Next(this DateTime current, DayOfWeek dayOfWeek)
        {
            int offsetDays = dayOfWeek - current.DayOfWeek;
            if (offsetDays <= 0)
            {
                offsetDays += 7;
            }
            DateTime result = current.AddDays(offsetDays);

            return result;
        }

        /// <summary>
        /// Verifica se a data não é MinValue ou MaxValue
        /// </summary>
        /// <param name="current">Objeto</param>
        /// <returns>true caso seja válida</returns>
        public static bool IsValid(this DateTime current)
        {
            return current != DateTime.MinValue && current != DateTime.MaxValue;
        }

        /// <summary>
        /// Formata data nullable para dd/MM/yyyy
        /// </summary>
        /// <param name="dateTime">Objeto</param>
        /// <returns>Formata Ex:"30/12/2012" ou "" caso vazio</returns>
        public static string FormatoData(this DateTime? dateTime)
        {
            return dateTime == null ? "" : dateTime.Value.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Formata data para dd/MM/yyyy
        /// </summary>
        /// <param name="dateTime">Objeto</param>
        /// <param name="mostrarNull"></param>
        /// <returns>Formata Ex:"30/12/2012"</returns>
        public static string FormatoData(this DateTime dateTime, bool mostrarNull = false)
        {
            if (mostrarNull)
                return dateTime == DateTime.MinValue ? "" : dateTime.ToString("dd/MM/yyyy");
            return dateTime.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Formata data hora para dd/MM/yyyy HH:mm
        /// </summary>
        /// <param name="dateTime">Objeto</param>
        /// <returns>Formata Ex:"30/12/2012 10:30"</returns>
        public static string FormatoDataHora(this DateTime? dateTime)
        {
            return dateTime == null ? "" : dateTime.Value.ToString("dd/MM/yyyy HH:mm");
        }

        /// <summary>
        /// Formata data hora para dd/MM/yyyy HH:mm
        /// </summary>
        /// <param name="dateTime">Objeto</param>
        /// <returns>Formata Ex:"30/12/2012 10:30"</returns>
        public static string FormatoDataHora(this DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy HH:mm");
        }

        /// <summary>
        /// Formata hora nullable para HH:mm
        /// </summary>
        /// <param name="dateTime">Objeto</param>
        /// <returns>Formata Ex:"10:30"</returns>
        public static string FormatoHora(this DateTime? dateTime)
        {
            return dateTime == null ? "" : dateTime.Value.ToString("HH:mm");
        }

        /// <summary>
        /// Formata hora para HH:mm
        /// </summary>
        /// <param name="dateTime">Objeto</param>
        /// <returns>Formata Ex:"10:30"</returns>
        public static string FormatoHora(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm");
        }


        /// <summary>
        /// Adiciona dias na data contando somente os dias de semana.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="businessDays">Numero de dias.</param>
        /// <returns>data</returns>
        public static DateTime AddBusinessDays(this DateTime source, int businessDays)
        {
            var dayOfWeek = businessDays < 0
                                ? ((int)source.DayOfWeek - 12) % 7
                                : ((int)source.DayOfWeek + 6) % 7;

            switch (dayOfWeek)
            {
                case 6:
                    businessDays--;
                    break;
                case -6:
                    businessDays++;
                    break;
            }

            return source.AddDays(businessDays + ((businessDays + dayOfWeek) / 5) * 2);
        }
    }
}
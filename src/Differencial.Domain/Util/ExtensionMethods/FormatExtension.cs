using System;

namespace Differencial.Domain.Util.ExtensionMethods
{
    public static class FormatExtension
    {
        public static string FormatCpfCnpj(this string cpfCnpj)
        {
            if (!string.IsNullOrEmpty(cpfCnpj))
            {
                cpfCnpj = cpfCnpj.Replace("/", "").Replace(".", "").Replace("-", "");
                if (cpfCnpj.Length == 11)
                    return Convert.ToUInt64(cpfCnpj).ToString(@"000\.000\.000\-00");

                return Convert.ToUInt64(cpfCnpj).ToString(@"00\.000\.000\/0000\-00");
            }

            return string.Empty;
        }

        public static string TruncateText(this string text, int maxSize)
        {
            if (!string.IsNullOrEmpty(text) && text.Length > maxSize)
            {
                return string.Concat(text.Substring(0, maxSize), "...");
            }

            return text;
        }

    }
}
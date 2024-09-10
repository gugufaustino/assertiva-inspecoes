using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Differencial.Domain.Util.ExtensionMethods
{
    public static class StringExtension
    {
        /// <summary>
        /// Verifica se a string está null ou vazia
        /// </summary>
        /// <param name="str">Objeto</param>
        /// <returns>true caso null ou vazia</returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Remove o último caractere
        /// </summary>
        /// <param name="instr">Objeto</param>
        /// <returns>string sem o último caractere</returns>
        public static string RemoveLastCharacter(this String instr)
        {
            return instr.Substring(0, instr.Length - 1);
        }

        /// <summary>
        /// Remove o numero de caracteres passado a partir do ultimo
        /// </summary>
        /// <param name="instr">Objeto</param>
        /// <param name="number">Posição</param>
        /// <returns>string sem os caracteres</returns>
        public static string RemoveLast(this String instr, int number)
        {
            return instr.Substring(0, instr.Length - number);
        }

        /// <summary>
        /// Remove o primeiro caractere
        /// </summary>
        /// <param name="instr">Objeto</param>
        /// <returns>string sem o primeiro caractere</returns>
        public static string RemoveFirstCharacter(this String instr)
        {
            return instr.Substring(1);
        }

        /// <summary>
        /// Remove o numero de caracteres passado a partir do primeiro
        /// </summary>
        /// <param name="instr">Objeto</param>
        /// <param name="number">Posição</param>
        /// <returns>string sem os caracteres</returns>
        public static string RemoveFirst(this String instr, int number)
        {
            return instr.Substring(number);
        }

        /// <summary>
        /// Valida se email está válido
        /// </summary>
        /// <param name="s">Objeto</param>
        /// <returns>true se email é válido</returns>
        public static bool IsValidEmailAddress(this String s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }

        /// <summary>
        /// Converte string para stream
        /// </summary>
        /// <param name="str">Objeto</param>
        /// <returns>objeto stream</returns>
        public static Stream ToStream(this String str)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(str);
            return new MemoryStream(byteArray);
        }

        /// <summary>
        /// Cria estrutura de alerta para mensagem na tela
        /// </summary>
        /// <param name="mensagem">mensagem</param>
        /// <returns></returns>
        public static string ConverterScript(this string mensagem)
        {
            return string.Format(@"<script>$(function(){{ alert('{0}');}});</script>", mensagem.Replace("\n", "</br>"));
        }

        /// <summary>
        /// Remove todos os caracteres não numericos de uma string
        /// </summary>
        /// <param name="str">texto a ser limpo</param>
        /// <returns></returns>
        public static string RemoveNonNumbers(this String str)
        {
            char[] ca = str.Where(char.IsNumber).ToArray();
            return new string(ca);
        }

        /// <summary>
        /// Método para reduzir tamanho de string, acrescentando os ... (reticência)
        /// </summary>
        /// <param name="str">O texto</param>
        /// <param name="quantidadeCaracteres">A quantidade de caracteres</param>
        /// <returns>O texto modificado</returns>
        public static string ReduzirTamanhoTexto(this string str, int quantidadeCaracteres)
        {
            if (!String.IsNullOrEmpty(str))
            {
                if (str.Length > quantidadeCaracteres)
                    return str.Substring(0, quantidadeCaracteres) + "...";
            }
            return str;
        }

        public static string RemoverMascaraTelefoneCelular(this string str)
        {
            if (str.IsNullOrEmpty() == false)
            {
                str = str.Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", string.Empty);
            }
            return str;
        }
        public static string RemoverMascaraCPFCNPJ(this string str)
        {
            if (str.IsNullOrEmpty() == false)
            {
                str = str.Replace(".", string.Empty).Replace("/", string.Empty).Replace("-", string.Empty);
            }
            return str;
        }

        public static bool ToBool(this string str)
        {
            if (str.IsNullOrEmpty() == false)
            {
                if (str.ToLower() == "true")
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Formata string hora para xxxxxx-xxx
        /// </summary>
        /// <param name="cep">Objeto</param>
        /// <returns>Formata Ex:"91350-222"</returns>
        public static string FormatoCEP(this string cep)
        {
            cep = cep.Replace("-", "");
            return String.IsNullOrEmpty(cep) || cep.Length != 8 ? "" : cep.Substring(0, 5) + "-" + cep.Substring(5, 3);
        }

        /// <summary>
        /// Formata string hora para xxxxxx-xxx
        /// </summary>
        /// <param name="cep">Objeto</param>
        /// <returns>Formata Ex:"91350-222"</returns>
        public static string RemoverMascaraCEP(this string cep)
        {
            if (cep.IsNullOrEmpty() == false)
                cep = cep.Replace("-", "");

            return cep.Replace("-", "");
        }


        /// <summary>
        /// Removers os zeros do inicio da string.
        /// </summary>
        /// <param name="valor">The valor</param>
        /// <returns></returns>
        public static string RemoverZerosDoInicio(this string valor)
        {
            if (valor.IsNullOrEmpty() == false)
            {
                valor = valor.TrimStart('0');
            }

            return valor;
        }

        /// <summary>
        /// Verifica se um objeto do banco é igual ao objeto informado pelo usuário.Ignorando acentuação letras maiúsculas ou minúsculas
        /// </summary>        
        /// <param name="valor">Valor da string</param>
        /// <param name="strComparar">Valor informado pelo usuário</param>
        /// <param name="removerAcentos">Valor que define se serão removidas as acentuações</param>
        /// <returns>Booleano</returns>
        public static bool CompareInsensitive(this string valor, string strComparar, bool removerAcentos = false)
        {
            if (valor.IsNullOrEmpty()) return false;
            if (strComparar.IsNullOrEmpty()) return false;

            strComparar = strComparar.Igualar();

            return removerAcentos
                ? (valor).Igualar().Contains(strComparar)
                : (valor).Trim().ToUpper().Contains(strComparar);
        }

        public static String Igualar(this String strCorrente)
        {
            return strCorrente.Trim().RemoveAccents().ToUpper();
        }

        /// <summary>
        /// Remove a acentuação de uma String
        /// </summary>
        /// <param name="text"></param>
        /// <returns>String</returns>
        public static string RemoveAccents(this string text)
        {
            if (text != null)
            {
                StringBuilder sbReturn = new StringBuilder();
                var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();

                foreach (char letter in arrayText)
                {
                    if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                        sbReturn.Append(letter);
                }
                return sbReturn.ToString();
            }

            return String.Empty;
        }

        public static string ValorEntreCaracter(this string texto, string caracterAbre, string caracterFecha)
        {

            texto = texto.Substring(texto.IndexOf(caracterAbre) + caracterAbre.Length);
            if (texto.IndexOf(caracterFecha) == -1)
                throw new Exception("Não encontrato caracter que fecha o limite da busca");
            texto = texto.Substring(0, texto.IndexOf(caracterFecha));
            return texto;
        }



        public static string Formata(this string texto, string args)
        {
            return string.Format(texto, args);
        }
        public static string Formata(this string texto, params string[] args)
        {
            return string.Format(texto, args);
        }

        /// <summary>
        /// Retorna valor sem espaços no final ou seu proprio valor no caso de nulo. Há tatamento de Null para não gerar excessão.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string TrimDefault(this string texto)
        {
            return texto.IsNullOrEmpty() ? texto : texto.Trim();
        }

    }
}
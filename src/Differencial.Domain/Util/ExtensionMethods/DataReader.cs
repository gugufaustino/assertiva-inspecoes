using System.Data;

namespace Differencial.Domain.Util.ExtensionMethods
{
    public static class DataReader
    {
        /// <summary>
        /// Método para verificar se uma coluna existe em um Data Reader
        /// </summary>
        /// <param name="reader">Data Reader utilizado</param>
        /// <param name="columnName">Nome da coluna</param>
        /// <returns>True caso exista e False caso não exista</returns>
        public static bool ColumnExists(this IDataReader reader, string columnName, bool caseSensitive = true)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (caseSensitive)
                {

                    if (reader.GetName(i) == columnName)
                        return true;
                }
                else
                {
                    if (reader.GetName(i).ToLower() == columnName.ToLower())
                        return true;
                }
            }

            return false;
        }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Differencial.Domain.Util.ExtensionMethods
{
    public static class StreamExtension
    {
        /// <summary>
        /// Método para ler um Stream e retornar o texto
        /// </summary>
        /// <param name="stream">Objeto</param>
        /// <returns>texto lido</returns>
        public static string ToString(this Stream stream)
        {
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Copia um stream para outro.
        /// Exemplo:
        /// using(var stream = response.GetResponseStream())
        /// using(var ms = new MemoryStream())
        /// {
        ///     stream.CopyTo(ms);
        ///      // Do something
        /// }
        /// </summary>
        /// <param name="fromStream">Do stream.</param>
        /// <param name="toStream">Para stream.</param>
        public static void CopyTo(this Stream fromStream, Stream toStream)
        {
            if (fromStream == null)
                throw new ArgumentNullException(nameof(fromStream));
            if (toStream == null)
                throw new ArgumentNullException(nameof(toStream));
            var bytes = new byte[8092];
            int dataRead;
            while ((dataRead = fromStream.Read(bytes, 0, bytes.Length)) > 0)
                toStream.Write(bytes, 0, dataRead);
        }

        public static byte[] ToByteArray(this Stream stream)
        {
            stream.Seek(0L, SeekOrigin.Begin);
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }


        public static async Task Salvar(this IFormFile arquivo, string path, string filename)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using var file = new FileStream(Path.Combine(path, filename), FileMode.Create);
            file.Seek(0L, SeekOrigin.Begin);
			 

			 await arquivo.CopyToAsync(file);

        }
    }
}
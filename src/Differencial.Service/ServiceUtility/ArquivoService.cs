using Microsoft.AspNetCore.Http;
using System.IO;

namespace Differencial.Service.Util
{
    public static class ArquivoService
    {

        public static void Excluir(string caminhoarquivo)
        {
            File.Delete(caminhoarquivo); 
        } 
        public static void Salvar(Stream arquivo, string diretorio,  string nomearquivo)
        {
            if (!Directory.Exists(diretorio))
                Directory.CreateDirectory(diretorio);

            using (var fileStream = File.Create(diretorio + nomearquivo))
            {
                arquivo.Seek(0, SeekOrigin.Begin);
                arquivo.CopyTo(fileStream);
            }
            arquivo.Seek(0, SeekOrigin.Begin);
        } 
   
    }
}

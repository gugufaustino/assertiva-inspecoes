using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;


namespace Differencial.Web.Generico
{
    public class UtilWeb
    {
        
       

        public UtilWeb()
        {

        }


        public static UsuarioLogadoDTO UsuarioLogado
        {
            get
            {
                var usuarioService = (IUsuarioService)HttpContextHelper.Current.RequestServices.GetService(typeof(IUsuarioService));
                return usuarioService.UsuarioAutenticado;

            }
        }

        public static string ImagemBase64(string arquivoUri)
        {
            try
            {

                return "data:image/gif;base64," + Convert.ToBase64String(File.ReadAllBytes(arquivoUri));
            }
            catch (Exception)
            {

            }
            return "";
        }

        public static string ImagemBase64FromCache(string arquivoUri)
        {
            try
            {
                var cache = (ICache)HttpContextHelper.Current.RequestServices.GetService(typeof(ICache));

                string img = cache.Buscar(arquivoUri) as string;
                if (img == null)
                {
                    img = ImagemBase64(arquivoUri);
                    cache.Definir(arquivoUri, img);
                }

                return img;
            }
            catch (Exception)
            {

            }
            return "";
        }

        public static void RedirecionarNaoAutenticado()
        {

            if (!HttpContextHelper.Current.User.Identity.IsAuthenticated)
            {
                HttpContextHelper.Current.Response.Redirect(@"~/Home/Login");
                //System.Web.HttpUtility.UrlEncode(context.Request.QueryString.ToUriComponent());
            }
        }

    }
}
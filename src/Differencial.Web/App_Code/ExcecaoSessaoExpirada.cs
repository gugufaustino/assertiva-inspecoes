using System;
using Microsoft.AspNetCore.Http;

namespace Differencial.Web.Generico
{
    [Serializable]
    class ExcecaoSessaoExpirada : Exception
    {
        public ExcecaoSessaoExpirada()
        {
            throw new NotImplementedException();
            //if (HttpContextHelper.Current != null)
            //{
            //    HttpContextHelper.Current.Response.Redirect(@"~/Home/Login?ReturnUrl=" +
            //                                                     System.Web.HttpUtility.UrlEncode(
            //                                                         HttpContextHelper.Current.Request.RawUrl));
            //    HttpContextHelper.Current.ApplicationInstance.CompleteRequest();
            //}
        }

    }
}

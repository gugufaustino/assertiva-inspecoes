using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Differencial.Web.Helpers
{
    public class JsonNetResult : JsonResult
    {

        public JsonNetResult(object value)
            : base(value)
        {

        }

        //public override void ExecuteResult(ControllerContext context)
        //{
        //    if (context == null)
        //        throw new ArgumentNullException("context");
        //    if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
        //        throw new InvalidOperationException("JSON GET is not allowed");

        //    HttpResponseBase response = context.HttpContext.Response;
        //    response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;

        //    if (this.ContentEncoding != null)
        //        response.ContentEncoding = this.ContentEncoding;
        //    if (this.Data == null)
        //        return;

        //    var Settings = new JsonSerializerSettings
        //    {
        //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        //        Culture = System.Globalization.CultureInfo.CurrentCulture,
        //        Converters = new List<JsonConverter> { new DecimalDoubleConverter() }
        //    };

        //    var scriptSerializer = JsonSerializer.Create(Settings);

        //    using (var sw = new System.IO.StringWriter())
        //    {
        //        scriptSerializer.Serialize(sw, this.Data);
        //        response.Write(sw.ToString());
        //    }
        //}


    }

    public class DecimalDoubleJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(decimal) || objectType == typeof(decimal?))
                || (objectType == typeof(double) || objectType == typeof(double?));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Float || token.Type == JTokenType.Integer)
            {
                return token.ToObject<double>();
            }
            else if (token.Type == JTokenType.String && (objectType == typeof(Double) || objectType == typeof(double)))
            {
                return Double.Parse(token.ToString(), System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));
            }
            else if (token.Type == JTokenType.Null && objectType == typeof(decimal?))
            {
                return null;
            }
            throw new JsonSerializationException("Unexpected token type: " + token.Type.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value.GetType() == typeof(double) || value.GetType() == typeof(double?))
            {
                writer.WriteValue(((double)value).ToString(new System.Globalization.CultureInfo("pt-BR")));
            }
            else if (value.GetType() == typeof(decimal) || value.GetType() == typeof(decimal?))
            {
                writer.WriteValue(((decimal)value).ToString(new System.Globalization.CultureInfo("pt-BR")));
            }
            else
            {
                throw new NotImplementedException("DecimalDoubleConverter.WriteJson: Tipo Não implementado para " + value.GetType());
            }

        }
    }

}
using Differencial.Domain;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Domain.Annotation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


namespace Differencial.Web.Helpers
{
    public static class HtmlModelExtensions
    {
        #region "Metodos Auxiliares"
        public static T GetAttributeFrom<T>(this object instance, string propertyName) where T : Attribute
        {
            var property = instance.GetType().GetProperty(propertyName);
            T t = (T)property.GetCustomAttributes(typeof(T), false).FirstOrDefault();
            if (t == null)
            {
                MetadataTypeAttribute[] metaAttr = (MetadataTypeAttribute[])instance.GetType().GetCustomAttributes(typeof(MetadataTypeAttribute), true);
                if (metaAttr.Length > 0)
                {
                    foreach (MetadataTypeAttribute attr in metaAttr)
                    {
                        var subType = attr.MetadataClassType;
                        var pi = subType.GetProperty(propertyName);
                        if (pi != null)
                        {
                            t = (T)pi.GetCustomAttributes(typeof(T), false).FirstOrDefault();
                            return t;
                        }
                    }
                }
            }
            else
            {
                return t;
            }
            return null;
        }
        public static T GetAttributeFrom<T>(this Type instance, string propertyName) where T : Attribute
        {
            var property = instance.GetProperty(propertyName);
            T t = (T)property.GetCustomAttributes(typeof(T), false).FirstOrDefault();
            if (t == null)
            {
                MetadataTypeAttribute[] metaAttr = (MetadataTypeAttribute[])instance.GetCustomAttributes(typeof(MetadataTypeAttribute), true);
                if (metaAttr.Length > 0)
                {
                    foreach (MetadataTypeAttribute attr in metaAttr)
                    {
                        var subType = attr.MetadataClassType;
                        var pi = subType.GetProperty(propertyName);
                        if (pi != null)
                        {
                            t = (T)pi.GetCustomAttributes(typeof(T), false).FirstOrDefault();
                            return t;
                        }
                    }
                }
            }
            else
            {
                return t;
            }
            return null;
        }
        public static TAttribute RetornaAttribute<TAttribute, TModel, TProperty>(this ViewDataDictionary<TModel> viewdata, Expression<Func<TModel, TProperty>> expression, IHtmlHelper<TModel> helper)
          where TAttribute : Attribute
        {
            var metaData = modelExpressionProvider(helper).CreateModelExpression(viewdata, expression).Metadata;

            var name = (((MemberExpression)expression.Body).Member as PropertyInfo).Name;

            var maxLengthAttr = metaData.ContainerType.GetAttributeFrom<TAttribute>(name);
            return maxLengthAttr;
        }

        public static ModelExpressionProvider modelExpressionProvider(IHtmlHelper helper)
        {
            var expresionProvider = helper.ViewContext.HttpContext.RequestServices
                                     .GetService(typeof(ModelExpressionProvider)) as ModelExpressionProvider;

            return expresionProvider;// new ModelExpressionProvider(helper.MetadataProvider);
        }

        private static Dictionary<string, Object> defineAtributosBasicoInput(object htmlAttributes, bool desabilitado, bool requerido = false)
        {
            var attributes = new Dictionary<string, Object>();

            if (htmlAttributes != null)
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
                {
                    attributes.Add(property.Name, property.GetValue(htmlAttributes));
                }
            string strCssClassFormControl = " form-control";

            if (attributes.ContainsKey("class"))
                attributes["class"] += strCssClassFormControl;
            else
                attributes.Add("class", strCssClassFormControl);

            if (desabilitado)
                attributes.Add("disabled", "disabled");

            if (requerido)
                attributes.Add("required", "required");

            return attributes;
        }

        private static Dictionary<string, Object> defineAtributosBasicoCheck(object htmlAttributes, bool desabilitado)
        {
            var attributes = new Dictionary<string, Object>();

            if (htmlAttributes != null)
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
                {
                    attributes.Add(property.Name, property.GetValue(htmlAttributes));
                }
            string strCssClassFormControl = " checkbox";

            if (attributes.ContainsKey("class"))
                attributes["class"] += strCssClassFormControl;
            else
                attributes.Add("class", strCssClassFormControl);


            if (desabilitado)
                attributes.Add("disabled", "disabled");

            return attributes;
        }

        private static Dictionary<string, object> defineAtributosBasicoLabel(object htmlAttributes = null, bool required = false)
        {
            var attributes = new Dictionary<string, object>();

            if (htmlAttributes != null)
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
                {
                    attributes.Add(property.Name, property.GetValue(htmlAttributes));
                }
            string strCssClassFormControl = " text-nowrap";

            if (required)
                strCssClassFormControl += " required";

            if (attributes.ContainsKey("class"))
                attributes["class"] += strCssClassFormControl;
            else
                attributes.Add("class", strCssClassFormControl);

            return attributes;
        }

        #endregion

        #region "Label e Status"
        public static string DisplayShortNameFor<TModel, TClass, TValue>(this IHtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TClass> model, Expression<Func<TModel, TValue>> expression)
        {
            var name = modelExpressionProvider(helper).GetExpressionText(expression);

            name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            var metadata = helper.MetadataProvider.GetMetadataForProperty(typeof(TClass), name);

            name = metadata.SimpleDisplayProperty ?? metadata.DisplayName ?? metadata.PropertyName;

            return new HtmlString(name).Value;
        }

        private static IHtmlContent LabelFwFor<TModel, TProperty>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, bool shortDisplayName = false, object htmlAttributes = null)
        {

            var metadata = modelExpressionProvider(helper).CreateModelExpression(helper.ViewData, expression).Metadata;

            string displayName;
            if (shortDisplayName)
            {
                var attrs = (metadata as Microsoft.AspNetCore.Mvc.ModelBinding.Metadata.DefaultModelMetadata).Attributes;
                var displayAttr = (DisplayAttribute)attrs.Attributes.First(i => i.GetType() == typeof(DisplayAttribute));
                displayName = displayAttr.ShortName;
            }
            else
                displayName = metadata.DisplayName ?? metadata.PropertyName;

            var attributes = defineAtributosBasicoLabel(htmlAttributes, metadata.IsRequired);
            attributes.Add("for", helper.IdFor(expression).ToString());

            var htmlLbl = helper.Label(expression.ToString(), displayName, attributes);
            var builder = new HtmlContentBuilder();
            builder.AppendHtml(htmlLbl);
            if (!String.IsNullOrEmpty(metadata.Description))
            {
                string htmlInformation = string.Format(" <i class=\"fa fa-question-circle text-muted\" data-toggle=\"tooltip\" data-placement=\"auto\" title=\"{0}\"></i>", metadata.Description);
                builder.AppendHtmlLine(htmlInformation);

            }

            return builder;
        }
        public static IHtmlContent FwLabelFor<TModel, TProperty>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return LabelFwFor(helper, expression, false, htmlAttributes);
        }
        public static IHtmlContent FwShortLabelFor<TModel, TProperty>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return LabelFwFor(helper, expression, true, htmlAttributes);
        }

        public static IHtmlContent FwDisplayTipoSituacaoProcessoEnum(this TipoSituacaoProcessoEnum enumVal, bool shortDisplayName = false)
        {
            var metadata = enumVal.GetAttributeOfType<SituacaoProcessoAttribute>();

            var displayName = string.Empty;
            if (shortDisplayName)
                displayName = metadata.ShortName;
            else
                displayName = metadata.Name;


            return new HtmlString(displayName);
        }
        public static IHtmlContent FwDisplayEnum(this Enum enumVal, bool shortDisplayName = false)
        {
            var metadata = enumVal.GetAttributeOfType<DisplayAttribute>();

            var displayName = string.Empty;
            if (shortDisplayName)
                displayName = metadata.ShortName;
            else
                displayName = metadata.Name;


            return new HtmlString(displayName);
        }


        public static IHtmlContent FwStatusFor<TModel, TProperty>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, bool shortDisplayName = false, object htmlAttributes = null)
        {
            var attributes = defineAtributosBasicoInput(htmlAttributes, true);
            attributes["class"] += " " + "text-center ";

            var name = helper.NameFor(expression);
            var valor = helper.ValueFor(expression).ToString();

            var member = ((MemberExpression)expression.Body).Member as PropertyInfo;
            if (member.PropertyType == typeof(bool) || member.PropertyType == typeof(bool?))
            {
                bool bvalor;
                if (bool.TryParse(valor, out bvalor))
                    valor = bvalor ? (shortDisplayName ? "Sim" : "Ativado") : (shortDisplayName ? "Não" : "Desativado");
                else
                    valor = string.Empty;
            }
            else if (member.PropertyType == typeof(int) || member.PropertyType == typeof(int?))
            {
                int ivalor;
                if (int.TryParse(valor, out ivalor))
                {
                    var attributeEnum = ((TipoSituacaoProcessoEnum)ivalor).GetAttributeOfType<SituacaoProcessoAttribute>();
                    if (shortDisplayName)
                        valor = attributeEnum.ShortName;
                    else
                        valor = attributeEnum.Name;
                }
                else
                    valor = string.Empty;
            }
            else if (member.PropertyType == typeof(TipoSituacaoProcessoEnum) || member.PropertyType == typeof(TipoSituacaoProcessoEnum?))
            {
                TipoSituacaoProcessoEnum situacao;
                if (Enum.TryParse(valor, out situacao))
                {
                    var attributeEnum = situacao.GetAttributeOfType<SituacaoProcessoAttribute>();
                    if (shortDisplayName)
                        valor = attributeEnum.ShortName;
                    else
                        valor = attributeEnum.Name;
                }
                else
                    valor = "";

            }

            return helper.TextBox("sts" + name, valor, attributes);
        }

        public static IHtmlContent FwLabel(this IHtmlHelper helper, string id, string label, bool requerido = false, object htmlAttributes = null, string descricaoTooltip = null)
        {
            var attributes = defineAtributosBasicoLabel(htmlAttributes, requerido);
            attributes.Add("for", id);
            attributes.Add("id", id);

            var htmlLbl = helper.Label(id, label, attributes);
            var builder = new HtmlContentBuilder();
            builder.AppendHtml(htmlLbl);
            if (!String.IsNullOrEmpty(descricaoTooltip))
            {
                string htmlInformation = string.Format(" <i class=\"fa fa-question-circle text-muted\" data-toggle=\"tooltip\" data-placement=\"auto\" title=\"{0}\"></i>", descricaoTooltip);
                builder.AppendHtml(htmlInformation);
            }
            return builder;
        }

        public static IHtmlContent Label(this IHtmlHelper helper, string labelText, object htmlAttributes)
        {
            return helper.Label(labelText, labelText, htmlAttributes);
        }

        #endregion

        #region "Text Box"
        public static IHtmlContent FwTextBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, bool desabilitado = false, object htmlAttributes = null)
        {

            var metadata = modelExpressionProvider(helper).CreateModelExpression(helper.ViewData, expression).Metadata;

            var attributes = defineAtributosBasicoInput(htmlAttributes, desabilitado, metadata.IsRequired);
            var maxLengthAttr = helper.ViewData.RetornaAttribute<MaxLengthAttribute, TModel, TProperty>(expression, helper);
            if (maxLengthAttr != null)
                attributes.Add("maxlength", maxLengthAttr.Length.ToString());

            return helper.TextBoxFor(expression, attributes);
        }

        public static IHtmlContent FwTextBox<TModel>(this IHtmlHelper<TModel> helper, string name, string value = "", bool desabilitado = false, object htmlAttributes = null, bool requerido = false)
        {
            if (String.IsNullOrEmpty(name))
                throw new InvalidCastException("Obirgatório a definição de 'name' para o uso do Helper sem 'For<>'");

            var attributes = defineAtributosBasicoInput(htmlAttributes, desabilitado, requerido);

            return helper.TextBox(name, value, attributes);
        }

        #endregion "Text Box"

        #region "Check Box"

        public static IHtmlContent FwCheckBoxFor<TModel>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, bool>> expression, bool desabilitado = false, object htmlAttributes = null)
        {
            var attributes = defineAtributosBasicoCheck(htmlAttributes, desabilitado);


            return helper.CheckBoxFor(expression, attributes);
        }

        public static IHtmlContent FwCheckBox<TModel>(this IHtmlHelper<TModel> helper, string name, bool bChecked = false, bool desabilitado = false, object htmlAttributes = null)
        {
            if (String.IsNullOrEmpty(name))
                throw new InvalidCastException("Obirgatório a definição de 'name' para o uso do Helper sem 'For<>'");

            var attributes = defineAtributosBasicoCheck(htmlAttributes, desabilitado);

            return helper.CheckBox(name, bChecked, attributes);
        }

        #endregion

        public static IHtmlContent FwDateFor<TModel, TProperty>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, bool desabilitado = false, bool calendario = true, object htmlAttributes = null)
        {
            var metadata = modelExpressionProvider(helper).CreateModelExpression(helper.ViewData, expression).Metadata;
            Dictionary<string, object> attributes = new Dictionary<string, object>();
            var divInputGroup = defineAtributosDate(out attributes, desabilitado, metadata.IsRequired, calendario, htmlAttributes);
            divInputGroup.InnerHtml.AppendHtml(helper.TextBoxFor(expression, @"{0:dd/MM/yyyy}", attributes));
            return divInputGroup;
        }
        public static IHtmlContent FwDate<TModel>(this IHtmlHelper<TModel> helper, string name, DateTime? value = null, bool desabilitado = false, bool requerido = false, bool calendario = true, object htmlAttributes = null)
        {
            Dictionary<string, object> attributes = new Dictionary<string, object>();
            var divInputGroup = defineAtributosDate(out attributes, desabilitado, requerido, calendario, htmlAttributes);
            divInputGroup.InnerHtml.AppendHtml(helper.TextBox(name, (value.HasValue ? value : null), @"{0:dd/MM/yyyy}", attributes));

            return divInputGroup;
        }
        private static TagBuilder defineAtributosDate(out Dictionary<string, object> attributes, bool desabilitado = false, bool requerido = false, bool calendario = true, object htmlAttributes = null)
        {
            attributes = defineAtributosBasicoInput(htmlAttributes, desabilitado, requerido);

            attributes["class"] += " " + "text-center";
            attributes.Add("data-mask", "99/99/9999");
            attributes.Add("maxlength", "10");

            var divInputGroup = new TagBuilder("div");
            divInputGroup.AddCssClass("input-group" + (desabilitado | !calendario ? string.Empty : " date"));
            divInputGroup.InnerHtml.AppendHtml("<span class=\"input-group-addon\"><i class=\"fa fa-calendar\"></i></span> ");
            return divInputGroup;

        }

        public static IHtmlContent FwTimeFor<TModel, TProperty>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, bool desabilitado = false, bool relogio = true, object htmlAttributes = null)
        {

            var metadata = modelExpressionProvider(helper).CreateModelExpression(helper.ViewData, expression).Metadata;

            Dictionary<string, object> attributes = new Dictionary<string, object>();
            var divInputGroup = defineAtributosTime(out attributes, desabilitado, metadata.IsRequired, relogio, htmlAttributes);

            divInputGroup.InnerHtml.AppendHtml(helper.TextBoxFor(expression, @"{0:hh\:mm}", attributes));

            return divInputGroup;
        }

        public static IHtmlContent FwTime<TModel>(this IHtmlHelper<TModel> helper, string name, DateTime? value = null, bool desabilitado = false, bool requerido = false, bool relogio = true, object htmlAttributes = null)
        {
            Dictionary<string, object> attributes = new Dictionary<string, object>();
            var divInputGroup = defineAtributosTime(out attributes, desabilitado, requerido, relogio, htmlAttributes);
            divInputGroup.InnerHtml.AppendHtml(helper.TextBox(name, (value.HasValue ? value : null), @"{0:hh\:mm}", attributes));
            return divInputGroup;
        }
        private static TagBuilder defineAtributosTime(out Dictionary<string, object> attributes, bool desabilitado = false, bool requerido = false, bool relogio = true, object htmlAttributes = null)
        {
            attributes = defineAtributosBasicoInput(htmlAttributes, desabilitado, requerido);
            attributes["class"] += " " + "text-center";
            attributes.Add("data-mask", "99:99");
            attributes.Add("maxlength", "10");

            var divInputGroup = new TagBuilder("div");
            divInputGroup.AddCssClass("input-group" + (desabilitado | !relogio ? string.Empty : " clockpicker"));
            divInputGroup.MergeAttribute("data-autoclose", "true");
            divInputGroup.InnerHtml.AppendHtml("<span class=\"input-group-addon\"><i class=\"fa fa-clock-o\"></i></span> ");

            return divInputGroup;

        }

        public static IHtmlContent FwCurrencyFor<TModel, TProperty>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, bool desabilitado = false, object htmlAttributes = null)
        {

            return FwNumberFor(helper, expression, desabilitado, htmlAttributes);
        }

        public static IHtmlContent FwCurrencyFor<TModel, TProperty>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string name, bool desabilitado = false, object htmlAttributes = null)
        {
            if (String.IsNullOrEmpty(name))
                throw new InvalidCastException("Obirgatório a definição de 'name' para o uso do Helper sem 'For<>'");

            return FwNumberFor(helper, expression, name, desabilitado, htmlAttributes);
        }

        public static IHtmlContent FwCurrency<TModel>(this IHtmlHelper<TModel> helper, string name, decimal? value = null, bool desabilitado = false, object htmlAttributes = null, bool requerido = false)
        {
            if (String.IsNullOrEmpty(name))
                throw new InvalidCastException("Obirgatório a definição de 'name' para o uso do Helper sem 'For<>'");

            var attributes = defineAtributosBasicoInput(htmlAttributes, desabilitado, requerido);
            var nomeAtributo = name;

            attributes["class"] += " " + "text-right";
            attributes.Add("type", "number");

            if (!attributes.ContainsKey("step"))
            {
                attributes.Add("step", "0.01");
                attributes.Add("placeholder", "0.00");
            }
            else
            {
                attributes.Add("placeholder", "0");
            }

            var sValue = string.Empty;
            if (value.HasValue)
                sValue = value.Value.ToString(new System.Globalization.CultureInfo("en-US"));

            return helper.TextBox(nomeAtributo, sValue, attributes);
        }


        public static IHtmlContent FwNumberFor<TModel, TProperty>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, bool desabilitado = false, object htmlAttributes = null)
        {
            return FwNumberFor(helper, expression, null, desabilitado, htmlAttributes);
        }

        private static IHtmlContent FwNumberFor<TModel, TProperty>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string name, bool desabilitado = false, object htmlAttributes = null, bool requerido = false)
        {
            var metadata = modelExpressionProvider(helper).CreateModelExpression(helper.ViewData, expression).Metadata;

            var attributes = defineAtributosBasicoInput(htmlAttributes, desabilitado, metadata.IsRequired);
            var nomeAtributo = string.IsNullOrEmpty(name) ? helper.NameFor(expression).ToString() : name;

            var maxLengthAttr = helper.ViewData.RetornaAttribute<MaxLengthAttribute, TModel, TProperty>(expression, helper);
            if (maxLengthAttr != null)
                attributes.Add("maxlength", maxLengthAttr.Length.ToString());

            var rangeAttr = helper.ViewData.RetornaAttribute<RangeAttribute, TModel, TProperty>(expression, helper);
            if (rangeAttr != null)
            {
                attributes.Add("min", rangeAttr.Minimum);
                attributes.Add("max", rangeAttr.Maximum);
            }
            attributes["class"] += " " + "text-right";
            attributes.Add("type", "number");


            var value = string.Empty;
            var member = ((MemberExpression)expression.Body).Member as PropertyInfo;
            bool IndValorCampoInteiro = (member.PropertyType == typeof(int) || member.PropertyType == typeof(int?));

            if (IndValorCampoInteiro)
            {

                if (!attributes.ContainsKey("step"))
                    attributes.Add("step", "1");

                int ivalor;
                if (int.TryParse(helper.ValueFor(expression).ToString(), out ivalor))
                    value = ivalor.ToString(new System.Globalization.CultureInfo("en-US"));
            }
            else
            {

                if (member.PropertyType != typeof(decimal) && member.PropertyType != typeof(decimal?) && member.PropertyType != typeof(System.Decimal) && member.PropertyType != typeof(System.Decimal?))
                {
                    throw new InvalidCastException("Uso do Helper deve ser aplicado somente para campo do tipo Decimal");
                }

                if (!attributes.ContainsKey("step"))
                    attributes.Add("step", "0.01");

                decimal dvalor;
                if (decimal.TryParse(helper.ValueFor(expression).ToString(), out dvalor))
                    value = dvalor.ToString(new System.Globalization.CultureInfo("en-US"));

            }

            return helper.TextBox(nomeAtributo, value, attributes);
        }

        public static IHtmlContent FwTextAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, bool desabilitado = false, object htmlAttributes = null)
        {

            var metadata = modelExpressionProvider(helper).CreateModelExpression(helper.ViewData, expression).Metadata;


            var attributes = defineAtributosBasicoInput(htmlAttributes, desabilitado, metadata.IsRequired);

            var maxLengthAttr = helper.ViewData.RetornaAttribute<MaxLengthAttribute, TModel, TProperty>(expression, helper);
            if (maxLengthAttr != null)
                attributes.Add("maxlength", maxLengthAttr.Length.ToString());

            if (!attributes.ContainsKey("rows"))
                attributes.Add("rows", "5");

            return helper.TextAreaFor(expression, attributes);
        }

        public static IHtmlContent FwPhoneBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, bool desabilitado = false, object htmlAttributes = null)
        {

            var metadata = modelExpressionProvider(helper).CreateModelExpression(helper.ViewData, expression).Metadata;

            var attributes = defineAtributosBasicoInput(htmlAttributes, desabilitado, metadata.IsRequired);
            var maxLengthAttr = helper.ViewData.RetornaAttribute<MaxLengthAttribute, TModel, TProperty>(expression, helper);
            if (maxLengthAttr != null)
                attributes.Add("maxlength", maxLengthAttr.Length.ToString());
            attributes.Add("data-mask", "(99)99999999?9");
            return helper.TextBoxFor(expression, attributes);
        }

        public static IHtmlContent FwTempoDecorridoFor<TModel, TProperty>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, DateTime dataComparacao)
        {

            DateTime dvalor;
            if (DateTime.TryParse(helper.ValueFor(expression).ToString(), out dvalor))
                return HtmlGridHelper.TempoDecorrido(dvalor, dataComparacao);
            else
                return null;
        }
        public static IHtmlContent FwTempoDecorridoFor<TModel, TProperty>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            return FwTempoDecorridoFor(helper, expression, DateTime.Now);
        }


        public static IHtmlContent FwEnumDropDownList<TModel>(this IHtmlHelper<TModel> helper, string name, string optionLabel, Type enumerador, bool desabilitado = false, bool requerido = false, object htmlAttributes = null)
        {

            var attributes = defineAtributosBasicoInput(htmlAttributes, desabilitado, requerido);

            var selectList = new List<SelectListItem>();

            foreach (var enumvalue in Enum.GetValues(enumerador))
            {
                selectList.Add(new SelectListItem
                {
                    Text = ((Enum)enumvalue).FwDisplayEnum().ToString(),
                    Value = enumvalue.ToString()
                });
            }


            return helper.DropDownList(name, selectList, optionLabel, attributes);
        }
        public static IHtmlContent FwEnumDropDownList<TModel>(this IHtmlHelper<TModel> helper, string name, string optionLabel, Enum valor, bool desabilitado = false, bool requerido = false, object htmlAttributes = null)
        {

            var attributes = defineAtributosBasicoInput(htmlAttributes, desabilitado, requerido);

            var selectList = new List<SelectListItem>();


            var enumerador = valor.GetType();

            foreach (var enumvalue in Enum.GetValues(enumerador))
            {
                selectList.Add(new SelectListItem
                {
                    Text = ((Enum)enumvalue).FwDisplayEnum().ToString(),
                    Value = enumvalue.ToString(),
                    Selected = enumvalue.Equals(valor)

                });
            }


            return helper.DropDownList(name, selectList, optionLabel, attributes);
        }


    }
}

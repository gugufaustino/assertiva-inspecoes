using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Differencial.Domain.Util.ExtensionMethods
{
    public static class MvcExtensions
    {
        /// <summary>
        /// Por default Insere um Campo vazio no select
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="list"></param>
        /// <param name="value"></param>
        /// <param name="text"></param>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> ToSelectList<TModel>(this IEnumerable<TModel> list, Func<TModel, object> value, Func<TModel, object> text, object selectedValue = null)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            selectList.Add(new SelectListItem { Value = string.Empty, Text = string.Empty });
            selectList.AddRange(list.Select(e => new SelectListItem() { Value = Convert.ToString(value.Invoke(e)), Text = Convert.ToString(text.Invoke(e)) }));
            return new SelectList(selectList, "Value", "Text", selectedValue);
        }

        public static IEnumerable<SelectListItem> ToSelectList<TModel>(this IEnumerable<TModel> list, Func<TModel, object> value, Func<TModel, object> text, bool inserirVazio, object selectedValue = null)
        {
            IEnumerable<SelectListItem> selectList = list.Select(e => new SelectListItem() { Value = Convert.ToString(value.Invoke(e)), Text = Convert.ToString(text.Invoke(e)) });

            if (inserirVazio)
                return ToSelectList<TModel>(list, value, text, selectedValue);

            return new SelectList(selectList, "Value", "Text", selectedValue);
        }

        public static SelectList GetEnumSelectList<E>()
        {
            Array values = Enum.GetValues(typeof(E));
            var items = new List<SelectListItem>(values.Length);

            foreach (var i in values)
            {
                items.Add(new SelectListItem
                {
                    Text = Enum.GetName(typeof(E), i),
                    Value = i.ToString()
                });
            }

            return new SelectList(items);
        }

    }

    public static class CoreExtensions
    {
        public static IDictionary<string, object> ToDictionary(this object data)
        {
            if (data == null) return new Dictionary<string, object>();

            BindingFlags publicAttributes = BindingFlags.Public | BindingFlags.Instance;
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            foreach (PropertyInfo property in
                     data.GetType().GetProperties(publicAttributes))
            {
                if (property.CanRead)
                {
                    dictionary.Add(property.Name, property.GetValue(data, null));
                }
            }
            return dictionary;
        }
    }
}

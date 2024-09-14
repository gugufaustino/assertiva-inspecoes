
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Threading.Tasks;

public class DecimalModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

        var valueAsString = valueProviderResult.FirstValue;
        if (string.IsNullOrWhiteSpace(valueAsString))
        {
            return Task.CompletedTask;
        }
        //  actualValue = Convert.ToDecimal(valueResult.AttemptedValue, new CultureInfo("en-US"));
        // Tentar converter o valor usando a cultura do Brasil
        if (decimal.TryParse(valueAsString, NumberStyles.Currency, new CultureInfo("en-US"), out var result))
        {
            var ptValue = Convert.ToDecimal(result, new CultureInfo("en-US"));
            bindingContext.Result = ModelBindingResult.Success(ptValue);
        }
        else
        {
            bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "O valor fornecido não é válido.");
        }

        return Task.CompletedTask;
    }
}



//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Globalization;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using Microsoft.AspNetCore.Mvc;

//namespace Differencial.Web
//{
//    public class DecimalModelBinder : IModelBinder
//    {
//        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
//        {
//            ValueProviderResult valueResult = bindingContext.ValueProvider
//                .GetValue(bindingContext.ModelName);

//            ModelState modelState = new ModelState { Value = valueResult };

//            object actualValue = null;

//            if (valueResult.AttemptedValue != string.Empty)
//            {
//                try
//                {
//                    actualValue = Convert.ToDecimal(valueResult.AttemptedValue, new CultureInfo("en-US"));
//                }
//                catch (FormatException e)
//                {
//                    modelState.Errors.Add(e);
//                }
//            }

//            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);

//            return actualValue;
//        }

//    }



//}
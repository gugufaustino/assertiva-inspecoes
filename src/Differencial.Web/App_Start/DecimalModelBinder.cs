﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace Differencial.Web
{
    public class DecimalModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);

            ModelState modelState = new ModelState { Value = valueResult };

            object actualValue = null;

            if (valueResult.AttemptedValue != string.Empty)
            {
                try
                {
                    actualValue = Convert.ToDecimal(valueResult.AttemptedValue, new CultureInfo("en-US"));
                }
                catch (FormatException e)
                {
                    modelState.Errors.Add(e);
                }
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);

            return actualValue;
        }

    }
}
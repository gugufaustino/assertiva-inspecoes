using Differencial.Domain.UOW;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace Differencial.Web.Filters
{
    public class Validacao : ActionFilterAttribute
    {
        public bool IgnorarId { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var modelState = filterContext.ModelState;

            if (IgnorarId)
            {
                var keys = modelState.Keys.Where(x => x.Equals("Id") || x.EndsWith(".Id"));
                foreach (var key in keys)
                {
                    modelState[key].Errors.Clear();
                }
            }

            base.OnActionExecuting(filterContext);

        } 
    }
}
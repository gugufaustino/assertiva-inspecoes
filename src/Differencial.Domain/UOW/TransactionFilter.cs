using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Differencial.Domain.UOW
{
    public class TransactionFilter : ActionFilterAttribute 
    {
        private readonly IUnitOfWork unitOfWork;

        public TransactionFilter(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Nova Instancia é criada para cada Request HTTP           
            unitOfWork.BeginTransaction();

            base.OnActionExecuting(filterContext);

        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                if (filterContext.Exception != null)
                    throw filterContext.Exception;

                if (filterContext.Exception == null && filterContext.Result is JsonResult)
                {
                    var result = (JsonResult)filterContext.Result;
                    dynamic dynResult = ((dynamic)result.Value);

                    if (dynResult.success || filterContext.Result is ViewResult)
                        unitOfWork.CommitTransaction();
                    else
                        unitOfWork.RollbackTransaction();
                }
                else if (filterContext.Exception == null &&
                    (filterContext.Result is RedirectToRouteResult
                    || filterContext.Result is RedirectToActionResult
                    || filterContext.Result is RedirectResult
                    || (filterContext.Result is ViewResult result && result.ViewData.ModelState.IsValid)))
                {
                    unitOfWork.CommitTransaction();
                }
                else if (filterContext.Exception != null && !filterContext.ExceptionHandled)
                {
                    unitOfWork.RollbackTransaction();
                }
            }
            catch (Exception)
            {
                unitOfWork.RollbackTransaction();
            }
            base.OnActionExecuted(filterContext);
        }
    }
}
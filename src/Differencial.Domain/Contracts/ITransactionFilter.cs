using Microsoft.AspNetCore.Mvc.Filters;

namespace Differencial.Domain.Contracts
{
    public interface ITransactionFilter
    {
        void OnActionExecuted(ActionExecutedContext filterContext);
        void OnActionExecuting(ActionExecutingContext filterContext);
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using Core.Constants;

namespace Api.Validators
{
    public class ValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var message = string.Join(" | ", context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                context.Result = new BadRequestObjectResult(new { error = ErrorCodes.ErrorInvalidInput(message) });
            }
        }
    }
}

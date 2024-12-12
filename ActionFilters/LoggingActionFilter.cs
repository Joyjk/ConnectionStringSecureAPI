using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
namespace ConnectionStringSecureAPI.ActionFilters
{
    public class LoggingActionFilter : IActionFilter
    {
        private readonly ILogger<LoggingActionFilter> _logger;

        public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Action {ActionName} is about to be executed", context.ActionDescriptor.DisplayName);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Action {ActionName} has been executed", context.ActionDescriptor.DisplayName);
        }
    }

}

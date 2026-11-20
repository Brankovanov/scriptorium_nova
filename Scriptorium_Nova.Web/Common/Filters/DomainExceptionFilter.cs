using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Scriptorium_Nova.Web.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Scriptorium_Nova.Web.Common.Filters;

public class DomainExceptionFilter : IExceptionFilter
{
    private readonly ILogger<DomainExceptionFilter> _logger;

    public DomainExceptionFilter(ILogger<DomainExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case NotFoundException ex:
                _logger.LogInformation("Not found: {Message}", ex.Message);
                context.Result = new NotFoundResult();
                context.ExceptionHandled = true;
                break;

            case ForbiddenException ex:
                _logger.LogInformation("Forbidden: {Message}", ex.Message);
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                context.ExceptionHandled = true;
                break;
        }
    }
}
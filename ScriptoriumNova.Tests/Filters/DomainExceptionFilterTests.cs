using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging.Abstractions;
using Scriptorium_Nova.Web.Common.Exceptions;
using Scriptorium_Nova.Web.Common.Filters;

namespace ScriptoriumNova.Tests.Filters;

public class DomainExceptionFilterTests
{
    private readonly DomainExceptionFilter _filter =
        new(NullLogger<DomainExceptionFilter>.Instance);

    private static ExceptionContext CreateContext(Exception exception)
    {
        var actionContext = new ActionContext(
            new DefaultHttpContext(), new RouteData(), new ActionDescriptor());

        return new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = exception
        };
    }

    [Fact]
    public void OnException_UnrelatedException_IsLeftUnhandled()
    {
        var context = CreateContext(new InvalidOperationException("bug"));

        _filter.OnException(context);

        Assert.Null(context.Result);
        Assert.False(context.ExceptionHandled);
    }

    [Fact]
    public void OnException_NotFoundException_SetsNotFoundResultAndHandles()
    {
        var context = CreateContext(new NotFoundException("Entry 5 not found."));

        _filter.OnException(context);

        var result = Assert.IsType<NotFoundResult>(context.Result);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        Assert.True(context.ExceptionHandled);
    }

    [Fact]
    public void OnException_ForbiddenException_SetsForbiddenStatusAndHandles()
    {
        var context = CreateContext(new ForbiddenException("Role too low."));

        _filter.OnException(context);

        var result = Assert.IsType<StatusCodeResult>(context.Result);
        Assert.Equal(StatusCodes.Status403Forbidden, result.StatusCode);
        Assert.True(context.ExceptionHandled);
    }
}
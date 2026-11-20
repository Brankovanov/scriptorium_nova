using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Scriptorium_Nova.Web.Controllers;
using Scriptorium_Nova.Web.Services.Interfaces;

namespace ScriptoriumNova.Tests.Controllers;

public class HomeControllerTests
{
    private static HomeController CreateController() => new(
        Substitute.For<IEntryService>(),
        Substitute.For<IEntryTypeService>())
    {
        ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
    };

    [Theory]
    [InlineData(404, "NotFound")]
    [InlineData(403, "Forbidden")]
    [InlineData(500, "Error")]
    public void HttpStatus_ReturnsMatchingView(int code, string expectedView)
    {
        HomeController controller = CreateController();

        ViewResult result = Assert.IsType<ViewResult>(controller.HttpStatus(code));

        Assert.Equal(expectedView, result.ViewName);
    }
}
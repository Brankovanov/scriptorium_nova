using Scriptorium_Nova.Web.Common.Exceptions;

public class ForbiddenException : DomainException
{
    public ForbiddenException(string message) : base(message) { }
}
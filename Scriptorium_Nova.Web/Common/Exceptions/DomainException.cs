namespace Scriptorium_Nova.Web.Common.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
}
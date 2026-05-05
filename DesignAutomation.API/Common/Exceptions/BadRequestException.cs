namespace DesignAutomation.API.Common.Exceptions;

public class BadRequestException : Exception
{
    public IReadOnlyDictionary<string, string[]>? Errors { get; }

    public BadRequestException(string message) : base(message) { }

    public BadRequestException(string message, IReadOnlyDictionary<string, string[]> errors) : base(message)
    {
        Errors = errors;
    }
}

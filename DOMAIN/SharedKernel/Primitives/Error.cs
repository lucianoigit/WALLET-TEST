namespace DOMAIN.SharedKernel.Primitives;

public record Error
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.", ErrorType.Failure);

    public string? Code { get; }
    public string? Message { get; }
    public ErrorType Type { get; }


    public Error(string code, string message, ErrorType errorType)
    {
        Code = code;
        Message = message;
        Type = errorType;
    }

    public static Error NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    public static Error Validation(string code, string description) =>
        new(code, description, ErrorType.Validation);

    public static Error Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);

    public static Error Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);
}

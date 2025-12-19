namespace GestaoLogisticaAPI.Domain.Common;

public class Result<T>
{
    public T? Value { get; private set; }
    public bool IsSuccess { get; private set; }
    public bool IsFailure => !IsSuccess;
    public string? ErrorMessage { get; private set; }

    // Construtor privado para sucesso
    private Result(T value)
    {
        Value = value;
        IsSuccess = true;
        ErrorMessage = null;
    }

    // Construtor privado para falha
    private Result(string errorMessage)
    {
        Value = default;
        ErrorMessage = errorMessage;
        IsSuccess = false;
    }

    // Métodos estáticos de fábrica
    public static Result<T> Success(T value) => new Result<T>(value);
    public static Result<T> Failure(string errorMessage) => new Result<T>(errorMessage);
}
    
public class Result
{
    public bool IsSuccess { get; private set; }
    public bool IsFailure => !IsSuccess;
    public string? ErrorMessage { get; private set; }

    private Result() { IsSuccess = true; ErrorMessage = null; }
    private Result(string errorMessage) { IsSuccess = false; ErrorMessage = errorMessage; }

    public static Result Ok() => new Result();
    public static Result Failure(string errorMessage) => new Result(errorMessage);
}
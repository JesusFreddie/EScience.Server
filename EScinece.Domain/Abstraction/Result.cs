namespace EScinece.Domain.Abstraction;

public readonly struct Result<TValue, TError>
{
    private readonly bool _success;
    public readonly TValue Value;
    public readonly TError Error;

    private Result(TValue v, TError e, bool success)
    {
        Value = v;
        Error = e;
        _success = success;
    }

    public bool IsOk => _success;

    public static Result<TValue, TError> Ok(TValue v)
    {
        return new(v, default(TError), true);
    }

    public static Result<TValue, TError> Err(TError e)
    {
        return new(default(TValue), e, false);
    }

    public static implicit operator Result<TValue, TError>(TValue v) => new(v, default(TError), true);
    public static implicit operator Result<TValue, TError>(TError e) => new(default(TValue), e, false);

    public R Match<R>(
        Func<TValue, R> success,
        Func<TError, R> failure) =>
        _success ? success(Value) : failure(Error);
}
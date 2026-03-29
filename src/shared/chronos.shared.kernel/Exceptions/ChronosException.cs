using System.Net;

namespace chronos.shared.kernel.Exceptions;

/// <summary>
/// Base exception for all Chronos domain exceptions.
/// </summary>
public abstract class ChronosException(
    string code,
    string[]? @params = null) : Exception(code)
{
    public abstract HttpStatusCode StatusCode { get; } 
    
    /// <summary>
    /// Gets the error code.
    /// </summary>
    public string Code { get; } = code;

    /// <summary>
    /// Collection of parameters
    /// </summary>
    public string[] Params { get; } = @params ?? [];
}

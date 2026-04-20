using System.Net;
using System.Text.RegularExpressions;

namespace chronos.shared.kernel.Exceptions;

/// <summary>
/// Exception thrown when a requested entity is not found.
/// </summary>
public sealed partial class NotFoundException(
    string entityName,
    string[]? @params = null) : ChronosException($"{ToSnakeCase(entityName)}_not_found", @params)
{
    public override HttpStatusCode StatusCode { get; } = HttpStatusCode.NotFound;

    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        return SnakeCaseRegex().Replace(input, "$1_$2").ToLowerInvariant();
    }

    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex SnakeCaseRegex();
}

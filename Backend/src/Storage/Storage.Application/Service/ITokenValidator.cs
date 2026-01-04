using System.Diagnostics.CodeAnalysis;
using Storage.Application.Model;

namespace Storage.Application.Service;

public interface ITokenValidator
{
    public bool TryValidate(string? value, [NotNullWhen(true)] out IFileToken? token);
}

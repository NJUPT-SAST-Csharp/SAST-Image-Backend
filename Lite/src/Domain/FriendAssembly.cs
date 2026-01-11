using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Infrastructure"), InternalsVisibleTo("Domain.Tests")]

namespace Domain;

public static class DomainAssembly
{
    public static Assembly Assembly => typeof(DomainAssembly).Assembly;
}

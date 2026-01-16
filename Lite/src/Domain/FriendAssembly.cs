using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Infrastructure"), InternalsVisibleTo("Domain.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Domain;

public static class DomainAssembly
{
    public static Assembly Assembly => typeof(DomainAssembly).Assembly;
}

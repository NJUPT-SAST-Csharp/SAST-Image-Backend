using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SastImg.Infrastructure")]

namespace SastImg.Application;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}

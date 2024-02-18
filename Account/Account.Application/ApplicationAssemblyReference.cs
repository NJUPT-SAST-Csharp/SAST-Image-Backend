using System.Reflection;

namespace Account.Application
{
    public static class ApplicationAssemblyReference
    {
        public static Assembly Assembly => typeof(ApplicationAssemblyReference).Assembly;
    }
}

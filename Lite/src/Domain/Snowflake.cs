using IdGen;

namespace Domain;

public static class Snowflake
{
    private static readonly IdGenerator _generator = new(233);

    public static long NewId => _generator.First();
}

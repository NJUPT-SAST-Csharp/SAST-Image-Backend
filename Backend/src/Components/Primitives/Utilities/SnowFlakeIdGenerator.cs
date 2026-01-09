using IdGen;

namespace Primitives.Utilities;

public static class SnowFlakeIdGenerator
{
    private static readonly IdGenerator _idGenerator = new(233);
    public static long NewId => _idGenerator.First();
}

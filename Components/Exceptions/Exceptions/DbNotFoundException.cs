using System.Collections;

namespace Exceptions.Exceptions;

public sealed class DbNotFoundException(string entityName, string id) : Exception
{
    public override string Message => $"Couldn't find [{entityName}] with id [{id}].";

    public override IDictionary Data => new Hashtable() { { entityName, id } };
}

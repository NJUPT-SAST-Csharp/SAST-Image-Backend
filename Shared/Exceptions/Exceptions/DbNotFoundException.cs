using System.Collections;

namespace Exceptions.Exceptions
{
    public sealed class DbNotFoundException(string entityName, string id) : Exception
    {
        private readonly string _id = id;
        private readonly string _entity = entityName;

        public override string Message => $"Couldn't find [{_entity}] with id [{_id}].";

        public override IDictionary Data => new Hashtable() { { _entity, _id } };
    }
}

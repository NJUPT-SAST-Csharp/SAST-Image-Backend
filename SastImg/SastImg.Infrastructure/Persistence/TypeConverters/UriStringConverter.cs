using System.Data;
using Dapper;

namespace SastImg.Infrastructure.Persistence.TypeConverters
{
    internal class UriStringConverter : SqlMapper.TypeHandler<Uri>
    {
        public override void SetValue(IDbDataParameter parameter, Uri value)
        {
            parameter.Value = value.ToString();
        }

        public override Uri Parse(object value)
        {
            return new Uri((string)value);
        }
    }
}

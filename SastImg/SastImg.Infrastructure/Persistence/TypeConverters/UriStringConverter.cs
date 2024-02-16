using Dapper;
using System.Data;

namespace SastImg.Infrastructure.Persistence.TypeConverters
{
    internal class UriStringConverter : SqlMapper.TypeHandler<Uri?>
    {
        public override void SetValue(IDbDataParameter parameter, Uri? value)
        {
            parameter.Value = value?.ToString();
        }

        public override Uri? Parse(object value)
        {
            if (string.IsNullOrWhiteSpace(value as string))
                return null;
            else
                return new Uri((string)value);
        }
    }
}

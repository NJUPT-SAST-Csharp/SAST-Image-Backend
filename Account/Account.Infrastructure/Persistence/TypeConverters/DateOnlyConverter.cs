using System.Data;
using Dapper;

namespace Account.Infrastructure.Persistence.TypeConverters;

internal sealed class DateOnlyConverter : SqlMapper.TypeHandler<DateOnly?>
{
    public override DateOnly? Parse(object value)
    {
        if (value is null)
        {
            return null;
        }
        else
        {
            return DateOnly.FromDateTime((DateTime)value);
        }
    }

    public override void SetValue(IDbDataParameter parameter, DateOnly? value)
    {
        parameter.Value = value.HasValue ? value.Value.ToDateTime(TimeOnly.MinValue) : null;
    }
}

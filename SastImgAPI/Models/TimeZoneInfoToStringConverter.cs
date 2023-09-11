using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SastImgAPI.Models
{
    public class TimeZoneInfoToStringConverter : ValueConverter<TimeZoneInfo, string>
    {
        public TimeZoneInfoToStringConverter(ConverterMappingHints? mappingHints = null)
            : base(tz => tz.Id, str => TimeZoneInfo.FindSystemTimeZoneById(str), mappingHints) { }
    }
}

using Microsoft.AspNetCore.Identity;
using SastImgAPI.Services;

namespace SastImgAPI.Models.Identity
{
    public class Role : IdentityRole<long>
    {
        public override long Id { get; set; } = CodeAccessor.GenerateSnowflakeId;
    }
}

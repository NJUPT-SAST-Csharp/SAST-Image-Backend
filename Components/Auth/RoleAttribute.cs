using Identity;

namespace Auth;

/// <summary>
/// Attribute to specify the roles required for the specific controller or endpoint.
/// </summary>
/// <remarks>
/// Registration for Auth is required through: <br/>
/// <see cref="AuthorizationExtension.UseInternalAuth(Microsoft.AspNetCore.Builder.IApplicationBuilder)"/>
/// </remarks>
/// <param name="allowedRoles">
/// The roles that are allowed to access the controller or endpoint.
/// <example>
///     <list type="bullet|number|table">
///         <item>
///             <term><c>[Role(Roles.ADMIN, Roles.USER)]</c></term>
///             <description>requires either ADMIN or USER role.</description>
///         </item>
///         <item>
///             <term><c>[Role(Roles.ADMIN | Roles.USER)]</c></term>
///             <description>requires both ADMIN and USER allowedRoles.</description>
///         </item>
///         <item>
///             <term><c>[Role(Roles.NONE)]</c> or <c>[Role()]</c></term>
///             <description>allows access to everyone.</description>
///         </item>
///     </list>
/// </example>
/// </param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class RoleAttribute(params Roles[] allowedRoles) : Attribute, IRolesData
{
    public Roles[] Roles { get; } = allowedRoles;
}

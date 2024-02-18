using Microsoft.AspNetCore.Authorization;

namespace Auth.Authorization.Extensions
{
    public static class AuthorizationBuilderExtension
    {
        /// <summary>
        /// Add <b>USER</b> and <b>ADMIN</b> policies.
        /// </summary>
        /// <remarks>
        ///   <br><b>USER</b></br>
        ///   <br>Include required claim <b>Id</b></br>
        ///   <br>Include required claim <b>Username</b></br>
        ///   <br>Include required claim <b>Roles</b> -> <b>USER</b></br>
        ///   <para/>
        ///   <br><b>ADMIN</b></br>
        ///   <br>Include required claim <b>Id</b></br>
        ///   <br>Include required claim <b>Username</b></br>
        ///   <br>Include required claim <b>Roles</b> -> <b>ADMIN</b></br>
        /// </remarks>
        public static AuthorizationBuilder AddBasicPolicies(this AuthorizationBuilder builder)
        {
            builder
                .AddDefaultPolicy(
                    AuthorizationRole.AUTH.ToString(),
                    policy =>
                        policy
                            .RequireAuthenticatedUser()
                            .RequireClaim("Username")
                            .RequireClaim("Roles")
                            .RequireClaim("Id")
                )
                .AddPolicy(
                    AuthorizationRole.USER.ToString(),
                    policy =>
                        policy
                            .RequireAuthenticatedUser()
                            .RequireClaim("Username")
                            .RequireClaim("Roles", AuthorizationRole.USER.ToString())
                            .RequireClaim("Id")
                )
                .AddPolicy(
                    AuthorizationRole.ADMIN.ToString(),
                    policy =>
                        policy
                            .RequireAuthenticatedUser()
                            .RequireClaim("Username")
                            .RequireClaim("Roles", AuthorizationRole.ADMIN.ToString())
                            .RequireClaim("Id")
                );
            return builder;
        }
    }
}

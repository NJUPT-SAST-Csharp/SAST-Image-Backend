using Microsoft.AspNetCore.Authorization;

namespace Auth.Authorization.Extensions
{
    public static class AuthorizationBuilderExtension
    {
        /// <summary>
        /// Add <b>User</b> and <b>Admin</b> policies.
        /// </summary>
        /// <remarks>
        ///   <br><b>User</b></br>
        ///   <br>Include required claim <b>Id</b></br>
        ///   <br>Include required claim <b>Username</b></br>
        ///   <br>Include required claim <b>Roles</b> -> <b>User</b></br>
        ///   <para/>
        ///   <br><b>Admin</b></br>
        ///   <br>Include required claim <b>Id</b></br>
        ///   <br>Include required claim <b>Username</b></br>
        ///   <br>Include required claim <b>Roles</b> -> <b>Admin</b></br>
        /// </remarks>
        public static AuthorizationBuilder AddBasicPolicies(this AuthorizationBuilder builder)
        {
            builder
                .AddPolicy(
                    AuthorizationRole.Auth.ToString(),
                    policy =>
                        policy
                            .RequireAuthenticatedUser()
                            .RequireClaim("Username")
                            .RequireClaim("Roles")
                            .RequireClaim("Id")
                )
                .AddPolicy(
                    AuthorizationRole.User.ToString(),
                    policy =>
                        policy
                            .RequireAuthenticatedUser()
                            .RequireClaim("Username")
                            .RequireClaim("Roles", AuthorizationRole.User.ToString())
                            .RequireClaim("Id")
                )
                .AddPolicy(
                    AuthorizationRole.Admin.ToString(),
                    policy =>
                        policy
                            .RequireAuthenticatedUser()
                            .RequireClaim("Username")
                            .RequireClaim("Roles", AuthorizationRole.Admin.ToString())
                            .RequireClaim("Id")
                );
            return builder;
        }

        /// <summary>
        ///  Add <b>Registrant</b> policy
        /// </summary>
        /// <remarks>
        ///  <br>Include required claim <b>Email</b></br>
        ///  <br>Include required claim <b>Roles</b> -> <b>Registrant</b></br>
        /// </remarks>
        public static AuthorizationBuilder AddRegistrantPolicy(this AuthorizationBuilder builder)
        {
            builder.AddPolicy(
                AuthorizationRole.Registrant.ToString(),
                policy =>
                    policy
                        .RequireAuthenticatedUser()
                        .RequireClaim("Email")
                        .RequireClaim("Roles", AuthorizationRole.Registrant.ToString())
            );
            return builder;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PageTurnerAPI.Services
{
    public static class AuthorizationPolicy
    {

        public static void AddAdminPolicy(this AuthorizationOptions options, string authScheme)
        {
            options.AddPolicy("adminRole", policy =>
            {
                policy.RequireAuthenticatedUser()
                      .AddAuthenticationSchemes(authScheme)
                      .RequireClaim("user_type", "Admin");
            });
        }

        public static void CheckUserPolicy(this AuthorizationOptions options, string authScheme)
        {
            options.AddPolicy("username", policy =>
            {
                policy.RequireAuthenticatedUser()
                      .AddAuthenticationSchemes(authScheme);
            });
        }

    }
}